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

        public Text pokemon1;
        public Text pokemon2;
        public Text pokemon3;
        public Text pokemon4;
        public Text pokemon5;
        public Text pokemon6;

        public void SetHUD(Unit unit, bool isPlayer, PlayerBattle player, Pokemon[] playerPokemon)
        {
            yourName.text = unit.pokemon.name;
            level.text = "Level " + unit.pokemon.level;
            hpSlider.maxValue = unit.pokemon.current_hp;
            hpSlider.value = unit.pokemon.temp_hp;
            currentHP.text = unit.pokemon.temp_hp + "/" + unit.pokemon.current_hp;
            if (isPlayer) SetMoves(unit);
            if (isPlayer) SetBalls(player);
            if (isPlayer) SetPokemon(playerPokemon);
            hpSlider.interactable = false;
            //hpSlider.colors.disabledColor = Color.black;
        }
        public void SetHUD(Unit unit)
        {
            yourName.text = unit.pokemon.name;
            level.text = "Level " + unit.pokemon.level;
            hpSlider.maxValue = unit.pokemon.current_hp;
            hpSlider.value = unit.pokemon.temp_hp;
            currentHP.text = unit.pokemon.temp_hp + "/" + unit.pokemon.current_hp;
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

        public void SetPokemon(Pokemon[] pokemons)
        {
            if (pokemons[0] != null) pokemon1.text = pokemons[0].name + ", Level " + pokemons[0].level + ", HP: " + pokemons[0].temp_hp + "/" + pokemons[0].current_hp;
            if (pokemons[1] != null) pokemon2.text = pokemons[1].name + ", Level " + pokemons[1].level + ", HP: " + pokemons[1].temp_hp + "/" + pokemons[1].current_hp;
            if (pokemons[2] != null) pokemon3.text = pokemons[2].name + ", Level " + pokemons[2].level + ", HP: " + pokemons[2].temp_hp + "/" + pokemons[2].current_hp;
            if (pokemons[3] != null) pokemon4.text = pokemons[3].name + ", Level " + pokemons[3].level + ", HP: " + pokemons[3].temp_hp + "/" + pokemons[3].current_hp;
            if (pokemons[4] != null) pokemon5.text = pokemons[4].name + ", Level " + pokemons[4].level + ", HP: " + pokemons[4].temp_hp + "/" + pokemons[4].current_hp;
            if (pokemons[5] != null) pokemon6.text = pokemons[5].name + ", Level " + pokemons[5].level + ", HP: " + pokemons[5].temp_hp + "/" + pokemons[5].current_hp;
        }
        public void SetActivePokemon(Pokemon[] pokemons, int num, Unit unit)
        {
            unit.pokemon = pokemons[num];
            SetMoves(unit);
            SetHUD(unit);
        }
    }
}