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
                String[] multiple_stat_changes = attack.target.Split(',');
                foreach (string statChange in multiple_stat_changes)
                {

                    if (attack.target == "self" || attack.target == "enemy") //If you are staging the enemy
                    {
                        switch (statChange) //Switch statement should be self-explanatory.
                        {

                            case "Attack":
                                Target.pokemon.attack_stage += attack.stat_change_amount; //changes it by the amount the attack changes. It is typically 1.
                                if (Target.pokemon.attack_stage > 6) Target.pokemon.attack_stage = 6; //If you go above or below 6, set it to 6
                                if (Target.pokemon.attack_stage < -6) Target.pokemon.attack_stage = -6;
                                Target.pokemon.current_attack = (int)(Target.pokemon.max_attack * Target.multipliers[Target.pokemon.attack_stage + 6]); //Makes your current stat based on the multiplier at the stage you are at.
                                break;

                            case "Defense":
                                Target.pokemon.defense_stage += attack.stat_change_amount;
                                if (Target.pokemon.defense_stage > 6) Target.pokemon.defense_stage = 6;
                                if (Target.pokemon.defense_stage < -6) Target.pokemon.defense_stage = -6;
                                Target.pokemon.current_defense = (int)(Target.pokemon.max_defense * Target.multipliers[Target.pokemon.defense_stage + 6]);
                                //Debug.Log("Index: " + (enemy.pokemon.defense_stage + 6) + " At Index: " + multipliers[enemy.pokemon.defense_stage + 6] + " Max: " + enemy.pokemon.max_defense +
                                //    " Current: " + enemy.pokemon.current_defense);
                                if (Target.pokemon.current_defense == (double)0) Target.pokemon.current_defense = 1;
                                break;

                            case "Speed":
                                Target.pokemon.speed_stage += attack.stat_change_amount;
                                if (Target.pokemon.speed_stage > 6) Target.pokemon.speed_stage = 6;
                                if (Target.pokemon.speed_stage < -6) Target.pokemon.speed_stage = -6;
                                Target.pokemon.current_speed = (int)((double)Target.pokemon.max_speed * Target.multipliers[Target.pokemon.speed_stage + 6]);
                                break;

                            case "Special Attack":
                                Target.pokemon.sp_attack_stage += attack.stat_change_amount;
                                if (Target.pokemon.sp_attack_stage > 6) Target.pokemon.sp_attack_stage = 6;
                                if (Target.pokemon.sp_attack_stage < -6) Target.pokemon.sp_attack_stage = -6;
                                Target.pokemon.current_sp_attack = (int)(Target.pokemon.max_sp_attack * Target.multipliers[Target.pokemon.sp_attack_stage + 6]);
                                break;

                            case "Special Defense":
                                Target.pokemon.sp_defense_stage += attack.stat_change_amount;
                                if (Target.pokemon.sp_defense_stage > 6) Target.pokemon.sp_defense_stage = 6;
                                if (Target.pokemon.sp_defense_stage < -6) Target.pokemon.sp_defense_stage = -6;
                                Target.pokemon.current_sp_defense = (int)(Target.pokemon.max_sp_defense * Target.multipliers[Target.pokemon.sp_defense_stage + 6]);
                                break;

                            case "Critical": //I don't know why critical is different, but I don't want to change anything now.
                                Target.pokemon.critical_stage += attack.stat_change_amount;
                                break;

                            case "Evasion":
                                Target.pokemon.evasion_stage += attack.stat_change_amount;
                                if (Target.pokemon.evasion_stage > 6) Target.pokemon.evasion_stage = 6;
                                if (Target.pokemon.evasion_stage < -6) Target.pokemon.evasion_stage = -6;
                                Target.pokemon.current_evasion = (int)(1 * Target.evasionMultipliers[Target.pokemon.evasion_stage + 6]);
                                break;

                            case "Accuracy":
                                Target.pokemon.sp_defense_stage += attack.stat_change_amount;
                                if (Target.pokemon.sp_defense_stage > 6) Target.pokemon.sp_defense_stage = 6;
                                if (Target.pokemon.sp_defense_stage < -6) Target.pokemon.sp_defense_stage = -6;
                                Target.pokemon.current_accuracy = (int)(1 * Target.accuracyMultipliers[Target.pokemon.accuracy_stage + 6]);
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

            //without prioritymove, who will go first
            BattleState whoGoesFirst = Utility.WhoGoesFirst(Moves.get_move("Tackle"), playerAttack, pokemon, Player.pokemon);
            //Debug.Log(whoGoesFirst);

            //Data on players Pokemon
            double playerEmul = Utility.EffectivenessMultiplier(playerAttack, pokemon);
            double playerStab = Utility.STAB(playerAttack, Player.pokemon);
            int playerDamage = Utility.CalculateDamage(Player, this, playerAttack, false, playerEmul, playerStab);
            bool playerAttackLethal = Utility.isLethal(playerDamage, pokemon);


            int AIturnsUntilFaint = Utility.turnsUntilFaint(playerDamage, pokemon, whoGoesFirst);


             

            Debug.Log("AI HP: " + pokemon.current_hp);
            Debug.Log("Player Damage: " + playerDamage);
            Debug.Log("AI will faint in " + AIturnsUntilFaint + " turns");


            //get sum of moves
            int num_moves = this.pokemon.CountMoves();
            //Debug.Log("Num Moves: " + num_moves);

            int decided_attack = 0;
            int decided_attack_damage = 0;
            int playerTurnsUntilFaint = -1;

            bool killsEnemy = false;

            
            if(whoGoesFirst == BattleState.PLAYERTURN && playerAttackLethal)
            {
                Debug.Log("AI will die before it can attack");
                //TODO Check for swap
                return 0;
            }

            
            //Debug.Log("Enemy has " + enemyhealth + " hp.");
            

            //does basic damage calculation for each move the AI pokemon has
            for (int i = 0; i < num_moves; i++)
            {
                //Debug.Log(this.pokemon.currentMoves[i].name);

                if(this.pokemon.currentMoves[i].base_power > 0 && this.pokemon.currentMoves[i].current_pp > 0)
                {
                    double emul = Utility.EffectivenessMultiplier(this.pokemon.currentMoves[i], Player.pokemon);
                    double stab = Utility.STAB(this.pokemon.currentMoves[i], this.pokemon);
                    //Debug.Log("Effectiveness: " + emul);
                    int potentialDmg = Utility.CalculateDamage(this, Player, this.pokemon.currentMoves[i], false, emul, stab);
                    available_moves.Add(this.pokemon.currentMoves[i].name, potentialDmg);
                    Debug.Log(this.pokemon.currentMoves[i].name + " might do " + potentialDmg + " damage.");
                }
            }


            //sets decided attack to which move will do the most damage
            foreach (KeyValuePair<string, int> attack in available_moves)
            {
                if(attack.Value > decided_attack_damage)
                {
                    decided_attack = this.pokemon.getMoveIndex(attack.Key);
                    decided_attack_damage = attack.Value;
                    killsEnemy = Utility.isLethal(decided_attack_damage, Player.pokemon);
                    //Debug.Log(attack.Key + " Lethal? " + Utility.isLethal(decided_attack_damage, Player.pokemon));
                    playerTurnsUntilFaint = Utility.turnsUntilFaint(decided_attack_damage, Player.pokemon, whoGoesFirst);
                    Debug.Log("Player HP: " + Player.pokemon.current_hp);
                    Debug.Log("AI Damage: " + decided_attack_damage);
                    Debug.Log("Player will faint in " + playerTurnsUntilFaint + " turns");

                }

            }
                
            return decided_attack;
        }
        


    }
}