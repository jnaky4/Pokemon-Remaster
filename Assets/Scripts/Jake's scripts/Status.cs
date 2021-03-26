using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Burn:
 *  self_damage 1/8 .125 of max hp
 *  current_damage_physical * 1/2      .5
 *  ignore_type fire
 *  persistence true
 *  applies after both players attacked that turn
 *
 * no early pokemon have freeze abilities
 * Freeze:
 * unable_to_attack: 100% chance
 * fire moves thaw freeze
 * removal_chance: 20% chance beginning of turn
 * ignore_type ice
 * persistence true
 *
 *
 * Paralysis:
 * current speed * 1/2
 * unable_to_attack: 25% chance
 * ignore_type electric
 * persistence true
 *
 * Poison:
 * self_damage 1/8 .125 of max hp
 * ignore_type poison
 * persistence true
 * Overworld affect: Damage
 *
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

        static Dictionary<string, Status> all_status_effects = new Dictionary<string, Status>()
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
            //                          name            persist ig_type     s_dmg%  u_atk%  afct_st     afct_st_mul rmv_ch  min_dur max_dur
            {"Burn",  new Status(       "Burn",         true,   "Fire",     .125,   0,      "Attack",   .5,         0,      -1,     -1)},
            {"Freeze",  new Status(     "Freeze",       true,   "Ice",      0,      1.0,    "null",     1,          .2,     -1,     -1)},
            {"Sleep",  new Status(      "Sleep",        true,   "null",     0,      1.0,    "null",     1,          0,      3,      1)},
            {"Paralysis", new Status(   "Paralysis",    true,   "Electric", 0,      .25,    "Speed",    .5,         0,      -1,     -1)},
            {"Poison", new Status(      "Poison",       true,   "Poison",   .125,   0,      "null",     1,          0,      -1,     -1)},
            {"Flinch", new Status(      "Flinch",       false,  "null",     0,      1.0,    "null",     1,          0,      1,      1)},
            {"Confusion", new Status(   "Confusion",    false,  "null",     .175,   .33,    "null",     1,          0,      5,      2)},
            //for csv, does nothing
            {"null", new Status(        "null",         false,  "null",     0,      0,      "null",     1,          0,      -1,     -1) }

        };

        public string name;
        //if the move persist outside battle/switched out
        public bool persistance;
        public string ignore_type;
        public double self_damage;
        public double unable_to_attack_chance;
        public string affect_stat;
        public double affect_stat_mulitplier;
        public double removal_chance;
        public int max_duration;
        public int min_duration;

        public Status(string name, bool persistance, string ignore_type, double self_damage, double unable_to_attack_chance, string affect_stat, double affect_stat_mulitplier, double removal_chance, int max_duration, int min_duration)
        {
            this.name = name;
            this.persistance = persistance;
            this.ignore_type = ignore_type;
            this.self_damage = self_damage;
            this.unable_to_attack_chance = unable_to_attack_chance;
            this.affect_stat = affect_stat;
            this.affect_stat_mulitplier = affect_stat_mulitplier;
            this.removal_chance = removal_chance;
            this.max_duration = max_duration;
            this.min_duration = min_duration;
        }
        public static Status get_status(string name)
        {
            return all_status_effects[name];
        }

        public bool SeeIfBurn(Moves move)
        {
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            double chance = move.status_chance * 100;
            if (num <= chance) return true;
            return false;
        }

        public void BurnSelf(Unit unit)
        {
            unit.TakeDamage(unit.damage * (.125));
        }

    }
}