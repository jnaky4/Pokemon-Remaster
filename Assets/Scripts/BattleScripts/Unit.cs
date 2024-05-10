using System;
using UnityEngine;
using System.Collections.Generic;

namespace Pokemon
{
    public class Unit : MonoBehaviour
    {
        #region Variables

        public Pokemon pokemon;

        public double damage; //Gets updated each turn depending on all of the battle factors.

        #endregion Variables

        /// <summary>
        /// Subtracts one from PP whenever we do that specific move. Do not call on struggle, as struggle has infinite pp. Also it would break.
        /// </summary>
        /// <param name="numMove">The index of the move we want to decrease pp.</param>
        public void DoPP(int numMove)
        {
            pokemon.currentMoves[numMove].current_pp--;
        } 

        /// <summary>
        /// Applies damage to this current pokemon, based on the function above (which was applied to the opponent pokemon).
        /// </summary>
        /// <param name="dmg">The opponent's damage.</param>
        /// <returns>True if we died, false otherwise</returns>
        public bool TakeDamage(double dmg)
        {
            pokemon.current_hp -= (int)dmg;
            if (pokemon.current_hp > pokemon.max_hp) pokemon.current_hp = pokemon.max_hp;
            if (pokemon.current_hp < 0) pokemon.current_hp = 0;

            if (pokemon.current_hp <= 0) return true;
            else return false;
        }

