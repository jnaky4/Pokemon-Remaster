using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class StartMenu : MonoBehaviour
    {
        public VectorValue playerPosition;
        private void Start()
        {
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
        }

        public void NewGame()
        {



            GameController.player.pokeBalls = 10;
            GameController.player.greatBalls = 0;
            GameController.player.ultraBalls = 0;
            GameController.player.masterBalls = 0;

            GameController.player.displayPokeBalls = true;
            GameController.player.displayGreatBalls = true;
            GameController.player.displayUltraBalls = true;
            GameController.player.displayMasterBalls = true;



            GameController.player.money = 3000;
            GameController.player.name = "Purple";
            GameController.location = "Pallet Town";
            GameController.scene = "Pallet Town";



            playerPosition.initialValue.x = 3.5f;
            playerPosition.initialValue.y = 2.8f;

            int level = 5;
            int dex = 25;
            string move1 = "Tackle", move2 = "Growl";

            GameController.playerPokemon[0] = new Pokemon(dex, level, move1, move2);
/*            GameController.playerPokemon[1] = new Pokemon(25, 20, "Thunder Wave", "Quick Attack", "Thunder Shock", "Growl");
            GameController.playerPokemon[2] = new Pokemon(60, 19, "Water Gun", "Bubble", "Splash", "Crabhammer", 7900);
            GameController.playerPokemon[3] = new Pokemon(69, 17, "Leech Seed", "Vine Whip", "Poison Powder", "Razor Leaf", 5500);
            GameController.playerPokemon[4] = new Pokemon(12, 20, "Sleep Powder", "Poison Powder", "Stun Spore", "Psychic");*/


            SceneManager.LoadSceneAsync(GameController.scene);
        }
        public void Continue()
        {
            if (PlayerPrefs.HasKey("X"))
            {
                GameController.player.pokeBalls = PlayerPrefs.GetInt("Pokeball");
                GameController.player.greatBalls = PlayerPrefs.GetInt("Greatball");
                GameController.player.ultraBalls = PlayerPrefs.GetInt("Ultraball");
                GameController.player.masterBalls = PlayerPrefs.GetInt("Masterball");

                GameController.player.displayPokeBalls = PlayerPrefs.GetString("DisplayPoke").Equals("True");
                GameController.player.displayGreatBalls = PlayerPrefs.GetString("DisplayGreat").Equals("True");
                GameController.player.displayUltraBalls = PlayerPrefs.GetString("DisplayUltra").Equals("True");
                GameController.player.displayMasterBalls = PlayerPrefs.GetString("DisplayMaster").Equals("True");

                GameController.player.money = PlayerPrefs.GetInt("Money");
                GameController.player.name = PlayerPrefs.GetString("Name");
                GameController.location = PlayerPrefs.GetString("Location");
                GameController.scene = PlayerPrefs.GetString("Scene");

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
            }
        }
    }
}