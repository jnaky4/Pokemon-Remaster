using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class Unit : MonoBehaviour
    {
        public Pokemon pokemon;

        public double damage;
        private double stab = 1;
        private double critical = 1;
        private double random = 1;
        private double[] multipliers = new double[] { (2 / 8), (2 / 7), (2 / 6), (2 / 5), (2 / 4), (2 / 3), (2 / 2), (3 / 2), (4 / 2), (5 / 2), (6 / 2), (7 / 2), (8 / 2) };
        private double[] accuracyMultipliers = new double[] { (3 / 9), (3 / 8), (3 / 7), (3 / 6), (3 / 5), (3 / 4), (3 / 3), (4 / 3), (5 / 3), (6 / 3), (7 / 3), (8 / 3), (9 / 3) };
        private double[] evasionMultipliers = new double[] { (9 / 3), (8 / 3), (7 / 3), (6 / 3), (5 / 3), (4 / 3), (3 / 3), (3 / 4), (3 / 5), (3 / 6), (3 / 7), (3 / 8), (3 / 9) };

        public void DoPP(int numMove)
        {
            pokemon.currentMoves[numMove].current_pp--;
        }

        public void SetDamage(double enemyDefense, double pokemonAttack, double attackPower, Moves move, bool crit, double type1Defend, double type2Defend)
        {
            try
            {
                if (pokemon.type1.type.Equals(move.move_type.type) || (pokemon.type2.type != null && pokemon.type2.type.Equals(move.move_type.type)))
                {
                    stab = 1.5;
                }
                else
                {
                    stab = 1;
                }
            }
            catch
            {
                stab = 1;
            }
            finally
            {
                if (crit)
                {
                    critical = 1.5;
                }
                else
                {
                    critical = 1;
                }
                System.Random rnd = new System.Random();
                double num = rnd.Next(85, 100);
                random = num / 100;

                try
                {
                    if (move.current_stat_change.CompareTo("null") == 0)
                    {
                        damage = (((((2 * pokemon.level) / 5) + 2) * attackPower * (pokemonAttack / enemyDefense)) / 50) + 2;
                        damage = damage * (critical * stab * random * type1Defend * type2Defend * pokemon.burn);
                    }
                    else
                    {
                        damage = 0;
                    }
                }
                catch(Exception ex)
                {
                    damage = 100000;
                    Debug.Log(ex.ToString());
                }
                if (damage < 0) damage = 0;
            }

        }

        public bool TakeDamage(double dmg)
        {
            pokemon.current_hp -= (int)dmg;
            if (pokemon.current_hp < 0) pokemon.current_hp = 0;
            if (pokemon.current_hp <= 0) return true;
            else return false;
        }

        public void SetStages(Moves attack, Unit enemy)
        {
            if (attack.target.CompareTo("enemy") == 0 || attack.target.CompareTo("both") == 0)
            {
                switch (attack.current_stat_change)
                {
                    case "Attack":
                        enemy.pokemon.attack_stage += attack.stat_change_amount;
                        if (enemy.pokemon.attack_stage > 6) enemy.pokemon.attack_stage = 6;
                        if (enemy.pokemon.attack_stage < -6) enemy.pokemon.attack_stage = -6;
                        enemy.pokemon.current_attack = enemy.pokemon.max_attack * multipliers[enemy.pokemon.attack_stage + 6];
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
                    case "Critical":
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
    }
}