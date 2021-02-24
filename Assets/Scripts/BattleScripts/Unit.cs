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

        public void DoPP(int numMove)
        {
            pokemon.currentMoves[numMove].current_pp--;
        }

        public void SetDamage(int enemyDefense, int attackPower, Moves move, bool crit)
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
                    this.damage = ((((((2 * pokemon.level) / 5) + 2) * attackPower * (pokemon.current_attack / enemyDefense))) / 50) + 2;
                    this.damage = this.damage * (critical * stab * random * pokemon.type * pokemon.burn);
                }
                catch
                {
                    this.damage = 100000;
                }
            }

        }

        public bool TakeDamage(double dmg)
        {
            pokemon.current_hp -= (int)dmg;

            if (pokemon.current_hp <= 0) return true;
            else return false;
        }
    }
}