using System.Linq;
using UnityEngine;

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
            double dmg_multiplier = 1;

            //calculates the damage multiplier for attacking move on both defender types
            dmg_multiplier = Type.attacking_type_dict[attack.type.name][defender.pokemon.type1.name] * Type.attacking_type_dict[attack.type.name][defender.pokemon.type2.name];

            
            
            //Debug.Log(attack.base_power);
            if (attack.base_power > 0)
            {
                if (attack.category == "Physical")
                {
                    attacker.SetDamage(defender.pokemon.current_defense, attacker.pokemon.current_attack, attack.base_power, attack, crit, dmg_multiplier);
                }
                else
                {
                    attacker.SetDamage(defender.pokemon.current_sp_defense, attacker.pokemon.current_sp_attack, attack.base_power, attack, crit, dmg_multiplier);
                }
            }
            else
            {
                attacker.SetDamage(1, 0, 0, attack, crit, 1);
            }
            return dmg_multiplier;


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

        public static bool IsGround(Unit unit)
        {
            bool x = false;
            if (unit.pokemon.type1.name.Equals("Ground")) return true;
            try
            {
                if (unit.pokemon.type2.name.Equals("Ground")) return true;
            }
            catch
            {

            }
            return x;
        }


        public static bool IsImmune(Moves attack, Unit Defender)
        {
            bool safeTypeTwo = (Defender.pokemon.type2 != null) ? (attack.status.ignore_type == Defender.pokemon.type2.name) : false;
            bool immunity = (attack.status.ignore_type == Defender.pokemon.type1.name) || (safeTypeTwo);
            //Debug.Log("immunity: " + immunity);
            //Debug.Log("typeTwo: " + Defender.pokemon.type2 + " ignore type: " + attack.status.ignore_type); 

            return immunity;
        }
    }
}

