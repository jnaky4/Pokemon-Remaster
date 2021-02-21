using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Burn:
 *  self_damage 1/8 .125 of max hp
 *  current_damage_physical * 1/2      .5
 *  ignore_type fire
 *  persistance true
 *  applies after both players attacked that turn
 *  
 * no early pokemon have freeze abilities
 * Freeze: 
 * unable_to_attack: 100% chance
 * fire moves thaw freeze
 * removal_chance: 20% chance beginning of turn
 * ignore_type ice
 * persistance true
 * 
 * 
 * Paralysis:
 * current speed * 1/2
 * unable_to_attack: 25% chance
 * ignore_type electric
 * persistance true
 * 
 * Poison:
 * self_damage 1/8 .125 of max hp
 * ignore_type poison
 * persistance true
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

            //name, persistance, ignoretype, self_dmg%, unable attack%, affect_stat, affect_stat_multiplier, removal_chance, min_duration, max_duration
            {"Burn",  new Status("Burn", true, "Fire", .25, 0, "Physical", .25, 0, -1, -1)},
            {"Freeze",  new Status("Freeze", true, "Ice", 0, 1.0, "null", 0, .2, -1, -1)},
            {"Sleep",  new Status("Sleep", true, "null", 0, 1.0, "null", 0, 0, 3, 1)},
            {"Paralysis", new Status("Paralysis", true, "Electric", 0, .25, "Speed", .5, 0, -1, -1) },
            {"Poison", new Status("Poison", true, "Poison", .125, 0, "null", 0, 0, -1, -1) },
            {"Confusion", new Status("Confusion", false, "null", .175, .33, "null", 0, 0, 5, 2) },
            {"Flinch", new Status("Flinch", false, "null", 0, 1.0, "null", 0, 0, 1, 1) },
            //for csv, does nothing
            {"null", new Status("null", false, "null", 0, 0, "null", 0, 0, -1, -1) }

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




    }
}