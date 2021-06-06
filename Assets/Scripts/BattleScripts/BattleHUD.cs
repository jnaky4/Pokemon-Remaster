using System.IO;
using System.Linq;
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

        public Text currentHP;
        public Slider expSlider;
        public Text forget1;
        public Button forget1Button;
        public Image forget1type;
        public Image forget1overlay;
        public Text forget2;
        public Button forget2Button;
        public Image forget2type;
        public Image forget2overlay;
        public Text forget3;
        public Button forget3Button;
        public Image forget3type;
        public Image forget3overlay;
        public Text forget4;
        public Button forget4Button;
        public Image forget4type;
        public Image forget4overlay;
        public Text forget5;
        public Button forget5Button;
        public Image forget5type;
        public Image forget5overlay;
        public Text greatBalls;
        public Slider hpSlider;
        public Text level;
        public Text masterBalls;
        public Button move1Button;
        public Image move1type;
        public Image move1overlay;
        public Button move2Button;
        public Image move2type;
        public Image move2overlay;
        public Button move3Button;
        public Image move3type;
        public Image move3overlay;
        public Button move4Button;
        public Image move4type;
        public Image move4overlay;
        public Text moves1;
        public Text moves2;
        public Text moves3;
        public Text moves4;
        public Text pp1;
        public Text pp2;
        public Text pp3;
        public Text pp4;
        public Button poke1;
        public Button poke2;
        public Button poke3;
        public Button poke4;
        public Button poke5;
        public Button poke6;
        public Text pokeBalls;
        public Text pokemon1;
        public Image pokemon1Image;
        public Text pokemon2;
        public Image pokemon2Image;
        public Text pokemon3;
        public Image pokemon3Image;
        public Text pokemon4;
        public Image pokemon4Image;
        public Text pokemon5;
        public Image pokemon5Image;
        public Text pokemon6;
        public Image pokemon6Image;
        public Text ultraBalls;
        public Text yourName;
        private int tempHP;
        private int tempXP;
        private bool updateHp = false;
        private bool updateXP = false;
        public bool negative = false;

        public Image enemy1;
        public Image enemy2;
        public Image enemy3;
        public Image enemy4;
        public Image enemy5;
        public Image enemy6;

        public Image player1;
        public Image player2;
        public Image player3;
        public Image player4;
        public Image player5;
        public Image player6;

        #endregion Declaration of variables

        #region Functions

        public void SetUpEnemy()
        {
            if (GameController.isCatchable)
            {
                enemy6.enabled = false;
                enemy5.enabled = false;
                enemy4.enabled = false;
                enemy3.enabled = false;
                enemy2.enabled = false;
                enemy1.enabled = false;
                return;
            }
            int x = GameController.opponentPokemon.Count(s => s != null);
            if (x < 6)
            {
                enemy6.enabled = false;
            }
            if (x < 5)
            {
                enemy5.enabled = false;
            }
            if (x < 4)
            {
                enemy4.enabled = false;
            }
            if (x < 3)
            {
                enemy3.enabled = false;
            }
            if (x < 2)
            {
                enemy2.enabled = false;
            }
            if (x < 1)
            {
                enemy1.enabled = false;
            }
        }
        public void SetUpPlayer()
        {
            int x = GameController.playerPokemon.Count(s => s != null);
            if (x < 6)
            {
                player6.enabled = false;
            }
            if (x < 5)
            {
                player5.enabled = false;
            }
            if (x < 4)
            {
                player4.enabled = false;
            }
            if (x < 3)
            {
                player3.enabled = false;
            }
            if (x < 2)
            {
                player2.enabled = false;
            }
            if (x < 1)
            {
                player1.enabled = false;
            }
        }

        public void CrossOutEnemyBall(int x)
        {
            var sprite = Resources.Load<Sprite>("Images/Items/Pokeball_Dead2");
            if (x == 6)
            {
                enemy6.sprite = sprite;
            }
            if (x == 5)
            {
                enemy5.sprite = sprite;
            }
            if (x == 4)
            {
                enemy4.sprite = sprite;
            }
            if (x == 3)
            {
                enemy3.sprite = sprite;
            }
            if (x == 2)
            {
                enemy2.sprite = sprite;
            }
            if (x == 1)
            {
                enemy1.sprite = sprite;
            }
        }

        public void CrossOutPlayerBall(int x)
        {
            var sprite = Resources.Load<Sprite>("Images/Items/Pokeball_Dead2");
            if (x == 6)
            {
                player6.sprite = sprite;
            }
            if (x == 5)
            {
                player5.sprite = sprite;
            }
            if (x == 4)
            {
                player4.sprite = sprite;
            }
            if (x == 3)
            {
                player3.sprite = sprite;
            }
            if (x == 2)
            {
                player2.sprite = sprite;
            }
            if (x == 1)
            {
                player1.sprite = sprite;
            }
        }

        public void ResetStatus(Pokemon poke)
        {
            level.color = GetColorOfStatus("null");
            level.text = "Level " + poke.level;
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

        public void SetEXP(Pokemon poke, int xp)
        {
            tempXP = poke.current_exp;
            expSlider.maxValue = poke.next_lvl_exp;
            expSlider.minValue = poke.base_lvl_exp;
            updateXP = true;
        }

        public void SetEXP(Pokemon poke)
        {
            expSlider.maxValue = poke.next_lvl_exp;
            expSlider.minValue = poke.base_lvl_exp;
            expSlider.value = poke.current_exp;
        }

        public void SetForgetMoves(Unit unit)
        {
            var path = "Images/Menu Icons/Type ";
            if (unit.pokemon.currentMoves[0] != null)
            {
                forget1.text = unit.pokemon.currentMoves[0].name;

                forget1type.sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[0].move_type.name + " Hidden");

                Color c = forget1overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[0].move_type.name);
                forget1overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[1] != null)
            {
                forget2.text = unit.pokemon.currentMoves[1].name + " ";

                forget2type.sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[1].move_type.name + " Hidden");

                Color c = forget2overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[1].move_type.name);
                forget2overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[2] != null)
            {
                forget3.text = unit.pokemon.currentMoves[2].name + " ";

                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[2].move_type.name + " Hidden");
                forget3type.sprite = sprite;

                Color c = forget3overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[2].move_type.name);
                forget3overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[3] != null)
            {
                forget4.text = unit.pokemon.currentMoves[3].name + " ";

                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[3].move_type.name + " Hidden");
                forget4type.sprite = sprite;

                Color c = forget4overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[3].move_type.name);
                forget4overlay.GetComponent<Image>().color = c;
            }

            forget5.text = unit.pokemon.learned_move.name + " ";

            var s = Resources.Load<Sprite>(path + unit.pokemon.learned_move.move_type.name + " Hidden");
            forget5type.sprite = s;

            Color c5 = forget5overlay.GetComponent<Image>().color;
            c5 = GetColorOfMove(unit.pokemon.learned_move.move_type.name);
            forget5overlay.GetComponent<Image>().color = c5;
        }


        


        public void SetHP(int newHP, Unit unit, string whichplayer)
        {
            this.negative = newHP < hpSlider.value ? true : false;
            tempHP = newHP;
            if (newHP != hpSlider.value) updateHp = true;

            if (whichplayer == "Player")
            {
                if (unit.pokemon.IsFainted()) currentHP.text = 0 + "/" + unit.pokemon.max_hp;
                else currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
            }
            

        }
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
            //level.color = GetColorOfStatus("null");
            hpSlider.maxValue = unit.pokemon.max_hp;
            hpSlider.value = unit.pokemon.current_hp;
            currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
            expSlider.maxValue = unit.pokemon.next_lvl_exp;
            expSlider.minValue = unit.pokemon.base_lvl_exp;
            expSlider.value = unit.pokemon.current_exp;
            if (isPlayer) SetMoves(unit);
            if (isPlayer) SetBalls(player);
            if (isPlayer) SetPokemon(playerPokemon);
            //if (isPlayer) SetEXP(unit.pokemon);
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
            level.color = GetColorOfStatus("null");
            hpSlider.maxValue = unit.pokemon.max_hp;
            hpSlider.value = unit.pokemon.current_hp;
            currentHP.text = unit.pokemon.current_hp + "/" + unit.pokemon.max_hp;
        }

        /// <summary>
        /// Sets the moves of your player's pokemon.
        /// </summary>
        /// <param name="unit">The unit we want to set.</param>
        public void SetMoves(Unit unit)
        {
            var path = "Images/Menu Icons/Type ";
            if (unit.pokemon.currentMoves[0] != null)
            {
                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[0].move_type.name + " Hidden");
                moves1.text = unit.pokemon.currentMoves[0].name;
                pp1.text = unit.pokemon.currentMoves[0].current_pp + "/" + unit.pokemon.currentMoves[0].maxpp;
                move1type.sprite = sprite;
                Color c = move1overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[0].move_type.name);
                //c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[0].move_type.type);
                move1overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[1] != null)
            {
                moves2.text = unit.pokemon.currentMoves[1].name;
                pp2.text = unit.pokemon.currentMoves[1].current_pp + "/" + unit.pokemon.currentMoves[1].maxpp;
                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[1].move_type.name + " Hidden");
                move2type.sprite = sprite;

                Color c = move2overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[1].move_type.name);
                move2overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[2] != null)
            {
                moves3.text = unit.pokemon.currentMoves[2].name;
                pp3.text = unit.pokemon.currentMoves[2].current_pp + "/" + unit.pokemon.currentMoves[2].maxpp;

                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[2].move_type.name + " Hidden");
                move3type.sprite = sprite;

                Color c = move3overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[2].move_type.name);
                move3overlay.GetComponent<Image>().color = c;
            }
            if (unit.pokemon.currentMoves[3] != null)
            {
                moves4.text = unit.pokemon.currentMoves[3].name;
                pp4.text = unit.pokemon.currentMoves[3].current_pp + "/" + unit.pokemon.currentMoves[3].maxpp;

                var sprite = Resources.Load<Sprite>(path + unit.pokemon.currentMoves[3].move_type.name + " Hidden");
                move4type.sprite = sprite;

                Color c = move4overlay.GetComponent<Image>().color;
                c = GetColorOfMove(unit.pokemon.currentMoves[3].move_type.name);
                move4overlay.GetComponent<Image>().color = c;
            }
        }

        /// <summary>
        /// Sets the pokemon in the pokemon menu.
        /// </summary>
        /// <param name="pokemons">The array of pokemon to update in the pokemon menu.</param>
        public void SetPokemon(Pokemon[] pokemons)
        {
            string path = "Images/Menu Icons/Pokemon/";
            if (pokemons[0] != null)
            {
                pokemon1.text = "" + pokemons[0].name + ", Level " + pokemons[0].level + "\nHP: " + pokemons[0].current_hp + "/" + pokemons[0].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[0].dexnum.ToString().PadLeft(3, '0') + pokemons[0].name);
                pokemon1Image.sprite = sprite;

                ColorBlock c = poke1.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[0].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[0].type1.name);
                poke1.GetComponent<Button>().colors = c;
            }
            if (pokemons[1] != null)
            {
                pokemon2.text = "" + pokemons[1].name + ", Level " + pokemons[1].level + "\nHP: " + pokemons[1].current_hp + "/" + pokemons[1].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[1].dexnum.ToString().PadLeft(3, '0') + pokemons[1].name);
                pokemon2Image.sprite = sprite;

                ColorBlock c = poke2.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[1].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[1].type1.name);
                poke2.GetComponent<Button>().colors = c;
            }
            if (pokemons[2] != null)
            {
                pokemon3.text = "" + pokemons[2].name + ", Level " + pokemons[2].level + "\nHP: " + pokemons[2].current_hp + "/" + pokemons[2].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[2].dexnum.ToString().PadLeft(3, '0') + pokemons[2].name);
                pokemon3Image.sprite = sprite;

                ColorBlock c = poke3.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[2].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[2].type1.name);
                poke3.GetComponent<Button>().colors = c;
            }
            if (pokemons[3] != null)
            {
                pokemon4.text = "" + pokemons[3].name + ", Level " + pokemons[3].level + "\nHP: " + pokemons[3].current_hp + "/" + pokemons[3].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[3].dexnum.ToString().PadLeft(3, '0') + pokemons[3].name);
                pokemon4Image.sprite = sprite;

                ColorBlock c = poke4.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[3].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[3].type1.name);
                poke4.GetComponent<Button>().colors = c;
            }
            if (pokemons[4] != null)
            {
                pokemon5.text = "" + pokemons[4].name + ", Level " + pokemons[4].level + "\nHP: " + pokemons[4].current_hp + "/" + pokemons[4].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[4].dexnum.ToString().PadLeft(3, '0') + pokemons[4].name);
                pokemon5Image.sprite = sprite;

                ColorBlock c = poke5.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[4].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[4].type1.name);
                poke5.GetComponent<Button>().colors = c;
            }
            if (pokemons[5] != null)
            {
                pokemon6.text = "" + pokemons[5].name + ", Level " + pokemons[5].level + "\nHP: " + pokemons[5].current_hp + "/" + pokemons[5].max_hp;

                var sprite = Resources.Load<Sprite>(path + pokemons[5].dexnum.ToString().PadLeft(3, '0') + pokemons[5].name);
                pokemon6Image.sprite = sprite;

                ColorBlock c = poke6.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[5].type1.name);
                c.highlightedColor = GetDarkColorOfMove(pokemons[5].type1.name);
                poke6.GetComponent<Button>().colors = c;
            }
        }

        virtual public void SetStatus(Pokemon poke)
        {
            if (poke.statuses.Count > 0)
            {
                foreach (Status s in poke.statuses)
                {
                    if (s.persistence)
                    {
                        Color c = level.color;
                        c = GetColorOfStatus(s.name);
                        level.color = c;
                        level.text = s.name;
                        break;
                    }
                }
                //Status s = (Status)poke.statuses[0];
            }
            else
            {
                ResetStatus(poke);
            }
        }

        private Color GetColorOfMove(string type)
        {
            Color color = new Color(244f / 255f, 100f / 255f, 138f / 255f, 1);
            switch (type)
            {
                case "Normal":
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;

                case "Fire":
                    color = new Color(255f / 255f, 152f / 255f, 56f / 255f, 1);
                    break;

                case "Water":
                    color = new Color(58f / 255f, 176f / 255f, 232f / 255f, 1);
                    break;

                case "Electric":
                    color = new Color(246f / 255f, 216f / 255f, 48f / 255f, 1);
                    break;

                case "Grass":
                    color = new Color(64f / 255f, 208f / 255f, 112f / 255f, 1);
                    break;

                case "Ice":
                    color = new Color(98f / 255f, 204f / 255f, 212f / 255f, 1);
                    break;

                case "Fighting":
                    color = new Color(244f / 255f, 100f / 255f, 138f / 255f, 1);
                    break;

                case "Poison":
                    color = new Color(188f / 255f, 82f / 255f, 232f / 255f, 1);
                    break;

                case "Ground":
                    color = new Color(232f / 255f, 130f / 255f, 68f / 255f, 1);
                    break;

                case "Flying":
                    color = new Color(80f / 255f, 124f / 255f, 212f / 255f, 1);
                    break;

                case "Psychic":
                    color = new Color(255f / 255f, 136f / 255f, 130f / 255f, 1);
                    break;

                case "Bug":
                    color = new Color(153f / 255f, 204f / 255f, 51f / 255f, 1);
                    break;

                case "Rock":
                    color = new Color(196f / 255f, 174f / 255f, 112f / 255f, 1);
                    break;

                case "Ghost":
                    color = new Color(94f / 255f, 100f / 255f, 208f / 255f, 1);
                    break;

                case "Dragon":
                    color = new Color(80f / 255f, 136f / 255f, 188f / 255f, 1);
                    break;

                case "Dark":
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;

                case "Steel":
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;

                case "Fairy":
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;

                default:
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;
            }
            return color;
        }

        private Color GetColorOfStatus(string name)
        {
            Color color;
            switch (name)
            {
                case "Burn":
                    color = new Color(255f / 255f, 152f / 255f, 56f / 255f, 1);
                    break;

                case "Paralysis":
                    color = new Color(246f / 255f, 216f / 255f, 48f / 255f, 1);
                    break;

                case "Poison":
                    color = new Color(188f / 255f, 82f / 255f, 232f / 255f, 1);
                    break;

                case "Sleep":
                    color = new Color(146f / 255f, 154f / 255f, 156f / 255f, 1);
                    break;

                case "Freeze":
                    color = new Color(98f / 255f, 204f / 255f, 212f / 255f, 1);
                    break;

                default:
                    color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 1);
                    break;
            }
            return color;
        }

        private Color GetDarkColorOfMove(string type)
        {
            Color color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
            switch (type)
            {
                case "Normal":
                    color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
                    break;

                case "Fire":
                    color = new Color(156f / 255f, 83f / 255f, 31f / 255f, 1);
                    break;

                case "Water":
                    color = new Color(68f / 255f, 94f / 255f, 156f / 255f, 1);
                    break;

                case "Electric":
                    color = new Color(161f / 255f, 135f / 255f, 31f / 255f, 1);
                    break;

                case "Grass":
                    color = new Color(78f / 255f, 130f / 255f, 52f / 255f, 1);
                    break;

                case "Ice":
                    color = new Color(99f / 255f, 141f / 255f, 141f / 255f, 1);
                    break;

                case "Fighting":
                    color = new Color(125f / 255f, 31f / 255f, 26f / 255f, 1);
                    break;

                case "Poison":
                    color = new Color(104f / 255f, 42f / 255f, 104f / 255f, 1);
                    break;

                case "Ground":
                    color = new Color(146f / 255f, 125f / 255f, 68f / 255f, 1);
                    break;

                case "Flying":
                    color = new Color(109f / 255f, 94f / 255f, 156f / 255f, 1);
                    break;

                case "Psychic":
                    color = new Color(161f / 255f, 57f / 255f, 89f / 255f, 1);
                    break;

                case "Bug":
                    color = new Color(109f / 255f, 120f / 255f, 21f / 255f, 1);
                    break;

                case "Rock":
                    color = new Color(120f / 255f, 104f / 255f, 36f / 255f, 1);
                    break;

                case "Ghost":
                    color = new Color(73f / 255f, 57f / 255f, 99f / 255f, 1);
                    break;

                case "Dragon":
                    color = new Color(73f / 255f, 36f / 255f, 161f / 255f, 1);
                    break;

                case "Dark":
                    color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
                    break;

                case "Steel":
                    color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
                    break;

                case "Fairy":
                    color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
                    break;

                default:
                    color = new Color(109f / 255f, 109f / 255f, 78f / 255f, 1);
                    break;
            }
            return color;
        }

        private void Update()
        {
            if ((int)hpSlider.value == tempHP) updateHp = false;
            if (updateHp && negative)
            {
                hpSlider.value = (int)hpSlider.value - 1;
                if ((int)hpSlider.value == tempHP)
                {
                    updateHp = false;
                    tempHP = 0;
                }
            }
            if (updateHp && !negative)
            {
                hpSlider.value = (int)hpSlider.value + 1;
                if ((int)hpSlider.value == tempHP)
                {
                    updateHp = false;
                    tempHP = 0;
                }
            }

            if (expSlider.value == tempXP) updateXP = false;
            if (updateXP)
            {
                expSlider.value++;
                if (expSlider.value == tempXP) updateXP = false;
            }
        }

        #endregion Functions
    }
}