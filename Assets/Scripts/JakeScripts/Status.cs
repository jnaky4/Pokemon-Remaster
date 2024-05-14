﻿using System.Collections.Generic;

/*
 * Burn:
 *  self_damage 1/8 .125 of max hp
 *  current_damage_physical * 1/2      .5
 *  ignore_type fire
 *  persistence true
 *  applies after both players attacked that turn
 *
 **no early pokemon have freeze abilities**
 * Freeze:
 *  unable_to_attack: 100% chance
 *  fire moves thaw freeze
 *  removal_chance: 20% chance beginning of turn
 *  ignore_type ice
 *  persistence true
 *
 * Paralysis:
 *  current speed * 1/2
 *  unable_to_attack: 25% chance
 *  ignore_type electric
 *  persistence true
 *
 * Poison:
 *  self_damage 1/8 .125 of max hp
 *  ignore_type poison
 *  persistence true
 *  Overworld affect: Damage
 *
 * Sleep:
 *  unable_to_attack: 100%
 *  max_duration: 3 turns
 *  min_duration: 1
 *  persistance true
 *  removal_chance
 *
 * Confusion:
 *  max_duration: 4
 *  min_duration: 2
 *  removal_chance: 0
 *  persistance: false
 *  unable_to_attack: 33% .33
 *  self_damage: .175
 *
 *
 * NOT HANDLED:
 *  Poison in Overworld,
 *  when status is applied during fight ie,
 *  beginning of turn/end of turn,
 *  Special Cases for specific moves
 *
*/

namespace Pokemon
{
    public class Status
    {
        public static Dictionary<string, Status> all_status_effects = new()
        {
            /*
             * name: name of status effect
             * persist: t/f if status effect persist outside of combat
             * ignore_type: ignores applying effect to pokemon if type1 or type2 matches
             * self_damage: (0 to 1) % damage at end of turn to self based on max_hp
             * unable_to_attack_chance: (0 to 1) % chance  to be able to attack that turn
             * affect_stat: specific effect the status effects
             * affect_stat_multiplier: (0 to 1) mutiplier on stat
             * removal_chance: (0 to 1) chance per turn to remove status at beginning of turn
             * min_duration: (1 to 5):  -1 means always persist
             * max_duration: (1 to 5):  -1 means always persist
             */
            //                          name         adj            persist ig_type     s_dmg%  u_atk%  afct_st     afct_st_mul rmv_ch  min_dur max_dur
            {"Burn",  new Status(       "Burn",      "Burned",      true,   "Fire",     .125,   0,      "Attack",   .5,         0,      -1,     -1)},
            {"Freeze",  new Status(     "Freeze",    "Frozen",      true,   "Ice",      0,      1.0,    "null",     1,          .2,     -1,     -1)},
            {"Sleep",  new Status(      "Sleep",     "Asleep",      true,   "null",     0,      1.0,    "null",     1,          .4,      6,      6)},
            {"Paralysis", new Status(   "Paralysis", "Paralyzed",   true,   "Electric", 0,      .25,    "Speed",    .5,         0,      -1,     -1)},
            {"Poison", new Status(      "Poison",    "Poisoned",    true,   "Poison",   .125,   0,      "null",     1,          0,      -1,     -1)},
            {"Flinch", new Status(      "Flinch",    "Flinched",    false,  "null",     0,      1.0,    "null",     1,          0,      1,      1)},
            {"Confusion", new Status(   "Confusion", "Confused",    false,  "null",     .175,   .33,    "null",     1,          0,      3,      6)},
            {"Seeded", new Status(      "Seeded",    "Seeded",      false,  "null",     .0625,  0,      "null",     1,          0,      -1,     -1)},
            {"Recharging", new Status(  "Recharging", "Recharge",   false,  "null",     0,      1.0,    "null",     1,          0,      2,      2)},
            //for csv, does nothing
            {"null", new Status(        "null",       "null",       false,  "null",     0,      0,      "null",     1,          0,      -1,     -1)},
            {"immune", new Status(      "immune",     "null",       false,  "null",     0,      0,      "null",     1,          0,      -1,     -1) }
        };

        public string name;
        public string adj;
        //if the move persist outside battle/switched out
        public bool persistence;
        public string ignore_type;
        public double self_damage;
        public double unable_to_attack_chance;
        public string affect_stat;
        public double affect_stat_mulitplier;
        public double removal_chance;
        public int max_duration;
        public int min_duration;
        public int remaining_turns = 0;

        public Status(string name, string adj, bool persistance, string ignore_type, double self_damage, double unable_to_attack_chance, string affect_stat, double affect_stat_mulitplier, double removal_chance, int min_duration, int max_duration)
        {
            this.name = name;
            this.adj = adj;
            persistence = persistance;
            this.ignore_type = ignore_type;
            this.self_damage = self_damage;
            this.unable_to_attack_chance = unable_to_attack_chance;
            this.affect_stat = affect_stat;
            this.affect_stat_mulitplier = affect_stat_mulitplier;
            this.removal_chance = removal_chance;
            this.max_duration = max_duration;
            this.min_duration = min_duration;

        }

        public static Status GetStatus(string name)
        {
            return all_status_effects[name];
        }

        //roll for applying status
        public bool RollToApplyStatus(Moves move)
        { 
            int num = Utility.rnd.Next(1, 100);
            double chance = move.status_chance * 100;
            if (num <= chance) return true;
            return false;
        }

        //Takes the attacking move and the pokemon and checks if attack can apply status
        public static Status Apply_Attack_Status_Effect(Moves attacking_move, Unit Attacking, Unit Defending)
        {
            
            //if move has no status, return
            if (attacking_move.status.name == "null") return GetStatus("null");

            Unit Target = Defending;
            //if status targets self, apply to self
            if (attacking_move.status_target == "self") Target = Attacking;

            //Debug.Log("Attack has status");
            //if status is a perm status and there is already a perm status
            if (attacking_move.status.persistence && Target.pokemon.HasPersistenceStatus()) return Status.GetStatus("null");
            

            //Debug.Log("attacking_move.status.name " + attacking_move.status.name);
            //Debug.Log("Defending.pokemon.HasStatus(attacking_move.status.name )" + Defending.pokemon.HasStatus(attacking_move.status.name));

            //if same status applied, don't apply
            if (Target.pokemon.HasStatus(attacking_move.status.name)) return GetStatus("null");
            //Debug.Log("Attack status not already on pokemon");

            //if the status ignore type is either of the Defending pokemons types, return immune
            if (attacking_move.status.ignore_type == Target.pokemon.type1.name) return GetStatus("immune");
            if(attacking_move.status.ignore_type == Target.pokemon.type2.name) return GetStatus("immune");
            //Debug.Log("Pokemon Type not ignore type");

            //if defender is immune to attack type, dont apply
            if (Utility.EffectivenessMultiplier(attacking_move, Target.pokemon) == 0) return GetStatus("null");
            //Debug.Log("Pokemon not immune to attack");

            //if roll to apply status fails dont apply status
            if (!attacking_move.status.RollToApplyStatus(attacking_move)) return GetStatus("null");
            //Debug.Log("Status Effect Succeeded");

            System.Random rnd = new();

            attacking_move.status.remaining_turns = rnd.Next(attacking_move.status.min_duration, attacking_move.status.max_duration + 1);
            //Debug.Log("Status Remaining Turns: " + attacking_move.status.remaining_turns);
            Target.pokemon.statuses.Add(attacking_move.status);
            //StatReduce(Defending, attacking_move.status);
            
            return attacking_move.status;
        }
    }
}