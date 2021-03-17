using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public enum GameState { Roam, Dialog, Battle, TrainerEncounter}
    public class GameController : MonoBehaviour
    {
        public static GameState state = GameState.Roam;

        //player data
        public static Pokemon[] playerPokemon = new Pokemon[6];
        public static bool inCombat = false;
        public GameObject player;
        public static Dictionary<string, int> badges_completed = new Dictionary<string, int>() { };
        public static string location = "Pallet Town";

        //Game Level Cap
        public static int level_cap = 0;

        //combat data
        public static Pokemon[] opponentPokemon = new Pokemon[6];
        public static bool isCatchable = true;
        public static int catchRate = 0;

        //overworld data
        public GameObject overworldCam;
        public GameObject eventSystem;
        public Camera test;

        //triggers
        public static bool triggerCombat = false;
        public static bool endCombat = false;
        public static bool startCombatMusic = false;
        public static bool endCombatMusic = false;

        // Start is called before the first frame update
        void Awake ()
        {
            DontDestroyOnLoad(transform.gameObject);

            Pokemon.all_base_stats = BattleSystem.load_CSV("BASE_STATS");
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

            //Debug.Log("CSV's have been loaded");

            playerPokemon[0] = new Pokemon(4, 1, "Fury Attack", "Fire Spin", "Tail Whip", "Ember");
            playerPokemon[1] = new Pokemon(7, 5, "Water Gun", "Bubble", "Growl", "Slash");
            playerPokemon[2] = new Pokemon(1, 5, "Leech Seed", "Vine Whip", "Growl", "Razor Leaf");
            playerPokemon[3]  = new Pokemon(25, 5, "Thunder Wave", "Quick Attack", "Thunder Shock", "Growl");

        }

        private void Start()
        {
            DialogController.Instance.OnShowDialog += () =>
            {
                state = GameState.Dialog;
            };

            DialogController.Instance.OnCloseDialog += () =>
            {
                if (state == GameState.Dialog)
                {
                    state = GameState.Roam;
                }
            };
        }

        private void Update()
        {
            if (state == GameState.Dialog)
            {
                DialogController.Instance.HandleUpdate();
            }
            /*if (state == GameState.Roam)
                PlayerMovement.Instance.HandleUpdate();*/

            if (triggerCombat == true)
            {
                //overworldCam.SetActive(false);
                //player.SetActive(false);
                inCombat = true;
                eventSystem.SetActive(false);
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                overworldCam.SetActive(false);
                triggerCombat = false;
                startCombatMusic = true;
            }
            if (endCombat == true)
            {
                SceneManager.UnloadSceneAsync("BattleScene");
                inCombat = false;
                eventSystem.SetActive(true);
                overworldCam.SetActive(true);
                //player.SetActive(true);
                endCombatMusic = true;
                endCombat = false;
            }
        }
        public static void update_level_cap()
        {
            level_cap = ((badges_completed.Count * 10) + 10);
        }

    }
}