        /// <summary>
        /// Sets the stages.
        /// Stages are the multipliers to the various stats.
        /// Example: Tail whip lowers defense, but what does that mean? Your defense stage gets lowered, which lowers your current defense.
        /// Stages are so everything can be consistant and looks alright. Idk, but they are important.
        /// </summary>
        /// <param name="attack">The attack that raises/lowers stages.</param>
        /// <param name="Target">The enemy whose stage might get set.</param>
        public static void SetStages(Moves attack, Unit Defender)
        {
            //if roll > chance for stat to change return ie (roll)34 > 33(.33 * 100) return
            if (Utility.rnd.Next(1, 101)  >  attack.chance_stat_change * 100.0) return;
            Dictionary<string, (Func<int> getter, Action<int> setter)> statProperties = new()
            {
                { "Attack", (() => Defender.pokemon.stage_attack, value => Defender.pokemon.stage_attack = value) },
                { "Defense", (() => Defender.pokemon.stage_defense, value => Defender.pokemon.stage_defense = value) },
                { "Speed", (() => Defender.pokemon.stage_speed, value => Defender.pokemon.stage_speed = value) },
                { "Special Attack", (() => Defender.pokemon.stage_sp_attack, value => Defender.pokemon.stage_sp_attack = value) },
                { "Special Defense", (() => Defender.pokemon.stage_sp_defense, value => Defender.pokemon.stage_sp_defense = value) },
                { "Critical", (() => Defender.pokemon.stage_critical, value => Defender.pokemon.stage_critical = value) },
                { "Evasion", (() => Defender.pokemon.stage_speed, value => Defender.pokemon.stage_speed = value) },
                { "Accuracy", (() => Defender.pokemon.stage_accuracy, value => Defender.pokemon.stage_accuracy = value) }
            };
        
            // Apply stat changes
            foreach (string statChange in attack.current_stat_change.Split(','))
            {
                if (statProperties.ContainsKey(statChange))
                {
                    // Get the current value and calculate new value
                    int currentValue = statProperties[statChange].getter();
                    int newValue = currentValue + attack.stat_change_amount;
                    
                    newValue = Math.Max(-6, Math.Min(6, newValue)); // Clamping between -6 and 6

                    // Update the corresponding property using the dictionary
                    statProperties[statChange].setter(newValue);
                }
            }
        }
        //TODO Finish out AI decisions 
        public int DecideMove(Moves playerAttack, Unit Player)
        {
            //<move_name, score> 
            Dictionary<string, int> available_moves = new();


            //Debug.Log(whoGoesFirst);

            //Data on players Pokemon
            double playerEmul = Utility.EffectivenessMultiplier(playerAttack, pokemon);
            double playerStab = Utility.STAB(playerAttack, Player.pokemon);

            /*
            * using Gen 2-5 Crit Pobability based on stages
            * Stage Gen II-V	    Gen VI	        Gen VII onwards:
            * +0	1/16 (6.25%)	1/16 (6.25%)	1/24 (~4.167%)
            * +1	1/8 (12.5%)	    1/8 (12.5%)	    1/8 (12.5%)
            * +2	1/4 (25%)	    1/2 (50%)	    1/2 (50%)
            * +3	1/3 (~33.3%)	Always (100%)	Always (100%)
            * ++4   1/2 (50%)       Always (100%)	Always (100%)
            */

            int playerMinDamage = Utility.CalculateDamage(Player.pokemon, pokemon, playerAttack);
            int playerMaxNoCrit = Utility.CalculateDamage(Player.pokemon, pokemon, playerAttack);
            int playerDamage = Utility.CalculateDamage(Player.pokemon, pokemon, playerAttack);
            int playerMinCrit = Utility.CalculateDamage(Player.pokemon, pokemon, playerAttack);
            int playerMaxDamage = Utility.CalculateDamage(Player.pokemon, pokemon, playerAttack);

            double playerCritChance = Utility.CritChance(Player.pokemon) * 100;

            bool playerAttackLethal = Utility.IsLethal(playerDamage, pokemon);

            BattleState whoGoesFirst = Utility.WhoGoesFirst(Moves.get_move("Tackle"), playerAttack, pokemon, Player.pokemon);
            int AIturnsUntilFaint = Utility.TurnsUntilFaint(playerDamage, pokemon, whoGoesFirst);

            /* Weight Evaluation of AI attack.
             * 10: Guaranteed AI win, AI Attacking First
             * 9: Guaranteed AI win, AI Attacking Second
             * 8: AI will 2KO, AI Attacking First
             * 7: 2KO, AI Attacking Second
             * 6: 
             * 5:
             * 4:
             * 3:
             * 2:
             * 1: Guaranteed faint, AI Attacking First
             * 0: Guaranteed faint, AI Attacking Second
             * -1: Non Damaging, AI Attacking First
             * -2: Non Damaging, AI attacking Second
             */

            /* TODO:
             * Determine if wild or trainer
             * Determine non damaging moves impact
             * 
             * 
             * 
             * 
             * 
             */


/*            Debug.Log("AI HP: " + pokemon.current_hp);
            Debug.Log("Player Min Damage: " + playerMinDamage);
            Debug.Log("Plauyer Max No Crit: " + playerMaxNoCrit);
            Debug.Log("Player Max Damage: " + playerMaxDamage);
            //Debug.Log("Crit Chance: " + playerCritChance + "%");
            //Debug.Log("Player Damage: " + playerDamage);
            Debug.Log("AI will faint in " + AIturnsUntilFaint + " turns");*/


            //get sum of moves
            //int num_moves = 
            //Debug.Log("Num Moves: " + num_moves);

            int decided_move_index = -1;
            int decided_move_damage;
            int playerTurnsUntilFaint = -1;
            Moves highest_damaging_move;
            Moves least_damaging_lethal_move;
            bool killsEnemy = false;

            //Debug.Log("Enemy has " + enemyhealth + " hp.");


            //does estimated minmum damage calculation for each move the AI pokemon has
            available_moves = Utility.CalculateEachMoveDamage(pokemon, this, Player);
            //gets the highest damaging move
            highest_damaging_move = Utility.DecideHighestDamagingAttack(available_moves);
            //gets the least damaging move that is lethal
            least_damaging_lethal_move = Utility.LeastDamagingMoveThatKills(available_moves, Player.pokemon);


            if(highest_damaging_move != null)
            {
                //who will go first
                whoGoesFirst = Utility.WhoGoesFirst(highest_damaging_move, playerAttack, pokemon, Player.pokemon);
                decided_move_damage = available_moves[highest_damaging_move.name];
                playerTurnsUntilFaint = Utility.TurnsUntilFaint(decided_move_damage, Player.pokemon, whoGoesFirst);
                decided_move_index = pokemon.getMoveIndex(highest_damaging_move.name);
                killsEnemy = Utility.IsLethal(decided_move_damage, Player.pokemon);
            }

            if(least_damaging_lethal_move != null)
            {
                decided_move_index = pokemon.getMoveIndex(least_damaging_lethal_move.name);
            }

            if(playerTurnsUntilFaint > 3)
            {
                Moves status_move = Utility.DecideBestStatusAttack(pokemon, Player.pokemon);
                if (status_move != null)
                {
                    Debug.Log("Best Status Moves: " + status_move.name);
                    if (!Player.pokemon.HasStatus(status_move.status.name)) decided_move_index = pokemon.getMoveIndex(status_move.name);
                }

            }

/*            if (whoGoesFirst == BattleState.PLAYERTURN && playerAttackLethal)
            {
                Debug.Log("AI will die before it can attack");
                //TODO Check for swap and priority
                return 0;
            }*/
            return decided_move_index;
        }

    }
}