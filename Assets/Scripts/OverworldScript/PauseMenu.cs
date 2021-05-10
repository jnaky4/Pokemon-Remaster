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

        public Text playerName;
        public Text time;
        public Text money;

        private void Start()
        {
            pauseMenuUI.SetActive(false);
            progress.text = "Progress " + GameController.player.name + " Badges/money";
        }

        // Update is called once per frame
        void Update()
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

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            progressBadges.SetActive(false);
            otherProgress.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            SetImages();
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

            money.text = "Money: " + "¥" + GameController.player.money;
            playerName.text = "Trainer Name: " + GameController.player.name;
            time.text = "Total play time: " + ((int) (UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60 / 60) + " Hours, " + ((int)(UnityEngine.Time.realtimeSinceStartup + GameController.player.time) / 60) + " Minutes";
        }

        public void ProgressMenu()
        {
            progressBadges.SetActive(true);
            otherProgress.SetActive(true);
            pauseMenuUI.SetActive(false);
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