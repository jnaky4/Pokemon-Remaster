using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    /// <summary>
    /// This file deals with HUD elements
    /// </summary>
    public class BattleHUD : MonoBehaviour
    {
        #region Declaration of variables
        public Text yourName;
        public Text level;
        public Slider hpSlider;
        public Text currentHP;

        public Text moves1;
        public Text moves2;
        public Text moves3;
        public Text moves4;

        public SpriteRenderer move1type;
        public Image move2type;
        public Image move3type;
        public Image move4type;

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
        #endregion
        #region Functions
        /// <summary>
        /// Sets the HUD with pokemon name, level, hp, moves, the types of balls the player has, and the pokemon the player has.
        /// Player hud.
        /// </summary>
        /// <param name="unit">The unit we want to set all of this shit is.</param>
        /// <param name="isPlayer">if set to <c>true</c> [is player].</param>
        /// <param name="player">The player object.</param>
        /// <param name="playerPokemon">The player's pokemon array.</param>
        public void SetHUD(Unit unit, bool isPlayer, PlayerBattle player, Pokemon[] playerPokemon)
        {
            yourName.text = unit.pokemon.name;
            level.text = "Level " + unit.pokemon.level;
            hpSlider.maxValue = unit.pokemon.max_hp;
            hpSlider.value = unit.pokemon.current_hp;
            currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
            if (isPlayer) SetMoves(unit);
            if (isPlayer) SetBalls(player);
            if (isPlayer) SetPokemon(playerPokemon);
            hpSlider.interactable = false;
        }
        /// <summary>
        /// Sets the HUD with just name, level, and hp.
        /// This gets called to update enemy hud.
        /// </summary>
        /// <param name="unit">The unit we want to update.</param>
        public void SetHUD(Unit unit)
        {
            yourName.text = unit.pokemon.name;
            level.text = "Level " + unit.pokemon.level;
            hpSlider.maxValue = unit.pokemon.max_hp;
            hpSlider.value = unit.pokemon.current_hp;
            currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
        }

        /// <summary>
        /// Sets the enemy hp.
        /// </summary>
        /// <param name="hp">The hp.</param>
        public void SetHP(int hp)
        {
            hpSlider.value = hp;
        }

        /// <summary>
        /// Sets the player hp.
        /// </summary>
        /// <param name="hp">The hp.</param>
        /// <param name="unit">The unit.</param>
        public void SetHP(int hp, Unit unit)
        {
            hpSlider.value = hp;
            currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
        }
        /// <summary>
        /// Sets the moves of your player's pokemon.
        /// </summary>
        /// <param name="unit">The unit we want to set.</param>
        public void SetMoves(Unit unit)
        {
            if (unit.pokemon.currentMoves[0] != null)
            {
                moves1.text = unit.pokemon.currentMoves[0].name + " " + unit.pokemon.currentMoves[0].current_pp + "/" + unit.pokemon.currentMoves[0].maxpp;
                var path = Directory.GetCurrentDirectory();
                try
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, 30, 30), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(-63, 0, 100, 100), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                }
                catch (FileNotFoundException)
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, 100, 100), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(-63, 0, 100, 100), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                }

                /*            if (unit.pokemon.currentMoves[1] != null)
                            {
                                moves2.text = unit.pokemon.currentMoves[1].name + " " + unit.pokemon.currentMoves[1].current_pp + "/" + unit.pokemon.currentMoves[1].maxpp;
                                var path = Directory.GetCurrentDirectory();
                                try
                                {
                                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                                    {
                                        //Debug.Log("Does something happen here?");
                                        move2type = path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png";
                                    }
                                    else
                                    {
                                        move2type = path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png";
                                    }
                                }
                                catch (FileNotFoundException)
                                {
                                    move2type = path + "\\Images\\Menu Icons\\" + "Type Normal.png";
                                }
                            }
                            if (unit.pokemon.currentMoves[2] != null)
                            {
                                moves3.text = unit.pokemon.currentMoves[2].name + " " + unit.pokemon.currentMoves[2].current_pp + "/" + unit.pokemon.currentMoves[2].maxpp;
                                var path = Directory.GetCurrentDirectory();
                                try
                                {
                                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                                    {
                                        //Debug.Log("Does something happen here?");
                                        move3type = path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png";
                                    }
                                    else
                                    {
                                        move3type = path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png";
                                    }
                                }
                                catch (FileNotFoundException)
                                {
                                    move3type = path + "\\Images\\Menu Icons\\" + "Type Normal.png";
                                }
                            }
                            if (unit.pokemon.currentMoves[3] != null)
                            {
                                moves4.text = unit.pokemon.currentMoves[3].name + " " + unit.pokemon.currentMoves[3].current_pp + "/" + unit.pokemon.currentMoves[3].maxpp;
                                var path = Directory.GetCurrentDirectory();
                                try
                                {
                                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                                    {
                                        //Debug.Log("Does something happen here?");
                                        move4type = path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png";
                                    }
                                    else
                                    {
                                        move4type = path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png";
                                    }
                                }
                                catch (FileNotFoundException)
                                {
                                    move4type = path + "\\Images\\Menu Icons\\" + "Type Normal.png";
                                }
                            }*/
            }
        }
            /// <summary>
            /// Sets the balls of the player.
            /// </summary>
            /// <param name="player">The player whose balls we want to set.</param>
        public void SetBalls(PlayerBattle player)
        {
            if (player.pokeBalls) pokeBalls.text = "Poke balls (" + player.numPokeBalls + ")";
            if (player.greatBalls) greatBalls.text = "Great balls (" + player.numGreatBalls + ")";
            if (player.ultraBalls) ultraBalls.text = "Ultra balls (" + player.numUltraBalls + ")";
            if (player.masterBalls) masterBalls.text = "Master balls (" + player.numMasterBalls + ")";
        }

        /// <summary>
        /// Sets the pokemon in the pokemon menu.
        /// </summary>
        /// <param name="pokemons">The array of pokemon to update in the pokemon menu.</param>
        public void SetPokemon(Pokemon[] pokemons)
        {
            if (pokemons[0] != null) pokemon1.text = pokemons[0].name + ", Level " + pokemons[0].level + ", HP: " + pokemons[0].current_hp + "/" + pokemons[0].max_hp;
            if (pokemons[1] != null) pokemon2.text = pokemons[1].name + ", Level " + pokemons[1].level + ", HP: " + pokemons[1].current_hp + "/" + pokemons[1].max_hp;
            if (pokemons[2] != null) pokemon3.text = pokemons[2].name + ", Level " + pokemons[2].level + ", HP: " + pokemons[2].current_hp + "/" + pokemons[2].max_hp;
            if (pokemons[3] != null) pokemon4.text = pokemons[3].name + ", Level " + pokemons[3].level + ", HP: " + pokemons[3].current_hp + "/" + pokemons[3].max_hp;
            if (pokemons[4] != null) pokemon5.text = pokemons[4].name + ", Level " + pokemons[4].level + ", HP: " + pokemons[4].current_hp + "/" + pokemons[4].max_hp;
            if (pokemons[5] != null) pokemon6.text = pokemons[5].name + ", Level " + pokemons[5].level + ", HP: " + pokemons[5].current_hp + "/" + pokemons[5].max_hp;
        }

        /// <summary>
        /// Sets the active pokemon.
        /// </summary>
        /// <param name="pokemons">The pokemon array.</param>
        /// <param name="num">The number of pokemon we want to make active.</param>
        /// <param name="unit">The unit of whose pokemon we are dealing with.</param>
        public void SetActivePokemon(Pokemon[] pokemons, int num, Unit unit)
        {
            unit.pokemon = pokemons[num];
            SetMoves(unit);
            SetHUD(unit);
            SetPokemon(pokemons);
        }
        #endregion
    }
}