using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Pokemon
{
    public class Unit : MonoBehaviour
    {
        #region Variables

        public Pokemon pokemon;

        public double damage; //Gets updated each turn depending on all of the battle factors.
        private double[] multipliers = new double[] { (2f / 8f), (2f / 7f), (2f / 6f), (2f / 5f), (2f / 4f), (2f / 3f), (2f / 2f), (3f / 2f), (4f / 2f), (5f / 2f), (6f / 2f), (7f / 2f), (8f / 2f) }; //multipliers for all stages except accuracy and evasion
        private double[] accuracyMultipliers = new double[] { (3f / 9f), (3f / 8f), (3f / 7f), (3f / 6f), (3f / 5f), (3f / 4f), (3f / 3f), (4f / 3f), (5f / 3f), (6f / 3f), (7f / 3f), (8f / 3f), (9f / 3f) };
        private double[] evasionMultipliers = new double[] { (9f / 3f), (8f / 3f), (7f / 3f), (6f / 3f), (5f / 3f), (4f / 3f), (3f / 3f), (3f / 4f), (3f / 5f), (3f / 6f), (3f / 7f), (3f / 8f), (3f / 9f) };

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
        public static void SetStages(Moves attack, Unit Attacker, Unit Defender)
        {
            //Default Target
            Unit Target = Defender;

            //Pick target of stat change, more option that self, enemy, just not implemented yet  
            if (attack.status_target == "self") Target = Attacker;
            if (attack.status_target == "enemy") Target = Defender;

            //roll to see if stat change applies
            System.Random rnd = new System.Random();

            int chance_apply_stat = (int)attack.chance_stat_change * 100;
            int roll = rnd.Next(1, 101); 

            if (roll <= chance_apply_stat)
            {
                String[] multiple_stat_changes = attack.current_stat_change.Split(',');
                foreach (string statChange in multiple_stat_changes)
                {
                    //Debug.Log("StatChange: " + statChange);
                    if (attack.target == "self" || attack.target == "enemy") //If you are staging the enemy
                    {
                        switch (statChange) //Switch statement should be self-explanatory.
                        {

                            case "Attack":
                                Target.pokemon.stage_attack += attack.stat_change_amount; //changes it by the amount the attack changes. It is typically 1.
                                if (Target.pokemon.stage_attack > 6) Target.pokemon.stage_attack = 6; //If you go above or below 6, set it to 6
                                if (Target.pokemon.stage_attack < -6) Target.pokemon.stage_attack = -6;
                                Target.pokemon.current_attack = (int)(Target.pokemon.max_attack * Target.multipliers[Target.pokemon.stage_attack + 6]); //Makes your current stat based on the multiplier at the stage you are at.
                                break;

                            case "Defense":
                                Target.pokemon.stage_defense += attack.stat_change_amount;
                                if (Target.pokemon.stage_defense > 6) Target.pokemon.stage_defense = 6;
                                if (Target.pokemon.stage_defense < -6) Target.pokemon.stage_defense = -6;
                                Target.pokemon.current_defense = (int)(Target.pokemon.max_defense * Target.multipliers[Target.pokemon.stage_defense + 6]);
                                //Debug.Log("Index: " + (enemy.pokemon.defense_stage + 6) + " At Index: " + multipliers[enemy.pokemon.defense_stage + 6] + " Max: " + enemy.pokemon.max_defense +
                                //    " Current: " + enemy.pokemon.current_defense);
                                if (Target.pokemon.current_defense == (double)0) Target.pokemon.current_defense = 1;
                                break;

                            case "Speed":
                                Target.pokemon.stage_speed += attack.stat_change_amount;
                                if (Target.pokemon.stage_speed > 6) Target.pokemon.stage_speed = 6;
                                if (Target.pokemon.stage_speed < -6) Target.pokemon.stage_speed = -6;
                                Target.pokemon.current_speed = (int)((double)Target.pokemon.max_speed * Target.multipliers[Target.pokemon.stage_speed + 6]);
                                break;

                            case "Special Attack":
                                //Debug.Log("Stat Change Amount : " + attack.stat_change_amount);
                                //Debug.Log("Sp Attack Stage Before: " + Target.pokemon.stage_sp_attack);
                                Target.pokemon.stage_sp_attack += attack.stat_change_amount;
                                //Debug.Log("Sp Attack Stage After: " + Target.pokemon.stage_sp_attack);
                                if (Target.pokemon.stage_sp_attack > 6) Target.pokemon.stage_sp_attack = 6;
                                if (Target.pokemon.stage_sp_attack < -6) Target.pokemon.stage_sp_attack = -6;
                                //Debug.Log("Current Sp Attack Before: " + Target.pokemon.current_sp_attack);
                                Target.pokemon.current_sp_attack = (int)(Target.pokemon.max_sp_attack * Target.multipliers[Target.pokemon.stage_sp_attack + 6]);
                                //Debug.Log("Current Sp Attack After: " + Target.pokemon.current_sp_attack);
                                break;

                            case "Special Defense":
                                Target.pokemon.stage_sp_defense += attack.stat_change_amount;
                                if (Target.pokemon.stage_sp_defense > 6) Target.pokemon.stage_sp_defense = 6;
                                if (Target.pokemon.stage_sp_defense < -6) Target.pokemon.stage_sp_defense = -6;
                                Target.pokemon.current_sp_defense = (int)(Target.pokemon.max_sp_defense * Target.multipliers[Target.pokemon.stage_sp_defense + 6]);
                                break;

                            case "Critical": //I don't know why critical is different, but I don't want to change anything now.
                                Target.pokemon.critical_stage += attack.stat_change_amount;
                                break;

                            case "Evasion":
                                Target.pokemon.stage_evasion += attack.stat_change_amount;
                                if (Target.pokemon.stage_evasion > 6) Target.pokemon.stage_evasion = 6;
                                if (Target.pokemon.stage_evasion < -6) Target.pokemon.stage_evasion = -6;
                                Target.pokemon.current_evasion = (int)(1 * Target.evasionMultipliers[Target.pokemon.stage_evasion + 6]);
                                break;

                            case "Accuracy":
                                Target.pokemon.stage_sp_defense += attack.stat_change_amount;
                                if (Target.pokemon.stage_sp_defense > 6) Target.pokemon.stage_sp_defense = 6;
                                if (Target.pokemon.stage_sp_defense < -6) Target.pokemon.stage_sp_defense = -6;
                                Target.pokemon.current_accuracy = (int)(1 * Target.accuracyMultipliers[Target.pokemon.stage_accuracy + 6]);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        

        //TODO Finish out AI decisions 
        public int DecideMove(Moves playerAttack, Unit Player)
        {
            //<move_name, score> 
            Dictionary<string, int> available_moves = new Dictionary<string, int>();


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

            int playerMinDamage = Utility.CalculateDamage(Player, this, playerAttack, false, playerEmul, playerStab, 85);
            int playerMaxNoCrit = Utility.CalculateDamage(Player, this, playerAttack, false, playerEmul, playerStab, 100);
            int playerDamage = Utility.CalculateDamage(Player, this, playerAttack, false, playerEmul, playerStab);
            int playerMinCrit = Utility.CalculateDamage(Player, this, playerAttack, false, playerEmul, playerStab, 85);
            int playerMaxDamage = Utility.CalculateDamage(Player, this, playerAttack, true, playerEmul, playerStab, 100);

            double playerCritChance = Utility.CritChance(Player) * 100;

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
            available_moves = Utility.CalculateEachMoveDamage(this.pokemon, this, Player);
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
                decided_move_index = this.pokemon.getMoveIndex(highest_damaging_move.name);
                killsEnemy = Utility.IsLethal(decided_move_damage, Player.pokemon);
            }

            if(least_damaging_lethal_move != null)
            {
                decided_move_index = this.pokemon.getMoveIndex(least_damaging_lethal_move.name);
            }



            if(playerTurnsUntilFaint > 3)
            {
                Moves status_move = Utility.DecideBestStatusAttack(this.pokemon, Player.pokemon);
                if (status_move != null)
                {
                    Debug.Log("Best Status Moves: " + status_move.name);
                    if (!Player.pokemon.HasStatus(status_move.status.name)) decided_move_index = this.pokemon.getMoveIndex(status_move.name);
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