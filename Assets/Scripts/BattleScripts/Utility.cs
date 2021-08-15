using System.Linq;
using UnityEngine;
using System;

namespace Pokemon
{
    public class Utility : MonoBehaviour
    {
        public static int randomSeed = 1000;
        public static System.Random rnd = new System.Random(randomSeed);
        //public static System.Random rnd = new System.Random();


        /// <summary>
        /// Determines if a hit is a critical or not.
        /// </summary>
        /// <param name="unit">The unit that we are checking for. We need this to access the .pokemon, then the .critical_stage to determine how likely a crit is.</param>
        /// <returns>Returns true if the hit becomes a crit, false otherwise.</returns>
        public static bool CriticalHit(Unit unit)
        {
            


            int num = Utility.rnd.Next(1, 100);
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
        /// Calculates how much damage is done based on the types of the attacker and defender
        /// </summary>
        /// <param name="attacker">The attacker unit.</param>
        /// <param name="defender">The defender unit.</param>
        /// <param name="attack">The move we want to use.</param>
        /// <param name="crit">A bool that lets us know if the attack is a crit or not.</param>
        /// <returns>This returns the type1 advantage of the defender multiplied by the type2 advantage of the defender.</returns>
        public static int CalculateDamage(Unit attacker, Unit defender, Moves attack, bool crit, double dmg_multiplier, double stab)
        {
            double damage = 0;
            //calculates the damage multiplier for attacking move on both defender types
            //double dmg_multiplier = EffectivenessMultiplier(attack, defender.pokemon);
            double critical = (crit) ? 1.5 : 1; //If it is a crit, multiply by 1.5
            


            //System.Random rnd = new System.Random();

            //85 to 99
            double num = Utility.rnd.Next(85, 100);
            double random = num / 100; //Random number for the random element.
            double burn = 1;


            //doesnt get used
/*            if (attacker.pokemon.statuses.Contains(Status.get_status("Burn")))
            {
                burn = Status.get_status("Burn").affect_stat_mulitplier;
            }*/

            if (attack.base_power > 0)
            {
                if(attack.category == "Physical")
                {
                    damage = (((((2 * attacker.pokemon.level) / 5) + 2) * attack.base_power * (attacker.pokemon.current_attack / defender.pokemon.current_defense)) / 50) + 2; //Basic attacking
                }
                else
                {
                    damage = (((((2 * attacker.pokemon.level) / 5) + 2) * attack.base_power * (attacker.pokemon.current_sp_attack / defender.pokemon.current_sp_defense)) / 50) + 2;
                }
                
                damage *= (critical * stab * random * dmg_multiplier); //Extra multipliers.
                //immune moves will never deal damage
                if (dmg_multiplier == 0) damage = 0;
                //damaging moves always do 1 damage
                else if (damage == 0) damage = 1;
            }
            else
            {
                damage = 0;
            }
            if (damage < 0) damage = 0;

            attacker.damage = damage;

            return (int)damage;
        }


        public static double EffectivenessMultiplier(Moves attack, Pokemon defender)
        {
            return Type.attacking_type_dict[attack.type.name][defender.type1.name] * Type.attacking_type_dict[attack.type.name][defender.type2.name];
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

        public static double STAB(Moves attack, Pokemon attacker)
        {
            return attack.type.name == attacker.type1.name || attack.type.name == attacker.type2.name ? 1.5 : 1;
        }


        public static BattleState WhoGoesFirst(Moves playerMove, Moves enemyMove, Pokemon playerPokemon, Pokemon enemyPokemon)
        {

            //priorities arent the same
            //1,0 or 0,1
            if (playerMove.priority != enemyMove.priority)
            {
                return playerMove.priority > enemyMove.priority ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;

            }
            //priorities are the same, do additional checks
            //1,1, or 0,0
            if(playerPokemon.current_speed != enemyPokemon.current_speed)
            {
                return playerPokemon.current_speed > enemyPokemon.current_speed ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
            }
            //same priority, same speed
            else
            {

                return Utility.rnd.Next(2) < 1 ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
            }

        }

        public static bool isLethal(int damage, Pokemon target) => damage >= target.current_hp;

        public static int turnsUntilFaint(int attackerDamage, Pokemon defending, BattleState whoGoesFirst)
        {
            int turnsUntilFaint = -1;
            if (attackerDamage > 0)
            {
                turnsUntilFaint = (int)Math.Ceiling((double)defending.current_hp / (double)attackerDamage);

                turnsUntilFaint += whoGoesFirst == BattleState.PLAYERTURN ? 0 : 1;
            }
            return turnsUntilFaint;
        }

    }
}

