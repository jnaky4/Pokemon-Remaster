using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class GameController : MonoBehaviour
    {
        //player data
        public static Pokemon[] playerPokemon = new Pokemon[6];
        public static bool inCombat = false;
        public GameObject player;
        public static Dictionary<string, int> badges_completed = new Dictionary<string, int>() { };

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
            Type.load_type();
            Moves.load_moves();

            //Debug.Log("CSV's have been loaded");

            playerPokemon[0] = new Pokemon(4, 5, "Wing Attack", "Flamethrower", "Earthquake", "Slash");
            playerPokemon[1] = new Pokemon(7, 5, "Water Gun", "Hydro Pump", "Blizzard", "Slash");
            playerPokemon[2] = new Pokemon(1, 5, "Leech Seed", "Vine Whip", "Growl", "Slash");
        }

        private void Update()
        {
            if (triggerCombat == true)
            {
                //overworldCam.SetActive(false);
                player.SetActive(false);
                eventSystem.SetActive(false);
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                overworldCam.SetActive(false);
                triggerCombat = false;
            }
            if (endCombat == true)
            {
                SceneManager.UnloadSceneAsync("BattleScene");
                eventSystem.SetActive(true);
                overworldCam.SetActive(true);
                player.SetActive(true);
                endCombat = false;
            }
        }

    }
}
