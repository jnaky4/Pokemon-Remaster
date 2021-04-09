using System;
using System.Collections;
using UnityEngine;

namespace Pokemon
{
    public class Unit : MonoBehaviour
    {
        #region Variables

        public Pokemon pokemon;

        public double damage; //Gets updated each turn depending on all of the battle factors.
        private double stab = 1; //same type attack bonus
        private double critical = 1; //critical multiplier
        private double random = 1; //some randomness to the move damage
        private double[] multipliers = new double[] { (2 / 8), (2 / 7), (2 / 6), (2 / 5), (2 / 4), (2 / 3), (2 / 2), (3 / 2), (4 / 2), (5 / 2), (6 / 2), (7 / 2), (8 / 2) }; //multipliers for all stages except accuracy and evasion
        private double[] accuracyMultipliers = new double[] { (3 / 9), (3 / 8), (3 / 7), (3 / 6), (3 / 5), (3 / 4), (3 / 3), (4 / 3), (5 / 3), (6 / 3), (7 / 3), (8 / 3), (9 / 3) };
        private double[] evasionMultipliers = new double[] { (9 / 3), (8 / 3), (7 / 3), (6 / 3), (5 / 3), (4 / 3), (3 / 3), (3 / 4), (3 / 5), (3 / 6), (3 / 7), (3 / 8), (3 / 9) };

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
        /// Sets the damage that this unit does for this turn using this specific move.
        /// </summary>
        /// <param name="enemyDefense">The enemy's defense.</param>
        /// <param name="pokemonAttack">The pokemon's attack. I think this is the super effective shit, it might not be tho. IDK.</param>
        /// <param name="attackPower">The attack power of the move</param>
        /// <param name="move">The move itself.</param>
        /// <param name="crit">if set to <c>true</c> [crit].</param>
        /// <param name="type1Defend">The type1 defense number.</param>
        /// <param name="type2Defend">The type2 defense multiplier.</param>
        public void SetDamage(double enemyDefense, double pokemonAttack, double attackPower, Moves move, bool crit, double type1Defend, double type2Defend)
        {
            try
            {
                if (pokemon.type1.type.Equals(move.move_type.type) || (pokemon.type2.type != null && pokemon.type2.type.Equals(move.move_type.type))) //If STAB
                {
                    stab = 1.5;
                }
                else
                {
                    stab = 1;
                }
            }
            catch //Because sometimes this crashes.
            {
                stab = 1;
            }
            finally
            {
                if (crit) //If it is a crit, multiply by 1.5
                {
                    critical = 1.5;
                }
                else
                {
                    critical = 1;
                }
                System.Random rnd = new System.Random();
                double num = rnd.Next(85, 100);
                random = num / 100; //Random number for the random element.
                double burn = 1;
                if (pokemon.statuses.Contains(Status.get_status("Burn")))
                {
                    burn = Status.get_status("Burn").affect_stat_mulitplier;
                }
                try
                {
                    if (move.base_power > 0) //If this does actual attacking.
                    {
                        damage = (((((2 * pokemon.level) / 5) + 2) * attackPower * (pokemonAttack / enemyDefense)) / 50) + 2; //Basic attacking
                        damage *= (critical * stab * random * type1Defend * type2Defend * burn); //Extra multipliers.
                    }
                    else
                    {
                        damage = 0;
                    }
                }
                catch (Exception ex) //If we fuck up, you will get fucked up.
                {
                    damage = 100000;
                    Debug.Log(ex.ToString());
                }
                if (damage < 0) damage = 0; //If somehow you have negative damage, now you dont.
                if (move.heal > 0)
                {
                    this.TakeDamage(-damage * move.heal);
                }
                if (move.heal < 0)
                {
                    this.TakeDamage(damage * -move.heal);
                }
            }
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
        /// <param name="enemy">The enemy whose stage might get set.</param>
        public void SetStages(Moves attack, Unit enemy)
        {
            if (attack.target.CompareTo("enemy") == 0 || attack.target.CompareTo("both") == 0) //If you are staging the enemy
            {
                switch (attack.current_stat_change) //Switch statement should be self-explanatory.
                {
                    case "Attack":
                        enemy.pokemon.attack_stage += attack.stat_change_amount; //changes it by the amount the attack changes. It is typically 1.
                        if (enemy.pokemon.attack_stage > 6) enemy.pokemon.attack_stage = 6; //If you go above or below 6, set it to 6
                        if (enemy.pokemon.attack_stage < -6) enemy.pokemon.attack_stage = -6;
                        enemy.pokemon.current_attack = enemy.pokemon.max_attack * multipliers[enemy.pokemon.attack_stage + 6]; //Makes your current stat based on the multiplier at the stage you are at.
                        break;

                    case "Defense":
                        enemy.pokemon.defense_stage += attack.stat_change_amount;
                        if (enemy.pokemon.defense_stage > 6) enemy.pokemon.defense_stage = 6;
                        if (enemy.pokemon.defense_stage < -6) enemy.pokemon.defense_stage = -6;
                        enemy.pokemon.current_defense = enemy.pokemon.max_defense * multipliers[enemy.pokemon.defense_stage + 6];
                        if (enemy.pokemon.current_defense == 0) enemy.pokemon.current_defense = 0.01;
                        break;

                    case "Speed":
                        enemy.pokemon.speed_stage += attack.stat_change_amount;
                        if (enemy.pokemon.speed_stage > 6) enemy.pokemon.speed_stage = 6;
                        if (enemy.pokemon.speed_stage < -6) enemy.pokemon.speed_stage = -6;
                        enemy.pokemon.current_speed = enemy.pokemon.max_speed * multipliers[enemy.pokemon.speed_stage + 6];
                        break;

                    case "Special Attack":
                        enemy.pokemon.sp_attack_stage += attack.stat_change_amount;
                        if (enemy.pokemon.sp_attack_stage > 6) enemy.pokemon.sp_attack_stage = 6;
                        if (enemy.pokemon.sp_attack_stage < -6) enemy.pokemon.sp_attack_stage = -6;
                        enemy.pokemon.current_sp_attack = enemy.pokemon.max_sp_attack * multipliers[enemy.pokemon.sp_attack_stage + 6];
                        break;

                    case "Special Defense":
                        enemy.pokemon.sp_defense_stage += attack.stat_change_amount;
                        if (enemy.pokemon.sp_defense_stage > 6) enemy.pokemon.sp_defense_stage = 6;
                        if (enemy.pokemon.sp_defense_stage < -6) enemy.pokemon.sp_defense_stage = -6;
                        enemy.pokemon.current_sp_defense = enemy.pokemon.max_sp_defense * multipliers[enemy.pokemon.sp_defense_stage + 6];
                        break;

                    case "Critical": //I don't know why critical is different, but I don't want to change anything now.
                        enemy.pokemon.critical_stage += attack.stat_change_amount;
                        break;

                    case "Evasion":
                        enemy.pokemon.evasion_stage += attack.stat_change_amount;
                        if (enemy.pokemon.evasion_stage > 6) enemy.pokemon.evasion_stage = 6;
                        if (enemy.pokemon.evasion_stage < -6) enemy.pokemon.evasion_stage = -6;
                        enemy.pokemon.current_evasion = 1 * evasionMultipliers[enemy.pokemon.evasion_stage + 6];
                        break;

                    case "Accuracy":
                        enemy.pokemon.sp_defense_stage += attack.stat_change_amount;
                        if (enemy.pokemon.sp_defense_stage > 6) enemy.pokemon.sp_defense_stage = 6;
                        if (enemy.pokemon.sp_defense_stage < -6) enemy.pokemon.sp_defense_stage = -6;
                        enemy.pokemon.current_accuracy = 1 * accuracyMultipliers[enemy.pokemon.accuracy_stage + 6];
                        break;

                    default:
                        break;
                }
            }
            if (attack.target.CompareTo("self") == 0 || attack.target.CompareTo("both") == 0)
            {
                switch (attack.current_stat_change)
                {
                    case "Attack":
                        pokemon.attack_stage += attack.stat_change_amount;
                        if (pokemon.attack_stage > 6) pokemon.attack_stage = 6;
                        if (pokemon.attack_stage < -6) pokemon.attack_stage = -6;
                        pokemon.current_attack = pokemon.max_attack * multipliers[pokemon.attack_stage + 6];
                        break;

                    case "Defense":
                        pokemon.defense_stage += attack.stat_change_amount;
                        if (pokemon.defense_stage > 6) pokemon.defense_stage = 6;
                        if (pokemon.defense_stage < -6) pokemon.defense_stage = -6;
                        pokemon.current_defense = pokemon.max_defense * multipliers[pokemon.defense_stage + 6];
                        if (pokemon.current_defense == 0) pokemon.current_defense = 0.01;
                        break;

                    case "Speed":
                        pokemon.speed_stage += attack.stat_change_amount;
                        if (pokemon.speed_stage > 6) pokemon.speed_stage = 6;
                        if (pokemon.speed_stage < -6) pokemon.speed_stage = -6;
                        pokemon.current_speed = pokemon.max_speed * multipliers[pokemon.speed_stage + 6];
                        break;

                    case "Special Attack":
                        pokemon.sp_attack_stage += attack.stat_change_amount;
                        if (pokemon.sp_attack_stage > 6) pokemon.sp_attack_stage = 6;
                        if (pokemon.sp_attack_stage < -6) pokemon.sp_attack_stage = -6;
                        pokemon.current_sp_attack = pokemon.max_sp_attack * multipliers[pokemon.sp_attack_stage + 6];
                        break;

                    case "Special Defense":
                        pokemon.sp_defense_stage += attack.stat_change_amount;
                        if (pokemon.sp_defense_stage > 6) pokemon.sp_defense_stage = 6;
                        if (pokemon.sp_defense_stage < -6) pokemon.sp_defense_stage = -6;
                        pokemon.current_sp_defense = pokemon.max_sp_defense * multipliers[pokemon.sp_defense_stage + 6];
                        break;

                    case "Critical":
                        pokemon.critical_stage += attack.stat_change_amount;
                        break;

                    case "Evasion":
                        pokemon.evasion_stage += attack.stat_change_amount;
                        if (pokemon.evasion_stage > 6) pokemon.evasion_stage = 6;
                        if (pokemon.evasion_stage < -6) pokemon.evasion_stage = -6;
                        pokemon.current_evasion = 1 * evasionMultipliers[pokemon.evasion_stage + 6];
                        break;

                    case "Accuracy":
                        pokemon.sp_defense_stage += attack.stat_change_amount;
                        if (pokemon.sp_defense_stage > 6) pokemon.sp_defense_stage = 6;
                        if (pokemon.sp_defense_stage < -6) pokemon.sp_defense_stage = -6;
                        pokemon.current_accuracy = 1 * accuracyMultipliers[pokemon.accuracy_stage + 6];
                        break;

                    default:
                        break;
                }
            }
        }

        public void BurnSelf()
        {
            TakeDamage(pokemon.max_hp * (.125));
        }

        public void PoisonSelf()
        {
            TakeDamage(pokemon.max_hp * (.125));
        }
    }
}