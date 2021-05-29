using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public enum GameState { Roam, Dialog, Battle, TrainerEncounter }

    public class GameController : MonoBehaviour
    {
        public static GameState state = GameState.Roam;

        //player data
        public static Pokemon[] playerPokemon = new Pokemon[6];
        public static PlayerData player = new PlayerData();
        public static bool inCombat = false;
        public static bool starterChosen = false;

        //public GameObject player;
        public static Dictionary<string, int> badges_completed = new Dictionary<string, int>() {/*{"Ground",1 }*/ };

        public static string location = "Pallet Town";
        public static string prevLocation = "Pallet Town";
        public static string scene = "Pallet Town";

        //Game Level Cap
        public static int level_cap = 10;

        //combat data
        public static Pokemon[] opponentPokemon = new Pokemon[6];

        public static string endText = "Good fight!";
        public static bool isCatchable = true;
        public static int catchRate = 0;
        public static string opponentName;
        public static string opponentType;
        public static int winMoney;

        //overworld data
        public GameObject overworldCam;

        //GameObject overworldCam;
        public GameObject eventSystem;

        public Camera test;

        //triggers
        public static bool triggerCombat = false;
        public static bool inDialog = false;
        public static bool endCombat = false;
        public static bool startCombatMusic = false;
        public static bool endCombatMusic = false;
        public static string music = "Pallet Town";
        public static string soundFX = null;

        //rnd
        public static System.Random _rnd = new System.Random();

        private static GameController Instance = null;

        // Start is called before the first frame update
        private void Awake()
        {
            // If there is not already an instance of SoundManager, set it to this.
            if (Instance == null)
            {
                Instance = this;
            }
            //If an instance already exists, destroy whatever this object is to enforce the singleton.
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            //overworldCam = GameObject.Find("Main Camera");
            //overworldCam = GameObject.FindGameObjectWithTag("OverworldCam");

            DontDestroyOnLoad(transform.gameObject);


            //COMMENT THIS OUT FOR TESTING
            /*Pokemon.all_base_stats = BattleSystem.load_CSV("BASE_STATS");
            Moves.all_moves = BattleSystem.load_CSV("MOVES");
            Type.type_attack = BattleSystem.load_CSV("TYPE_ATTACK");
            Type.type_defend = BattleSystem.load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = BattleSystem.load_CSV("LEARNSET");
            Pokedex.all_pokedex = BattleSystem.load_CSV("POKEMON");
            Route.all_routes = BattleSystem.load_CSV("ROUTES");
            Trainer.all_trainers = BattleSystem.load_CSV("TRAINERS");
            TMSet.all_TMSet = BattleSystem.load_CSV("TMSET");
            Type.load_type();
            Moves.load_moves();
            update_level_cap();
            GameController.playerPokemon[0] = new Pokemon(25, 20, "Thunder Wave", "Quick Attack", "Thunder Shock", "Growl");
            */
            //Debug.Log("CSV's have been loaded");
        }

        private void Start()
        {
            DialogController.Instance.OnShowDialog += () =>
            {
                if (state == GameState.Roam)
                {
                    state = GameState.Dialog;
                    //Debug.Log("Show Dialog");
                }
            };

            DialogController.Instance.OnCloseDialog += () =>
            {
                if (state == GameState.Dialog)
                {
                    //Debug.Log("Close Dialog");
                    state = GameState.Roam;
                }
            };
        }

        private void Update()
        {
            if (state == GameState.Dialog)
            {
                //Debug.Log("In dialog");
                DialogController.Instance.HandleUpdate();
            }
            /*if (state == GameState.Roam)
                PlayerMovement.Instance.HandleUpdate();*/
            if (state == GameState.Roam && inDialog == true)
                StartCoroutine(EndDialog());

            if (triggerCombat == true)
            {
                //overworldCam.SetActive(false);
                //player.SetActive(false);
                inCombat = true;
                eventSystem.SetActive(false);
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);

                if (overworldCam != null)
                    overworldCam.SetActive(false);
                triggerCombat = false;
                //music = "Battle Wild Begin";
                //startCombatMusic = true;
            }
            if (endCombat == true)
            {
                SceneManager.UnloadSceneAsync("BattleScene");
                //Debug.Log("Level Cap: " + level_cap);
                //Debug.Log("Badge Count: " + badges_completed.Count);
                inCombat = false;
                eventSystem.SetActive(true);
                overworldCam.SetActive(true);

                if (!isCatchable)
                {
                    var Trainer = GameObject.Find(opponentName).GetComponent<TrainerController>();
                    Trainer.isBeaten = true;
                    Trainer.reward();
                }
                music = location;
                endCombat = false;
            }
        }

        private IEnumerator EndDialog()
        {
            yield return new WaitForSeconds(1.0f);
            inDialog = false;
        }

        public static void update_level_cap()
        {
            level_cap = ((badges_completed.Count * 10) + 10);
        }
    }
}