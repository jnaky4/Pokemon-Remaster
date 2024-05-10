using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Pokemon
{

    public class Attack{
        public Attack(string Name)
        {
            this.name = name;
        }
        public string name;
        // public Moves move;
        public int damage;

        // public Pokemon Attacker;

        // public Pokemon Defender;
        public bool critalHit;

        public int TurnsLeft;

        ///Using Gen 3 Damage Calculations
        /// ( ((2 x level) / 5) + 2 x Power + A/D ) / 50 
        /// x Burn x Screen x Targets x Weather x FF + 2 ) 
        /// x Stockpile x Critical x DoubleDmg x Charge x HH x STAB x Type1 x Type2 x Random
        /// Ref: https://bulbapedia.bulbagarden.net/wiki/Damage
        public int calculateDamage(){
            
            return 0;
        }
        // public static bool CriticalHit(){
        //     // Next Range is from:
        //     //      (min to max-1) 
        //     // ie   1 to (101 - 1) 
        //     // or   1 to 100

        //     /*
        //     * using Gen 2-5 Crit Pobability based on stages
        //     * Gen II-V	            Gen VI	        Gen VII onwards:
        //         +0	1/16 (6.25%)	1/16 (6.25%)	1/24 (~4.167%)
        //         +1	1/8 (12.5%)	    1/8 (12.5%)	    1/8 (12.5%)
        //         +2	1/4 (25%)	    1/2 (50%)	    1/2 (50%)
        //         +3	1/3 (~33.3%)	Always (100%)	Always (100%)
        //         ++4 1/2 (50%)       Always (100%)	Always (100%)
        //     */
        //     int num = Utility.rnd.Next(1, 101);
        //     // switch(){
        //     if (unit.pokemon.critical_stage == 0)
        //     {
        //         if (num <= 6) return true;
        //     }
        //     if (unit.pokemon.critical_stage == 1)
        //     {
        //         if (num <= 13) return true;
        //     }
        //     if (unit.pokemon.critical_stage == 2)
        //     {
        //         if (num <= 25) return true;
        //     }
        //     if (unit.pokemon.critical_stage == 3)
        //     {
        //         if (num <= 33) return true;
        //     }
        //     if (unit.pokemon.critical_stage == 4)
        //     {
        //         if (num <= 50) return true;
        //     }
        //     return false;
            
        // }
    }

}
