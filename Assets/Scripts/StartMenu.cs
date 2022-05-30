using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class StartMenu : MonoBehaviour
    {
        public bool develop_mode = true;

        public Vector2 startingPosition;
        public VectorValue playerPosition;
        public PlayerData player;
        public String location;
        public String scene;
        public bool starterChosen;
        public Vector2 initialValue;
        public Pokemon[] playerPokemon;
        
        public Dictionary<string, int> badges_completed = new Dictionary<string, int>() { };
        //public Dictionary<string, int> badges_completed = new Dictionary<string, int>() { { "Boulder", 1 }, { "Cascade", 1 }, { "Thunder", 1 }, { "Rainbow", 1 }, { "Soul", 1 }, { "Marsh", 1 }, { "Volcano", 1 }, { "Earth", 1 }, { "Elite Four", 1 } };

        private void Start()
        {
            //Debug.Log("executing Start");
            Pokemon.all_base_stats = BattleSystem.load_CSV("BASE_STATS_GEN2");
            Moves.all_moves = BattleSystem.load_CSV("MOVES_GEN2");
            Type.type_attack = BattleSystem.load_CSV("TYPE_ATTACK_GEN2");
            Learnset.all_learnset = BattleSystem.load_CSV("LEARNSET_GEN2");
            Pokedex.all_pokedex = BattleSystem.load_CSV("POKEMON_GEN2");
            Trainer.all_trainers = BattleSystem.load_CSV("TRAINERS");
            //TMSet.all_TMSet = BattleSystem.load_CSV("TMSET");
            Items.all_Items = BattleSystem.load_CSV("ITEMS");

            Type.load_type();
            Moves.load_moves();
            Items.load_items_to_dict();
            //Debug.Log("finished importing csvs in start");
            PlayerLoad();
        }

        public void NewGame()
        {

            if (develop_mode)
            {
                Debug.Log("Development Mode Started");
                GameController.starterChosen = true;
                /*GameController.playerPokemon[0] = new Pokemon(155, 5, "Tackle");*/
                //Tauros
                GameController.playerPokemon[0] = new Pokemon(128, 100, "Body Slam", "Earthquake", "Blizzard", "Hyper Beam");
                GameController.playerPokemon[0].statuses.Add(Status.get_status("Paralysis"));
                //Exeggutor
                GameController.playerPokemon[1] = new Pokemon(103, 100, "Sleep Powder", "Mega Drain", "Explosion", "Psychic");
                //Rhydon
                GameController.playerPokemon[2] = new Pokemon(112, 100, "Rock Slide", "Earthquake", "Body Slam"/*, "Substitute"*/);
                //Chansey
                GameController.playerPokemon[3] = new Pokemon(113, 100, "Thunder Wave", "Thunderbolt", "Ice Beam", "Soft-Boiled");
                //Starmie
                GameController.playerPokemon[4] = new Pokemon(121, 100, "Surf", "Blizzard", "Recover", "Thunderbolt");
                //Zapdos
                GameController.playerPokemon[5] = new Pokemon(145, 100, "Drill Peck", "Toxic", "Thunder Wave", "Thunderbolt");


                badges_completed = new Dictionary<string, int>{ { "Boulder", 1 }, { "Cascade", 1 }, { "Thunder", 1 }, { "Rainbow", 1 }, { "Soul", 1 }, { "Marsh", 1 }, { "Volcano", 1 }, { "Earth", 1 }, { "Elite Four", 1 } };
                
                /*GameController.playerPokemon[1] = new Pokemon(25, 20, "Thunder Wave", "Quick Attack", "Thunder Shock", "Growl");
                GameController.playerPokemon[2] = new Pokemon(60, 19, "Water Gun", "Bubble", "Splash", "Crabhammer", 7900);
                GameController.playerPokemon[3] = new Pokemon(69, 17, "Leech Seed", "Vine Whip", "Poison Powder", "Razor Leaf", 5500);
                GameController.playerPokemon[4] = new Pokemon(12, 20, "Sleep Powder", "Poison Powder", "Stun Spore", "Psychic");
                */
            }
            else
            {
                int level = 5;
                int dex = 25;
                string move1 = "Tackle", move2 = "Tail Whip";
                GameController.starterChosen = false;
                GameController.playerPokemon[0] = new Pokemon(dex, level, move1, move2);
            }


            //Debug.Log("new game starts");
            GameController.player.pokeBalls = 99;
            GameController.player.greatBalls = 0;
            GameController.player.ultraBalls = 0;
            GameController.player.masterBalls = 1;

            GameController.player.displayPokeBalls = true;
            GameController.player.displayGreatBalls = true;
            GameController.player.displayUltraBalls = true;
            GameController.player.displayMasterBalls = true;

            GameController.player.money = 3000;
            GameController.player.name = "Red";
            GameController.location = "Pallet Town";
            GameController.scene = "Pallet Town";


            
            

            /*
            playerPosition.initialValue.x = 3.5f;
            playerPosition.initialValue.y = 2.8f;*/
            playerPosition.initialValue.x = startingPosition.x;
            playerPosition.initialValue.y = startingPosition.y;


            SceneManager.LoadSceneAsync(GameController.scene);
        }

        public void DoNothing()
        {

        }

        public void PlayerLoad()
        {
            if (PlayerPrefs.HasKey("X"))
            {
                player = (new GameObject("PlayerData")).AddComponent<PlayerData>();
                playerPokemon = new Pokemon[6];
                //Debug.Log("starting continue loads");
                player.pokeBalls = PlayerPrefs.GetInt("Pokeball");
                player.greatBalls = PlayerPrefs.GetInt("Greatball");
                player.ultraBalls = PlayerPrefs.GetInt("Ultraball");
                player.masterBalls = PlayerPrefs.GetInt("Masterball");
                //Debug.Log("loaded balls");
                player.displayPokeBalls = PlayerPrefs.GetString("DisplayPoke").Equals("True");
                player.displayGreatBalls = PlayerPrefs.GetString("DisplayGreat").Equals("True");
                player.displayUltraBalls = PlayerPrefs.GetString("DisplayUltra").Equals("True");
                player.displayMasterBalls = PlayerPrefs.GetString("DisplayMaster").Equals("True");
                //Debug.Log("loaded balls display");
                player.money = PlayerPrefs.GetInt("Money");
                player.name = PlayerPrefs.GetString("Name");
                location = PlayerPrefs.GetString("Location");
                scene = PlayerPrefs.GetString("Scene");

                starterChosen = Convert.ToBoolean(PlayerPrefs.GetInt("HasStarter"));
                player.starter = PlayerPrefs.GetInt("Starter");

                initialValue.x = PlayerPrefs.GetFloat("X");
                initialValue.y = PlayerPrefs.GetFloat("Y");
                //Debug.Log("loaded game cunt");

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

                        playerPokemon[i] = new Pokemon(dex, level, move1, move2, move3, move4, exp, hp);
                    }
                }
                //Debug.Log("loaded pokemon");
                if (PlayerPrefs.GetInt("BadgeRock") == 1) badges_completed.Add("Rock", 1);
                if (PlayerPrefs.GetInt("BadgeWater") == 1) badges_completed.Add("Water", 1);
                if (PlayerPrefs.GetInt("BadgeElectric") == 1) badges_completed.Add("Electric", 1);
                if (PlayerPrefs.GetInt("BadgeGrass") == 1) badges_completed.Add("Grass", 1);
                if (PlayerPrefs.GetInt("BadgePoison") == 1) badges_completed.Add("Poison", 1);
                if (PlayerPrefs.GetInt("BadgePsychic") == 1) badges_completed.Add("Psychic", 1);
                if (PlayerPrefs.GetInt("BadgeFire") == 1) badges_completed.Add("Fire", 1);
                if (PlayerPrefs.GetInt("BadgeGround") == 1) badges_completed.Add("Ground", 1);

                //Debug.Log("loaded badges");
                //Debug.Log("loaded scene");
            }
            else
            {
                //Debug.Log("why no key in prefs??");
            }


        }
        public void Continue()
        {
            if (PlayerPrefs.HasKey("X"))
            {
                GameController.player = player;
                GameController.location = location;
                GameController.scene = scene;
                GameController.starterChosen = starterChosen;
                playerPosition.initialValue.x = initialValue.x;
                playerPosition.initialValue.y = initialValue.y;
                GameController.playerPokemon = playerPokemon;
                GameController.badges_completed = badges_completed;
                GameController.update_level_cap();
                SceneManager.LoadSceneAsync(GameController.scene);
            }
            else
            {
                //Debug.Log("why no key in prefs??");
            }
        }
    }
}