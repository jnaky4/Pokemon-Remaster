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
        public Slider expSlider;
        public Text currentHP;

        public Text moves1;
        public Text moves2;
        public Text moves3;
        public Text moves4;

        public Text forget1;
        public Text forget2;
        public Text forget3;
        public Text forget4;
        public Text forget5;

        public Image forget1type;
        public Image forget2type;
        public Image forget3type;
        public Image forget4type;
        public Image forget5type;

        public Button forget1Button;
        public Button forget2Button;
        public Button forget3Button;
        public Button forget4Button;
        public Button forget5Button;

        public Button move1Button;
        public Button move2Button;
        public Button move3Button;
        public Button move4Button;

        public Image pokemon1Image;
        public Image pokemon2Image;
        public Image pokemon3Image;
        public Image pokemon4Image;
        public Image pokemon5Image;
        public Image pokemon6Image;

        public Image move1type;
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

        public Button poke1;
        public Button poke2;
        public Button poke3;
        public Button poke4;
        public Button poke5;
        public Button poke6;

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
        public void SetEXP(Pokemon poke)
        {
            expSlider.maxValue = poke.next_lvl_exp;
            expSlider.minValue = poke.base_lvl_exp;
            expSlider.value = poke.current_exp;
        }

        public void SetStatus(Pokemon poke)
        {
            if (poke.statuses.Count > 0)
            {
                Status s = (Status)poke.statuses[0];
                Color c = level.color;
                c = GetColorOfStatus(s.name);
                level.color = c;
                level.text = s.name;
            }
            else
            {
                ResetStatus(poke);
            }
        }
        public void ResetStatus(Pokemon poke)
        {
            level.color = GetColorOfStatus("null");
            level.text = "Level " + poke.level;
        }

        private Color GetColorOfStatus(string name)
        {
            Color color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
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
                    color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
                    break;
            }
            return color;
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
                moves1.text = unit.pokemon.currentMoves[0].name + " " + unit.pokemon.currentMoves[0].current_pp + "/" + unit.pokemon.currentMoves[0].maxpp + " ";
                var path = Directory.GetCurrentDirectory();
                try
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    ColorBlock c = move1Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove(unit.pokemon.currentMoves[0].move_type.type);
                    c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[0].move_type.type);
                    move1Button.GetComponent<Button>().colors = c;
                }
                catch (FileNotFoundException)
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move1type.sprite = NewSprite;
                    }
                    ColorBlock c = move1Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove("Normal");
                    c.highlightedColor = GetDarkColorOfMove("Normal");
                    move1Button.GetComponent<Button>().colors = c;
                }
            }
            if (unit.pokemon.currentMoves[1] != null)
            {
                moves2.text = unit.pokemon.currentMoves[1].name + " " + unit.pokemon.currentMoves[1].current_pp + "/" + unit.pokemon.currentMoves[1].maxpp + " ";
                var path = Directory.GetCurrentDirectory();
                try
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move2type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(15, 15);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move2type.sprite = NewSprite;
                    }
                    ColorBlock c = move2Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove(unit.pokemon.currentMoves[1].move_type.type);
                    c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[1].move_type.type);
                    move2Button.GetComponent<Button>().colors = c;
                }
                catch (FileNotFoundException)
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move2type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move2type.sprite = NewSprite;
                    }
                    ColorBlock c = move2Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove("Normal");
                    c.highlightedColor = GetDarkColorOfMove("Normal");
                    move2Button.GetComponent<Button>().colors = c;
                }
            }
            if (unit.pokemon.currentMoves[2] != null)
            {
                moves3.text = unit.pokemon.currentMoves[2].name + " " + unit.pokemon.currentMoves[2].current_pp + "/" + unit.pokemon.currentMoves[2].maxpp + " ";
                var path = Directory.GetCurrentDirectory();
                try
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move3type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(15, 15);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move3type.sprite = NewSprite;
                    }
                    ColorBlock c = move3Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove(unit.pokemon.currentMoves[2].move_type.type);
                    c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[2].move_type.type);
                    move3Button.GetComponent<Button>().colors = c;
                }
                catch (FileNotFoundException)
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move3type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move3type.sprite = NewSprite;
                    }
                    ColorBlock c = move3Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove("Normal");
                    c.highlightedColor = GetDarkColorOfMove("Normal");
                    move3Button.GetComponent<Button>().colors = c;
                }
            }
            if (unit.pokemon.currentMoves[3] != null)
            {
                moves4.text = unit.pokemon.currentMoves[3].name + " " + unit.pokemon.currentMoves[3].current_pp + "/" + unit.pokemon.currentMoves[3].maxpp + " ";
                var path = Directory.GetCurrentDirectory();
                try
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move4type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(15, 15);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move4type.sprite = NewSprite;
                    }
                    ColorBlock c = move4Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove(unit.pokemon.currentMoves[3].move_type.type);
                    c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[3].move_type.type);
                    move4Button.GetComponent<Button>().colors = c;
                }
                catch (FileNotFoundException)
                {
                    if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move4type.sprite = NewSprite;
                    }
                    else
                    {
                        Texture2D SpriteTexture = new Texture2D(0, 0);
                        byte[] fileData;
                        fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type Normal.png");
                        SpriteTexture.LoadImage(fileData);
                        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                        move4type.sprite = NewSprite;
                    }
                    ColorBlock c = move4Button.GetComponent<Button>().colors;
                    c.normalColor = GetColorOfMove("Normal");
                    c.highlightedColor = GetDarkColorOfMove("Normal");
                    move4Button.GetComponent<Button>().colors = c;
                }
            }
        }
        public void SetForgetMoves(Unit unit)
        {
            if (unit.pokemon.currentMoves[0] != null)
            {
                forget1.text = unit.pokemon.currentMoves[0].name + " ";
                var path = Directory.GetCurrentDirectory();

                if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Texture2D SpriteTexture = new Texture2D(0, 0);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget1type.sprite = NewSprite;
                }
                else
                {
                    Texture2D SpriteTexture = new Texture2D(0, 0);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[0].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget1type.sprite = NewSprite;
                }
                ColorBlock c = forget1Button.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(unit.pokemon.currentMoves[0].move_type.type);
                c.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[0].move_type.type);
                forget1Button.GetComponent<Button>().colors = c;
            }
            if (unit.pokemon.currentMoves[1] != null)
            {
                forget2.text = unit.pokemon.currentMoves[1].name + " ";
                var path2 = Directory.GetCurrentDirectory();

                if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Texture2D SpriteTexture = new Texture2D(0, 0);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path2 + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget2type.sprite = NewSprite;
                }
                else
                {
                    Texture2D SpriteTexture = new Texture2D(15, 15);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path2 + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[1].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget2type.sprite = NewSprite;
                }
                ColorBlock c2 = forget2Button.GetComponent<Button>().colors;
                c2.normalColor = GetColorOfMove(unit.pokemon.currentMoves[1].move_type.type);
                c2.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[1].move_type.type);
                forget2Button.GetComponent<Button>().colors = c2;

            }
            if (unit.pokemon.currentMoves[2] != null)
            {
                forget3.text = unit.pokemon.currentMoves[2].name + " ";
                var path3 = Directory.GetCurrentDirectory();

                if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Texture2D SpriteTexture = new Texture2D(0, 0);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path3 + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget3type.sprite = NewSprite;
                }
                else
                {
                    Texture2D SpriteTexture = new Texture2D(15, 15);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path3 + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[2].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget3type.sprite = NewSprite;
                }
                ColorBlock c3 = forget3Button.GetComponent<Button>().colors;
                c3.normalColor = GetColorOfMove(unit.pokemon.currentMoves[2].move_type.type);
                c3.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[2].move_type.type);
                forget3Button.GetComponent<Button>().colors = c3;

            }
            if (unit.pokemon.currentMoves[3] != null)
            {
                forget4.text = unit.pokemon.currentMoves[3].name + " ";
                var path4 = Directory.GetCurrentDirectory();

                if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                {
                    Texture2D SpriteTexture = new Texture2D(0, 0);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path4 + "/Images/Menu Icons/" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget4type.sprite = NewSprite;
                }
                else
                {
                    Texture2D SpriteTexture = new Texture2D(15, 15);
                    byte[] fileData;
                    fileData = File.ReadAllBytes(path4 + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.currentMoves[3].move_type.type + ".png");
                    SpriteTexture.LoadImage(fileData);
                    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                    forget4type.sprite = NewSprite;
                }
                ColorBlock c4 = forget4Button.GetComponent<Button>().colors;
                c4.normalColor = GetColorOfMove(unit.pokemon.currentMoves[3].move_type.type);
                c4.highlightedColor = GetDarkColorOfMove(unit.pokemon.currentMoves[3].move_type.type);
                forget4Button.GetComponent<Button>().colors = c4;

            }

            forget5.text = unit.pokemon.learned_move.name + " ";
            var path5 = Directory.GetCurrentDirectory();
            if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
            {
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(path5 + "/Images/Menu Icons/" + "Type " + unit.pokemon.learned_move.move_type.type + ".png");
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                forget5type.sprite = NewSprite;
            }
            else
            {
                Texture2D SpriteTexture = new Texture2D(15, 15);
                byte[] fileData;
                fileData = File.ReadAllBytes(path5 + "\\Images\\Menu Icons\\" + "Type " + unit.pokemon.learned_move.move_type.type + ".png");
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                forget5type.sprite = NewSprite;
            }
            ColorBlock c5 = forget5Button.GetComponent<Button>().colors;
            c5.normalColor = GetColorOfMove(unit.pokemon.learned_move.move_type.type);
            c5.highlightedColor = GetDarkColorOfMove(unit.pokemon.learned_move.move_type.type);
            forget5Button.GetComponent<Button>().colors = c5;
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
                    color = new Color(255f / 255f, 136f / 255f, 130f / 255f, 1);
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
            if (pokemons[0] != null)
            {
                pokemon1.text = "" + pokemons[0].name + ", Level " + pokemons[0].level + "\nHP: " + pokemons[0].current_hp + "/" + pokemons[0].max_hp;

                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[0].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon1Image.sprite = NewSprite;

                ColorBlock c = poke1.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[0].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[0].type1.type);
                poke1.GetComponent<Button>().colors = c;

            }
            if (pokemons[1] != null)
            {
                pokemon2.text = "" + pokemons[1].name + ", Level " + pokemons[1].level + "\nHP: " + pokemons[1].current_hp + "/" + pokemons[1].max_hp;
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[1].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon2Image.sprite = NewSprite;

                ColorBlock c = poke2.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[1].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[1].type1.type);
                poke2.GetComponent<Button>().colors = c;
            }
            if (pokemons[2] != null)
            {
                pokemon3.text = "" + pokemons[2].name + ", Level " + pokemons[2].level + "\nHP: " + pokemons[2].current_hp + "/" + pokemons[2].max_hp;
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[2].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon3Image.sprite = NewSprite;

                ColorBlock c = poke3.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[2].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[2].type1.type);
                poke3.GetComponent<Button>().colors = c;
            }
            if (pokemons[3] != null)
            {
                pokemon4.text = "" + pokemons[3].name + ", Level " + pokemons[3].level + "\nHP: " + pokemons[3].current_hp + "/" + pokemons[3].max_hp;
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[3].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon4Image.sprite = NewSprite;

                ColorBlock c = poke4.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[3].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[3].type1.type);
                poke4.GetComponent<Button>().colors = c;
            }
            if (pokemons[4] != null)
            {
                pokemon5.text = "" + pokemons[4].name + ", Level " + pokemons[4].level + "\nHP: " + pokemons[4].current_hp + "/" + pokemons[4].max_hp;
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[4].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon5Image.sprite = NewSprite;

                ColorBlock c = poke5.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[4].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[4].type1.type);
                poke5.GetComponent<Button>().colors = c;

            }
            if (pokemons[5] != null)
            {
                pokemon6.text = "" + pokemons[5].name + ", Level " + pokemons[5].level + "\nHP: " + pokemons[5].current_hp + "/" + pokemons[5].max_hp;
                Texture2D SpriteTexture = new Texture2D(0, 0);
                byte[] fileData;
                fileData = File.ReadAllBytes(pokemons[5].image1);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
                pokemon6Image.sprite = NewSprite;

                ColorBlock c = poke6.GetComponent<Button>().colors;
                c.normalColor = GetColorOfMove(pokemons[5].type1.type);
                c.highlightedColor = GetDarkColorOfMove(pokemons[5].type1.type);
                poke6.GetComponent<Button>().colors = c;
            }
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