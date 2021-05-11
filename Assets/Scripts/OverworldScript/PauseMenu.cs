using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;
        public VectorValue playerPosition;
        public GameObject pauseMenuUI;
        public GameObject saveButton;
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

        public Image poke1;
        public Image poke2;
        public Image poke3;
        public Image poke4;
        public Image poke5;
        public Image poke6;

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

        public Text playerName;
        public Text time;
        public Text money;

        private void Start()
        {
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(false);
            //pokePosition.SetActive(false);
            progress.text = GameController.player.name;
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

        private void SetPokemon()
        {
            string path = "Images/Menu Icons/Pokemon/";
            if (GameController.playerPokemon[0] != null)
            {
                pokeCanvas1.SetActive(true);
                poke1.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[0].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[0].name);
                pokeName1.text = "" + GameController.playerPokemon[0].name + ", Level " + GameController.playerPokemon[0].level + "\nHP: " + GameController.playerPokemon[0].current_hp + "/" + GameController.playerPokemon[0].max_hp;
            }
            else pokeCanvas1.SetActive(false);

            if (GameController.playerPokemon[1] != null)
            {
                pokeCanvas2.SetActive(true);
                poke2.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[1].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[1].name);
                pokeName2.text = "" + GameController.playerPokemon[1].name + ", Level " + GameController.playerPokemon[1].level + "\nHP: " + GameController.playerPokemon[1].current_hp + "/" + GameController.playerPokemon[1].max_hp;
            }
            else pokeCanvas2.SetActive(false);
            if (GameController.playerPokemon[2] != null)
            {
                pokeCanvas3.SetActive(true);
                poke3.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[2].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[2].name);
                pokeName3.text = "" + GameController.playerPokemon[2].name + ", Level " + GameController.playerPokemon[2].level + "\nHP: " + GameController.playerPokemon[2].current_hp + "/" + GameController.playerPokemon[2].max_hp;
            }
            else pokeCanvas3.SetActive(false);
            if (GameController.playerPokemon[3] != null)
            {
                pokeCanvas4.SetActive(true);
                poke4.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[3].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[3].name);
                pokeName4.text = "" + GameController.playerPokemon[3].name + ", Level " + GameController.playerPokemon[3].level + "\nHP: " + GameController.playerPokemon[3].current_hp + "/" + GameController.playerPokemon[3].max_hp;
            }
            else pokeCanvas4.SetActive(false);
            if (GameController.playerPokemon[4] != null)
            {
                pokeCanvas5.SetActive(true);
                poke5.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[4].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[4].name);
                pokeName5.text = "" + GameController.playerPokemon[4].name + ", Level " + GameController.playerPokemon[4].level + "\nHP: " + GameController.playerPokemon[4].current_hp + "/" + GameController.playerPokemon[4].max_hp;
            }
            else pokeCanvas5.SetActive(false);
            if (GameController.playerPokemon[5] != null)
            {
                pokeCanvas6.SetActive(true);
                poke6.sprite = Resources.Load<Sprite>(path + GameController.playerPokemon[5].dexnum.ToString().PadLeft(3, '0') + GameController.playerPokemon[5].name);
                pokeName6.text = "" + GameController.playerPokemon[5].name + ", Level " + GameController.playerPokemon[5].level + "\nHP: " + GameController.playerPokemon[5].current_hp + "/" + GameController.playerPokemon[5].max_hp;
            }
            else pokeCanvas6.SetActive(false);
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

            money.text = "Money: " + "?" + GameController.player.money;
            playerName.text = "Trainer Name: " + GameController.player.name;
            time.text = "Total play time: " + ((int) (UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60 / 60) + " Hours, " + ((int)(UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60) + " Minutes";
        }

        public void ProgressMenu()
        {
            progressBadges.SetActive(true);
            otherProgress.SetActive(true);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(false);
            //pokePosition.SetActive(false);
        }

        public void PokeMenu()
        {
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            pauseMenuUI.SetActive(false);
            pokeUI.SetActive(true);
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

            if (PlayerPrefs.GetInt("BadgeRock") == 1) GameController.badges_completed.Add("Rock", 1);
            if (PlayerPrefs.GetInt("BadgeWater") == 1) GameController.badges_completed.Add("Water", 1);
            if (PlayerPrefs.GetInt("BadgeElectric") == 1) GameController.badges_completed.Add("Electric", 1);
            if (PlayerPrefs.GetInt("BadgeGrass") == 1) GameController.badges_completed.Add("Grass", 1);
            if (PlayerPrefs.GetInt("BadgePoison") == 1) GameController.badges_completed.Add("Poison", 1);
            if (PlayerPrefs.GetInt("BadgePsychic") == 1) GameController.badges_completed.Add("Psychic", 1);
            if (PlayerPrefs.GetInt("BadgeFire") == 1) GameController.badges_completed.Add("Fire", 1);
            if (PlayerPrefs.GetInt("BadgeGround") == 1) GameController.badges_completed.Add("Ground", 1);

            GameController.update_level_cap();

            SceneManager.LoadSceneAsync(GameController.scene);
            Resume();
            pauseMenuUI.SetActive(true);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
    }
}