using System;
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
        private static Dictionary<string, Status> all_status_effects = new Dictionary<string, Status>()
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
            //                          name         adj            persist ig_type     s_dmg%  u_atk%  afct_st     afct_st_mul rmv_ch  max_dur min_dur
            {"Burn",  new Status(       "Burn",      "burned",      true,   "Fire",     .125,   0,      "Attack",   .5,         0,      -1,     -1)},
            {"Freeze",  new Status(     "Freeze",    "frozen",      true,   "Ice",      0,      1.0,    "null",     1,          .2,     -1,     -1)},
            {"Sleep",  new Status(      "Sleep",     "asleep",      true,   "null",     0,      1.0,    "null",     1,          0,      3,      1)},
            {"Paralysis", new Status(   "Paralysis", "paralyzed",   true,   "Electric", 0,      .25,    "Speed",    .5,         0,      -1,     -1)},
            {"Poison", new Status(      "Poison",    "poisoned",    true,   "Poison",   .125,   0,      "null",     1,          0,      -1,     -1)},
            {"Flinch", new Status(      "Flinch",    "flinched",    false,  "null",     0,      1.0,    "null",     1,          0,      1,      1)},
            {"Confusion", new Status(   "Confusion", "confused",    false,  "null",     .175,   .33,    "null",     1,          0,      5,      2)},
            {"Leech Seed", new Status(  "Leech Seed","seeded",      false,  "null",     .0625,  0,      "null",     1,          0,      -1,     -1)},
            //for csv, does nothing
            {"null", new Status(        "null",       "null",       false,  "null",     0,      0,      "null",     1,          0,      -1,     -1)}
        };

        public string name;
        public string adj;

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
        public int remaining_turns = 0;
        


        public Status(string name, string adj, bool persistance, string ignore_type, double self_damage, double unable_to_attack_chance, string affect_stat, double affect_stat_mulitplier, double removal_chance, int max_duration, int min_duration)
        {
            this.name = name;
            this.adj = adj;
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

        //roll for applying status
        public bool RollToApplyStatus(Moves move)
        {
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            double chance = move.status_chance * 100;
            if (num <= chance) return true;
            return false;
        }

        public static void ParalyzeSpeedReduce(Unit unit)
        {
            unit.pokemon.current_speed = (int)(unit.pokemon.current_speed * .5);
        }

        /*        public static void ReduceSleep(Unit unit)
                {
                    unit.pokemon.sleep--;
                    if (unit.pokemon.sleep == 0)
                    {
                        unit.pokemon.statuses.Remove(get_status("Sleep"));
                        //BattleSystem.dialogueText.text = unit.pokemon.name + " woke up!";
                    }
                }*/

        public static bool roll_for_Paralysis(Pokemon poke)
        {
            bool paralyzed = false;
            //check statuses in pokemon for paralysis
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Paralysis")) paralyzed = true;
            }

            //if paralyzed, roll for paralysis
            if (paralyzed)
            {
                System.Random rnd = new System.Random();
                //returns a number >= 0.0 AND < 1.0 : [0.0 - .99999]
                double num = rnd.NextDouble();
                if (num <= all_status_effects["Paralysis"].unable_to_attack_chance) return true;
            }
            return false;
        }

        public static bool SeeIfPoisoned(Pokemon poke)
        {
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Poison")) return true;
            }
            return false;
        }

        public static bool SeeIfBurned(Pokemon poke)
        {
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Burn")) return true;
            }
            return false;
        }

        public static bool SeeIfSleep(Pokemon poke)
        {
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Sleep")) return true;
            }
            return false;
        }

        public static bool SeeIfFreeze(Pokemon poke)
        {
            int check = 0;
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Freeze")) check = 1;
            }
            if (check == 0) return false;
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            if (num > 20) return true;
            else
            {
                poke.statuses.Remove("Freeze");
                //BattleSystem.dialogueText.text = poke.name + " unfroze!";
                return false;
            }
        }

        public static bool SeeIfPersistanceIsAlreadyHere(Pokemon poke)
        {
            foreach (Status s in poke.statuses)
            {
                if (s.persistance)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SeeIfLeech(Pokemon poke)
        {
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Leech Seed")) return true;
            }
            return false;
        }

        //Takes the attacking move and the pokemon and checks if attack can apply status
        public static void Apply_Attack_Status_Effect(Moves attacking_move, Unit enemy_pokemon)
        {
            
            foreach(Status status in enemy_pokemon.pokemon.statuses)
            {
                //if same status applied, don't apply
                if (attacking_move.status.name == status.name) return;
                //if status is a perm status and there is already a perm status
                if (attacking_move.status.persistance && status.persistance) return; 
            }

            //if roll to apply status fails dont apply status
            if (!attacking_move.status.RollToApplyStatus(attacking_move)) return;



            //extra checks on apply status, like pokemon type
            switch (attacking_move.status.name)
            {

                case "Burn":
                    
                    try
                    {
                        if (enemy_pokemon.pokemon.type1.type.Equals("Fire")) break;
                        if (enemy_pokemon.pokemon.type2.type.Equals("Fire")) break;
                    }
                    catch (NullReferenceException ex)
                    {
                        //throw;
                    }

                    Status burn = attacking_move.status;
                    burn.remaining_turns = -1;

                    enemy_pokemon.pokemon.statuses.Add(burn);

                    break;

                case "Paralysis":
                    try
                    {
                        if (enemy_pokemon.pokemon.type1.type.Equals("Electric")) break;
                        if (enemy_pokemon.pokemon.type2.type.Equals("Electric")) break;

                        if (attacking_move.move_type.type.Equals("Electric"))
                        {
                            if (enemy_pokemon.pokemon.type1.type.Equals("Ground")) break;
                            if (enemy_pokemon.pokemon.type2.type.Equals("Ground")) break;
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        //throw;
                    }

                    Status paralysis = attacking_move.status;
                    paralysis.remaining_turns = -1;
                    enemy_pokemon.pokemon.statuses.Add(paralysis);
                    ParalyzeSpeedReduce(enemy_pokemon);
                    break;

                case "Poison":
                    try
                    {
                        if (enemy_pokemon.pokemon.type1.type == "Poison") break;
                        if (enemy_pokemon.pokemon.type2.type == "Poison") break;
                    }
                    catch (NullReferenceException ex)
                    {
                        //throw;
                        //Debug.Log("Bellsprout should not be here.3");
                    }
                    //Debug.Log("Bellsprout should not be here.4");
                    Status poison = attacking_move.status;
                    poison.remaining_turns = -1;
                    enemy_pokemon.pokemon.statuses.Add(attacking_move.status);
                    break;

                case "Sleep":

                    Status sleep = attacking_move.status;
                    System.Random rnd = new System.Random();
                    //random.next range(x to y-1) : (0,20) = [0 to 19]
                    int remaining_turns = rnd.Next(sleep.min_duration, sleep.max_duration +1);
                    sleep.remaining_turns = remaining_turns;


                    enemy_pokemon.pokemon.statuses.Add(sleep);

                    //TODO change to Status.remaining_turns
                    enemy_pokemon.pokemon.sleep = GameController._rnd.Next(1, 4);
                    
                    break;

                case "Freeze":            
                    try
                    {
                        if (enemy_pokemon.pokemon.type1.type.Equals("Ice")) break;
                        if (enemy_pokemon.pokemon.type2.type.Equals("Ice")) break;
                    }
                    catch (NullReferenceException ex)
                    {
                        //throw;
                    }
                    Status freeze = attacking_move.status;
                    freeze.remaining_turns = -1;
                    enemy_pokemon.pokemon.statuses.Add(attacking_move.status);
                    break;

                default:
                    Status status = attacking_move.status;
                    System.Random rnd2 = new System.Random();
                    //random.next range(x to y-1) : (0,20) = [0 to 19]
                    int remaining_turns2 = rnd2.Next(status.min_duration, status.max_duration + 1);
                    status.remaining_turns = remaining_turns2;
                    enemy_pokemon.pokemon.statuses.Add(status);

                    break;
            }
        }







        
        
    }
}