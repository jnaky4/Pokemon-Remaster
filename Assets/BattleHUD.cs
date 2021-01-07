using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text name;
    public Text level;
    public Slider hpSlider;
    public Text currentHP;
       
    public void SetHUD(Unit unit)
    {
        name.text = unit.name;
        level.text = "Level " + unit.level;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        currentHP.text = unit.currentHP + "/" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetHP(int hp, Unit unit)
    {
        hpSlider.value = hp;
        currentHP.text = unit.currentHP + "/" + unit.maxHP;
    }
}
