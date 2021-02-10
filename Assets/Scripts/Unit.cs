using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class Unit : MonoBehaviour
    {
        public Pokemon pokemon;

        public bool canBeCaught = true;
        public int catchRate = 1;

        public int badge = 1;
        public int critical = 1;
        public int stab = 1;
        public int type = 1;
        public int burn = 1;

        public int damage;

        public void SetDamage(int enemyDefense, int attackPower)
        {
            try
            {
                this.damage = ((((((2 * pokemon.level) / 5) + 2) * attackPower * ( pokemon.temp_attack / enemyDefense)))/50) + 2;
                this.damage = this.damage * (badge * critical * stab * type * burn);
            }
            catch
            {
                this.damage = 100000;
            }
        }

        public bool TakeDamage(int dmg)
        {
            pokemon.temp_hp -= dmg;

            if (pokemon.temp_hp <= 0) return true;
            else return false;
        }
    }

}