using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class Utility : MonoBehaviour
    {
        /// <summary>
        /// Determines if a hit is a critical or not.
        /// </summary>
        /// <param name="unit">The unit that we are checking for. We need this to access the .pokemon, then the .critical_stage to determine how likely a crit is.</param>
        /// <returns>Returns true if the hit becomes a crit, false otherwise.</returns>
        public static bool CriticalHit(Unit unit)
        {
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            if (unit.pokemon.critical_stage == 0)
            {
                if (num <= 6) return true;
            }
            if (unit.pokemon.critical_stage == 1)
            {
                if (num <= 13) return true;
            }
            if (unit.pokemon.critical_stage == 2)
            {
                if (num <= 25) return true;
            }
            if (unit.pokemon.critical_stage == 3)
            {
                if (num <= 33) return true;
            }
            if (unit.pokemon.critical_stage == 4)
            {
                if (num <= 50) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the category of move.
        /// </summary>
        /// <param name="move">The move we want the category of.</param>
        /// <returns>The string of what category the move is.</returns>
        public static string GetCategoryOfMove(Moves move)
        {
            return move.category;
        }

        /// <summary>
        /// Calculates how much damage is done based on the types of the attacker and defender
        /// </summary>
        /// <param name="attacker">The attacker unit.</param>
        /// <param name="defender">The defender unit.</param>
        /// <param name="attack">The move we want to use.</param>
        /// <param name="crit">A bool that lets us know if the attack is a crit or not.</param>
        /// <returns>This returns the type1 advantage of the defender multiplied by the type2 advantage of the defender.</returns>
        public static double DoDamage(Unit attacker, Unit defender, Moves attack, bool crit)
        {
            double type1 = 1;
            double type2 = 1;

            switch (attack.move_type.type)
            {
                case "Normal":
                    type1 = defender.pokemon.type1.defend_normal;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_normal;
                    break;
                case "Fire":
                    type1 = defender.pokemon.type1.defend_fire;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_fire;
                    break;
                case "Water":
                    type1 = defender.pokemon.type1.defend_water;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_water;
                    break;
                case "Electric":
                    type1 = defender.pokemon.type1.defend_electric;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_electric;
                    break;
                case "Grass":
                    type1 = defender.pokemon.type1.defend_grass;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_grass;
                    break;
                case "Ice":
                    type1 = defender.pokemon.type1.defend_ice;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ice;
                    break;
                case "Fighting":
                    type1 = defender.pokemon.type1.defend_fighting;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_fighting;
                    break;
                case "Poison":
                    type1 = defender.pokemon.type1.defend_poison;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_poison;
                    break;
                case "Ground":
                    type1 = defender.pokemon.type1.defend_ground;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ground;
                    break;
                case "Flying":
                    type1 = defender.pokemon.type1.defend_flying;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_flying;
                    break;
                case "Psychic":
                    type1 = defender.pokemon.type1.defend_psychic;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_psychic;
                    break;
                case "Bug":
                    type1 = defender.pokemon.type1.defend_bug;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_bug;
                    break;
                case "Rock":
                    type1 = defender.pokemon.type1.defend_rock;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_rock;
                    break;
                case "Ghost":
                    type1 = defender.pokemon.type1.defend_ghost;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ghost;
                    break;
                case "Dragon":
                    type1 = defender.pokemon.type1.defend_dragon;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_dragon;
                    break;
                case "Dark":
                    break;
                case "Steel":
                    break;
                case "Fairy":
                    break;
                default:
                    break;
            }
            Debug.Log(attack.base_power);
            if (attack.base_power > 0)
            {
                if (Utility.GetCategoryOfMove(attack).CompareTo("Physical") == 0)
                {
                    attacker.SetDamage(defender.pokemon.current_defense, attacker.pokemon.current_attack, attack.base_power, attack, crit, type1, type2);
                }
                else
                {
                    attacker.SetDamage(defender.pokemon.current_sp_defense, attacker.pokemon.current_sp_attack, attack.base_power, attack, crit, type1, type2);
                }
            }
            else
            {
                attacker.SetDamage(1, 0, 0, attack, crit, 1, 1);
            }
            return type1 * type2;
        }


        /// <summary>
        /// Determines if the enemy Pokemon has any remaining moves or if it has to use struggle.
        /// </summary>
        /// <returns>True if they have to struggle, false otherwise.</returns>
        public static bool EnemyStruggle(Unit unit)
        {
            int i;
            bool struggle = false;

            for (i = 0; i < unit.pokemon.currentMoves.Count(s => s != null); i++)
            {
                if (unit.pokemon.currentMoves[i].current_pp != 0)
                {
                    struggle = false;
                    break;
                }
                struggle = true;
            }
            return struggle;
        }


    }
}

