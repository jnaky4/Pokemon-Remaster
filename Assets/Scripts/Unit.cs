using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool canBeCaught = true;
    public int catchRate = 1;

    public string name;
    public int level;
    public Pkm_Moves move1;
    public Pkm_Moves move2;
    public Pkm_Moves move3;
    public Pkm_Moves move4;

    public int attack;
    public int defense;
    public int speed;

    public int badge = 1;
    public int critical = 2;
    public int stab = 1;
    public int type = 2;
    public int burn = 1;

    public int damage;

    public int maxHP;
    public int currentHP;

    public void SetDamage(int enemyDefense, int attackPower)
    {
        this.damage = (((((2 * level) / 5) + 2) * 1 * (attackPower/enemyDefense))) + 2;
        this.damage = this.damage * (badge * critical * stab * type * burn);
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        
        if (currentHP <= 0) return true;
        else return false;
    }
}



