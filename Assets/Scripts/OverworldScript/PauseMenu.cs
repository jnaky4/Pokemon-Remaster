using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.IO;

namespace Pokemon
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;
        public VectorValue playerPosition;
        public GameObject pauseMenuUI;
        public GameObject saveButton;
        public GameObject inventoryMenu;
        public GameObject panel;
        public Transform itemSlotTemplate;

        public Text progress;
        public GameObject progressBadges;
        public GameObject otherProgress;
        public Image pokedexImage;
        public Image brock;
        public Image misty;
        public Image surge;
        public Image erika;
        public Image koga;
        public Image sabrina;
        public Image blane;
        public Image gio;

        public Image pokeImage1;
        public Image pokeImage2;
        public Image pokeImage3;
        public Image pokeImage4;
        public Image pokeImage5;
        public Image pokeImage6;

        public GameObject pokeCanvas1;
        public GameObject pokeCanvas2;
        public GameObject pokeCanvas3;
        public GameObject pokeCanvas4;
        public GameObject pokeCanvas5;
        public GameObject pokeCanvas6;

        public GameObject pokeUI;
        //public GameObject //pokePosition;

        public Text pokeName1;
        public Text pokeName2;
        public Text pokeName3;
        public Text pokeName4;
        public Text pokeName5;
        public Text pokeName6;

        public Dropdown pokeDrop1;
        public Dropdown pokeDrop2;
        public Dropdown pokeDrop3;
        public Dropdown pokeDrop4;
        public Dropdown pokeDrop5;
        public Dropdown pokeDrop6;

        public Image pokeColor1;
        public Image pokeColor2;
        public Image pokeColor3;
        public Image pokeColor4;
        public Image pokeColor5;
        public Image pokeColor6;

        public GameObject releaseUI;
        public Text releaseText;
        private int releaseNumber = 7;

        public Text playerName;
        public Text time;
        public Text money;

        private void Start()
        {
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(false);
            releaseUI.SetActive(false);
            //pokePosition.SetActive(false);
            progress.text = GameController.player.name;
            setItems();
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameController.inCombat)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (GameIsPaused)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pokeUI.SetActive(false);
            releaseUI.SetActive(false);
            inventoryMenu.SetActive(false);
            //pokePosition.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        void Pause()
        {
            pauseMenuUI.SetActive(true);
            if (SceneManager.GetActiveScene().name != "Pallet Town")
                saveButton.SetActive(false);
            else
                saveButton.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            SetImages();
            SetPokemon();
        }

        public void DisplayItemMenu()
        {
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(false);
            releaseUI.SetActive(false);
            inventoryMenu.SetActive(true);
        }

        public void setItems() {
            foreach(Items item in GameController.inventory)
            {
                var itemSlot = Instantiate(itemSlotTemplate, panel.transform);
                itemSlot.GetComponentsInChildren<Text>()[0].text = item.name;
                itemSlot.GetComponentsInChildren<Text>()[1].text = item.count.ToString();

                string path = Path.Combine("Images", "Items", item.name);
                itemSlot.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(path);          

            }
        }

        private void SetPokemon()
        {
            var x = 0;
            string path = Path.Combine("Images","Menu Icons", "Pokemon");
            if (GameController.playerPokemon[0] != null)
            {
                x++;
                pokeCanvas1.SetActive(true);
                pokeImage1.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[0].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[0].name));
                pokeName1.text = "" + GameController.playerPokemon[0].name + ", Level " + GameController.playerPokemon[0].level + "\nHP: " + GameController.playerPokemon[0].current_hp + "/" + GameController.playerPokemon[0].max_hp;
                pokeColor1.color = GetColorOfMove(GameController.playerPokemon[0].type1.name);
            }
            else pokeCanvas1.SetActive(false);

            if (GameController.playerPokemon[1] != null)
            {
                x++;
                pokeCanvas2.SetActive(true);
                pokeImage2.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[1].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[1].name));
                pokeName2.text = "" + GameController.playerPokemon[1].name + ", Level " + GameController.playerPokemon[1].level + "\nHP: " + GameController.playerPokemon[1].current_hp + "/" + GameController.playerPokemon[1].max_hp;
                pokeColor2.color = GetColorOfMove(GameController.playerPokemon[1].type1.name);
            }
            else pokeCanvas2.SetActive(false);
            if (GameController.playerPokemon[2] != null)
            {
                x++;
                pokeCanvas3.SetActive(true);
                pokeImage3.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[2].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[2].name));
                pokeName3.text = "" + GameController.playerPokemon[2].name + ", Level " + GameController.playerPokemon[2].level + "\nHP: " + GameController.playerPokemon[2].current_hp + "/" + GameController.playerPokemon[2].max_hp;
                pokeColor3.color = GetColorOfMove(GameController.playerPokemon[2].type1.name);
            }
            else pokeCanvas3.SetActive(false);
            if (GameController.playerPokemon[3] != null)
            {
                x++;
                pokeCanvas4.SetActive(true);
                pokeImage4.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[3].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[3].name));
                pokeName4.text = "" + GameController.playerPokemon[3].name + ", Level " + GameController.playerPokemon[3].level + "\nHP: " + GameController.playerPokemon[3].current_hp + "/" + GameController.playerPokemon[3].max_hp;
                pokeColor4.color = GetColorOfMove(GameController.playerPokemon[3].type1.name);
            }
            else pokeCanvas4.SetActive(false);
            if (GameController.playerPokemon[4] != null)
            {
                x++;
                pokeCanvas5.SetActive(true);
                pokeImage5.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[4].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[4].name));
                pokeName5.text = "" + GameController.playerPokemon[4].name + ", Level " + GameController.playerPokemon[4].level + "\nHP: " + GameController.playerPokemon[4].current_hp + "/" + GameController.playerPokemon[4].max_hp;
                pokeColor5.color = GetColorOfMove(GameController.playerPokemon[4].type1.name);
            }
            else pokeCanvas5.SetActive(false);
            if (GameController.playerPokemon[5] != null)
            {
                x++;
                pokeCanvas6.SetActive(true);
                pokeImage6.sprite = Resources.Load<Sprite>(Path.Combine(path, GameController.playerPokemon[5].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[5].name));
                pokeName6.text = "" + GameController.playerPokemon[5].name + ", Level " + GameController.playerPokemon[5].level + "\nHP: " + GameController.playerPokemon[5].current_hp + "/" + GameController.playerPokemon[5].max_hp;
                pokeColor6.color = GetColorOfMove(GameController.playerPokemon[5].type1.name);
            }
            else pokeCanvas6.SetActive(false);

            if (x >= 6) SetDropdown(pokeDrop6, x, 6);
            if (x >= 5) SetDropdown(pokeDrop5, x, 5);
            if (x >= 4) SetDropdown(pokeDrop4, x, 4);
            if (x >= 3) SetDropdown(pokeDrop3, x, 3);
            if (x >= 2) SetDropdown(pokeDrop2, x, 2);
            if (x >= 1) SetDropdown(pokeDrop1, x, 1);
        }

        private void SetDropdown(Dropdown d, int i, int k)
        {
            d.options.Clear();
            for (int j = 1; j <= i; j++)
            {
                var x = new Dropdown.OptionData(j.ToString());
                d.options.Add(x);
            }
            d.value = k - 1;
        }

        public void ReleasePokemon(int i)
        {
            if (GameController.playerPokemon[1] == null) return;
            pokeUI.SetActive(false);
            releaseUI.SetActive(true);
            releaseText.text = "Are you sure you want to release " + GameController.playerPokemon[i] + "?";
            releaseNumber = i;
        }
        public void OnYes()
        {
            GameController.playerPokemon[releaseNumber] = null;

            for (int i = 0; i < GameController.playerPokemon.Count(s => s != null) + 1; i++)
            {
                if (GameController.playerPokemon[i] == null)
                {
                    GameController.playerPokemon[i] = GameController.playerPokemon[i + 1];
                    GameController.playerPokemon[i + 1] = null;
                }
            }

            SetPokemon();
            releaseNumber = 7;
            releaseUI.SetActive(false);
            pokeUI.SetActive(true);
        }
        public void OnNo()
        {
            releaseUI.SetActive(false);
            pokeUI.SetActive(true);
            releaseNumber = 7;
        }

        public void ChangeDropdown(Dropdown active)
        {
            if (pokeDrop1.value == active.value && GameController.playerPokemon[0] != null)
            {
                var temp = GameController.playerPokemon[0];
                GameController.playerPokemon[0] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            if (pokeDrop2.value == active.value && GameController.playerPokemon[1] != null)
            {
                var temp = GameController.playerPokemon[1];
                GameController.playerPokemon[1] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            if (pokeDrop3.value == active.value && GameController.playerPokemon[2] != null)
            {
                var temp = GameController.playerPokemon[2];
                GameController.playerPokemon[2] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            if (pokeDrop4.value == active.value && GameController.playerPokemon[3] != null)
            {
                var temp = GameController.playerPokemon[3];
                GameController.playerPokemon[3] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            if (pokeDrop5.value == active.value && GameController.playerPokemon[4] != null)
            {
                var temp = GameController.playerPokemon[4];
                GameController.playerPokemon[4] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            if (pokeDrop6.value == active.value && GameController.playerPokemon[5] != null)
            {
                var temp = GameController.playerPokemon[5];
                GameController.playerPokemon[5] = GameController.playerPokemon[active.value];
                GameController.playerPokemon[active.value] = temp;
            }
            SetPokemon();
        }

        private Vector3 GetPosition(int x, Vector3 defaultPos)
        {
            Vector3 pos;
            switch (x)
            {
                case 1:
                    pos = new Vector3(0, 450, 0);
                    break;
                case 2:
                    pos = new Vector3(0, 270, 0);
                    break;
                case 3:
                    pos = new Vector3(0, 90, 0);
                    break;
                case 4:
                    pos = new Vector3(0, -90, 0);
                    break;
                case 5:
                    pos = new Vector3(0, -270, 0);
                    break;
                case 6:
                    pos = new Vector3(0, -450, 0);
                    break;
                default:
                    pos = defaultPos;
                    break;
            }
            return pos;
        }

        void SetImages()
        {
            pokedexImage.sprite = Resources.Load<Sprite>("Images/Menu Icons/Pokemon/" + GameController.playerPokemon[0].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[0].name);
            if (GameController.badges_completed.ContainsKey("Rock"))
            {
                brock.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Boulder Complete");
            }
            if (GameController.badges_completed.ContainsKey("Water"))
            {
                misty.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Cascade Complete");
            }
            if (GameController.badges_completed.ContainsKey("Electric"))
            {
                surge.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Thunder Complete");
            }
            if (GameController.badges_completed.ContainsKey("Grass"))
            {
                erika.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Rainbow Complete");
            }
            if (GameController.badges_completed.ContainsKey("Poison"))
            {
                koga.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Marsh Complete");
            }
            if (GameController.badges_completed.ContainsKey("Psychic"))
            {
                sabrina.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Soul Complete");
            }
            if (GameController.badges_completed.ContainsKey("Fire"))
            {
                blane.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Volcano Complete");
            }
            if (GameController.badges_completed.ContainsKey("Ground"))
            {
                gio.sprite = Resources.Load<Sprite>("Images/Menu Icons/Badge Earth Complete");
            }

            money.text = "Money: " + "$" + GameController.player.money;
            playerName.text = "Trainer Name: " + GameController.player.name;
            time.text = "Total play time: " + ((int) (UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60 / 60 % 60) + " Hours, " + ((int)(UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60 % 60) + " Minutes";
        }

        public void ProgressMenu()
        {
            progressBadges.SetActive(true);
            otherProgress.SetActive(true);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(false);
            releaseUI.SetActive(false);
            inventoryMenu.SetActive(false);
            //pokePosition.SetActive(false);
        }

        public void PokeMenu()
        {
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pauseMenuUI.SetActive(false);
            releaseUI.SetActive(false);
            pokeUI.SetActive(true);
            inventoryMenu.SetActive(false);
            //pokePosition.SetActive(true);
        }

        public void Save()
        {
            PlayerPrefs.SetInt("Pokeball", GameController.player.pokeBalls);
            PlayerPrefs.SetInt("Greatball", GameController.player.greatBalls);
            PlayerPrefs.SetInt("Ultraball", GameController.player.ultraBalls);
            PlayerPrefs.SetInt("Masterball", GameController.player.masterBalls);
            PlayerPrefs.SetString("DisplayPoke", GameController.player.displayPokeBalls.ToString());
            PlayerPrefs.SetString("DisplayGreat", GameController.player.displayGreatBalls.ToString());
            PlayerPrefs.SetString("DisplayUltra", GameController.player.displayUltraBalls.ToString());
            PlayerPrefs.SetString("DisplayMaster", GameController.player.displayMasterBalls.ToString());

            GameController.player.time += UnityEngine.Time.realtimeSinceStartup;
            PlayerPrefs.SetFloat("Time", (float)GameController.player.time);

            //PlayerPrefs.SetString("")

            PlayerPrefs.SetInt("Money", GameController.player.money);
            PlayerPrefs.SetString("Name", GameController.player.name);
            PlayerPrefs.SetString("Location", GameController.location);
            PlayerPrefs.SetString("Scene", GameController.scene);
            PlayerPrefs.SetFloat("X", GameObject.FindGameObjectWithTag("Player").transform.position.x);
            PlayerPrefs.SetFloat("Y", GameObject.FindGameObjectWithTag("Player").transform.position.y);
            PlayerPrefs.SetInt("HasStarter", Convert.ToInt32(GameController.starterChosen));
            PlayerPrefs.SetInt("Starter", GameController.player.starter);


            for (int i = 0; i < 6; i++)
            {
                if (GameController.playerPokemon[i] != null)
                {
                    PlayerPrefs.SetInt("Pokemon" + i + "_level", GameController.playerPokemon[i].level);
                    PlayerPrefs.SetInt("Pokemon" + i + "_dex", GameController.playerPokemon[i].dexnum);
                    for (int j = 0; j < 4; j++)
                    {
                        if (GameController.playerPokemon[i].currentMoves[j] != null)
                        {
                            PlayerPrefs.SetString("Pokemon" + i + "_move" + j, GameController.playerPokemon[i].currentMoves[j].name);
                        }
                        else PlayerPrefs.SetString("Pokemon" + i + "_move" + j, "null");
                    }
                    PlayerPrefs.SetInt("Pokemon" + i + "_exp", GameController.playerPokemon[i].current_exp);
                    PlayerPrefs.SetInt("Pokemon" + i + "_hp", GameController.playerPokemon[i].current_hp);
                }
                else PlayerPrefs.SetInt("Pokemon" + i + "_level", 0);
            }

            if (GameController.badges_completed.ContainsKey("Rock"))
            {
                PlayerPrefs.SetInt("BadgeRock", 1);
            }
            else PlayerPrefs.SetInt("BadgeRock", 0);
            if (GameController.badges_completed.ContainsKey("Water"))
            {
                PlayerPrefs.SetInt("BadgeWater", 1);
            }
            else PlayerPrefs.SetInt("BadgeWater", 0);
            if (GameController.badges_completed.ContainsKey("Electric"))
            {
                PlayerPrefs.SetInt("BadgeElectric", 1);
            }
            else PlayerPrefs.SetInt("BadgeElectric", 0);
            if (GameController.badges_completed.ContainsKey("Grass"))
            {
                PlayerPrefs.SetInt("BadgeGrass", 1);
            }
            else PlayerPrefs.SetInt("BadgeGrass", 0);
            if (GameController.badges_completed.ContainsKey("Poison"))
            {
                PlayerPrefs.SetInt("BadgePoison", 1);
            }
            else PlayerPrefs.SetInt("BadgePoison", 0);
            if (GameController.badges_completed.ContainsKey("Psychic"))
            {
                PlayerPrefs.SetInt("BadgePsychic", 1);
            }
            else PlayerPrefs.SetInt("BadgePsychic", 0);
            if (GameController.badges_completed.ContainsKey("Fire"))
            {
                PlayerPrefs.SetInt("BadgeFire", 1);
            }
            else PlayerPrefs.SetInt("BadgeFire", 0);
            if (GameController.badges_completed.ContainsKey("Ground"))
            {
                PlayerPrefs.SetInt("BadgeGround", 1);
            }
            else PlayerPrefs.SetInt("BadgeGround", 0);

            PlayerPrefs.Save();
        }

        public void Load()
        {
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            GameController.player.pokeBalls = PlayerPrefs.GetInt("Pokeball", 20);
            GameController.player.greatBalls = PlayerPrefs.GetInt("Greatball", 20);
            GameController.player.ultraBalls = PlayerPrefs.GetInt("Ultraball", 20);
            GameController.player.masterBalls = PlayerPrefs.GetInt("Masterball", 20);

            GameController.player.displayPokeBalls = PlayerPrefs.GetString("DisplayPoke").Equals("True");
            GameController.player.displayGreatBalls = PlayerPrefs.GetString("DisplayGreat").Equals("True");
            GameController.player.displayUltraBalls = PlayerPrefs.GetString("DisplayUltra").Equals("True");
            GameController.player.displayMasterBalls = PlayerPrefs.GetString("DisplayMaster").Equals("True");

            GameController.player.time = PlayerPrefs.GetFloat("Time");

            GameController.player.money = PlayerPrefs.GetInt("Money");
            GameController.player.name = PlayerPrefs.GetString("Name");
            GameController.location = PlayerPrefs.GetString("Location");
            GameController.scene = PlayerPrefs.GetString("Scene");

            //var t = GameObject.FindGameObjectWithTag("Player").transform.position;

            //t.x = PlayerPrefs.GetFloat("X");
            //t.y = PlayerPrefs.GetFloat("Y");

            playerPosition.initialValue.x = PlayerPrefs.GetFloat("X");
            playerPosition.initialValue.y = PlayerPrefs.GetFloat("Y");


            for (int i = 0; i < 6; i++)
            {
                if (PlayerPrefs.GetInt("Pokemon" + i + "_level") == 0)
                {
                    continue;
                }
                else
                {
                    int level = PlayerPrefs.GetInt("Pokemon" + i + "_level");
                    int dex = PlayerPrefs.GetInt("Pokemon" + i + "_dex");
                    int exp = PlayerPrefs.GetInt("Pokemon" + i + "_exp");
                    int hp = PlayerPrefs.GetInt("Pokemon" + i + "_hp");
                    string move1 = null, move2 = null, move3 = null, move4 = null;

                    if (!PlayerPrefs.GetString("Pokemon" + i + "_move0").Equals("null")) move1 = PlayerPrefs.GetString("Pokemon" + i + "_move0");
                    if (!PlayerPrefs.GetString("Pokemon" + i + "_move1").Equals("null")) move2 = PlayerPrefs.GetString("Pokemon" + i + "_move1");
                    if (!PlayerPrefs.GetString("Pokemon" + i + "_move2").Equals("null")) move3 = PlayerPrefs.GetString("Pokemon" + i + "_move2");
                    if (!PlayerPrefs.GetString("Pokemon" + i + "_move3").Equals("null")) move4 = PlayerPrefs.GetString("Pokemon" + i + "_move3");

                    GameController.playerPokemon[i] = new Pokemon(dex, level, move1, move2, move3, move4, exp, hp);
                }
            }

            if (PlayerPrefs.GetInt("BadgeRock") == 1 && !GameController.badges_completed.ContainsKey("Rock")) GameController.badges_completed.Add("Rock", 1);
            if (PlayerPrefs.GetInt("BadgeWater") == 1 && !GameController.badges_completed.ContainsKey("Water")) GameController.badges_completed.Add("Water", 1);
            if (PlayerPrefs.GetInt("BadgeElectric") == 1 && !GameController.badges_completed.ContainsKey("Electric")) GameController.badges_completed.Add("Electric", 1);
            if (PlayerPrefs.GetInt("BadgeGrass") == 1 && !GameController.badges_completed.ContainsKey("Grass")) GameController.badges_completed.Add("Grass", 1);
            if (PlayerPrefs.GetInt("BadgePoison") == 1 && !GameController.badges_completed.ContainsKey("Poison")) GameController.badges_completed.Add("Poison", 1);
            if (PlayerPrefs.GetInt("BadgePsychic" ) == 1 && !GameController.badges_completed.ContainsKey("Psychic")) GameController.badges_completed.Add("Psychic", 1);
            if (PlayerPrefs.GetInt("BadgeFire") == 1 && !GameController.badges_completed.ContainsKey("Fire")) GameController.badges_completed.Add("Fire", 1);
            if (PlayerPrefs.GetInt("BadgeGround") == 1 && !GameController.badges_completed.ContainsKey("Ground")) GameController.badges_completed.Add("Ground", 1);

            GameController.update_level_cap();

            SceneManager.LoadSceneAsync(GameController.scene);
            Resume();
            pauseMenuUI.SetActive(true);
        }

        public void QuitGame()
        {
            //Debug.Log("Quitting Game");
            Application.Quit();
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
    }
}