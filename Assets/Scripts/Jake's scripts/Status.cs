using System;
using System.Collections.Generic;

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
            {"Confusion", new Status(   "Confusion", "confused",    false,     "null",     .175,   .33,    "null",     1,          0,      5,      2)},
            //for csv, does nothing
            {"null", new Status(        "null",       "null",       false,  "null",     0,      0,      "null",     1,          0,      -1,     -1) }
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

        public bool SeeIfStatus(Moves move)
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

        public static void ReduceSleep(Unit unit)
        {
            unit.pokemon.sleep--;
            if (unit.pokemon.sleep == 0)
            {
                unit.pokemon.statuses.Remove(get_status("Sleep"));
                //BattleSystem.dialogueText.text = unit.pokemon.name + " woke up!";
            }
        }

        public static bool SeeIfParalyzed(Pokemon poke)
        {
            int check = 0;
            foreach (Status s in poke.statuses)
            {
                if (s.name.Equals("Paralysis")) check = 1;
            }
            if (check == 0) return false;
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            if (num <= 25) return true;
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

        public static void SeeIfStatusEffect(Moves move, Unit unit)
        {
            switch (move.status.name)
            {
                case "Burn":
                    if (move.status.SeeIfStatus(move))
                    {
                        if (SeeIfPersistanceIsAlreadyHere(unit.pokemon)) break;
                        try
                        {
                            if (unit.pokemon.type1.Equals("Fire")) break;
                            if (unit.pokemon.type2.Equals("Fire")) break;
                        }
                        catch (NullReferenceException ex)
                        {
                            //throw;
                        }
                        finally
                        {
                            unit.pokemon.statuses.Add(get_status("Burn"));
                        }
                    }
                    break;

                case "Paralysis":
                    if (move.status.SeeIfStatus(move))
                    {
                        if (SeeIfPersistanceIsAlreadyHere(unit.pokemon)) break;
                        try
                        {
                            if (unit.pokemon.type1.Equals("Electric")) break;
                            if (unit.pokemon.type2.Equals("Electric")) break;
                        }
                        catch (NullReferenceException ex)
                        {
                            //throw;
                        }
                        finally
                        {
                            unit.pokemon.statuses.Add(get_status("Paralysis"));
                            ParalyzeSpeedReduce(unit);
                        }
                    }
                    break;

                case "Poison":
                    if (move.status.SeeIfStatus(move))
                    {
                        if (SeeIfPersistanceIsAlreadyHere(unit.pokemon)) break;
                        try
                        {
                            if (unit.pokemon.type1.Equals("Poison")) break;
                            if (unit.pokemon.type2.Equals("Poison")) break;
                        }
                        catch (NullReferenceException ex)
                        {
                            //throw;
                        }
                        finally
                        {
                            unit.pokemon.statuses.Add(get_status("Poison"));
                        }
                    }
                    break;

                case "Sleep":
                    if (move.status.SeeIfStatus(move))
                    {
                        if (SeeIfPersistanceIsAlreadyHere(unit.pokemon)) break;
                        unit.pokemon.statuses.Add(get_status("Sleep"));
                        unit.pokemon.sleep = GameController._rnd.Next(1, 4);
                    }
                    break;

                case "Freeze":
                    if (move.status.SeeIfStatus(move))
                    {
                        if (SeeIfPersistanceIsAlreadyHere(unit.pokemon)) break;
                        try
                        {
                            if (unit.pokemon.type1.Equals("Ice")) break;
                            if (unit.pokemon.type2.Equals("Ice")) break;
                        }
                        catch (NullReferenceException ex)
                        {
                            //throw;
                        }
                        finally
                        {
                            unit.pokemon.statuses.Add(get_status("Freeze"));
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}