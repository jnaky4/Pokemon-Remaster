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

    public Text moves1;
    public Text moves2;
    public Text moves3;
    public Text moves4;

    public void SetHUD(Unit unit, bool isPlayer)
    {
        name.text = unit.name;
        level.text = "Level " + unit.level;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        currentHP.text = unit.currentHP + "/" + unit.maxHP;
        if (isPlayer) SetMoves(unit);
        hpSlider.interactable = false;
        //hpSlider.colors.disabledColor = Color.black;
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
    public void SetMoves(Unit unit)
    {
        moves1.text = unit.move1.name;
        moves2.text = unit.move2.name;
        moves3.text = unit.move3.name;
        moves4.text = unit.move4.name;
    }
}
