using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class BattleHUD : MonoBehaviour
    {
        public Text yourName;
        public Text level;
        public Slider hpSlider;
        public Text currentHP;

        public Text moves1;
        public Text moves2;
        public Text moves3;
        public Text moves4;

        public Text pokeBalls;
        public Text greatBalls;
        public Text ultraBalls;
        public Text masterBalls;

        public void SetHUD(Unit unit, bool isPlayer, PlayerBattle player)
        {
            yourName.text = unit.pokemon.name;
            level.text = "Level " + unit.pokemon.level;
            hpSlider.maxValue = unit.pokemon.current_hp;
            hpSlider.value = unit.pokemon.temp_hp;
            currentHP.text = unit.pokemon.temp_hp + "/" + unit.pokemon.current_hp;
            if (isPlayer) SetMoves(unit);
            if (isPlayer) SetBalls(player);
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
            currentHP.text = unit.pokemon.temp_hp + "/" + unit.pokemon.current_hp;
        }
        public void SetMoves(Unit unit)
        {
            moves1.text = unit.pokemon.currentMoves[0].move;
            moves2.text = unit.pokemon.currentMoves[1].move;
            moves3.text = unit.pokemon.currentMoves[2].move;
            moves4.text = unit.pokemon.currentMoves[3].move;
        }

        public void SetBalls(PlayerBattle player)
        {
            if (player.pokeBalls) pokeBalls.text = "Poke balls (" + player.numPokeBalls + ")";
            if (player.greatBalls) greatBalls.text = "Great balls (" + player.numGreatBalls + ")";
            if (player.ultraBalls) ultraBalls.text = "Ultra balls (" + player.numUltraBalls + ")";
            if (player.masterBalls) masterBalls.text = "Master balls (" + player.numMasterBalls + ")";
        }
    }
}