using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RUNAWAY, CAUGHTPOKEMON }

namespace Pokemon
{
    public class BattleSystem : MonoBehaviour
    {
        /**********************************************************************************************************************************************
         * VARIABLES
         **********************************************************************************************************************************************/
        
        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public PlayerBattle player;

        //IDK what these are but they need to stay

        Unit playerUnit;
        Unit enemyUnit;
        //The pokemon used in the fight

        public Button attackButton;
        public Button runAwayButton;
        public Button pokemonButton;
        public Button ballsButton;
        //Our button used to attack

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;
        //The HUDs (the shit that shows our current hp and stuff like that

        public Text dialogueText;
        //The dialogue text to let us know what is happening

        public BattleState state;
        //The current state of the battle

        public static bool poekmonMenuOpen = false;
        public GameObject pokeMenuUI;

        public static bool attackMenuOpen = false;
        public GameObject attackMenuUI;
        public Button attack1Button;
        public Button attack2Button;
        public Button attack3Button;
        public Button attack4Button;

        public static bool ballsMenuOpen = false;
        public GameObject ballsMenuUI;
        public Button balls1Button;
        public Button balls2Button;
        public Button balls3Button;
        public Button balls4Button;
        public GameObject b1GO;
        public GameObject b2GO;
        public GameObject b3GO;
        public GameObject b4GO;
        public GameObject b5GO;
        public GameObject b6GO;
        public GameObject b7GO;
        public GameObject b8GO;

        public string path = "..\\..\\CSV";
        public string moves = "\\MOVES.csv";

        /**********************************************************************************************************************************************
         * FUNCTIONS
         **********************************************************************************************************************************************/
        void Start()
        {
            Pokemon.all_base_stats = load_CSV("BASE_STATS");
            Moves.all_moves = load_CSV("MOVES");
            Type.type_attack = load_CSV("TYPE_ATTACK");
            Type.type_defend = load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = load_CSV("LEARNSET");
            Pokedex.all_pokedex = load_CSV("POKEMON");
            Type.load_type();
            Moves.load_moves();

            //dexnum and lvl
            Pokemon pokemon_variable = new Pokemon(6, 30);
            //devnum, lvl, 4 moves in string
            Pokemon Charizard = new Pokemon(6, 54, "Wing Attack", "Flamethrower", "Earthquake", "Slash");
            Pokemon Metapod = new Pokemon(11, 69, "Harden");

            int base_attack = pokemon_variable.base_attack;
            int current_attack = pokemon_variable.current_attack;
            string name = pokemon_variable.name;

            List<Learnset> pokemon_learnset = pokemon_variable.learnset;

            Type fire = pokemon_variable.type1;
            double multiplier = fire.attack_bug;
            double multiplier2 = fire.defend_bug;

            //size 4 list of 4 current moves added
            Moves move = Charizard.currentMoves[0];
            int base_power = Charizard.currentMoves[0].base_power;

            state = BattleState.START;
            pokeMenuUI.SetActive(false);
            attackMenuUI.SetActive(false);
            ballsMenuUI.SetActive(false);
            SetDownButtons();

            StartCoroutine(SetupBattle());
        }

        List<Dictionary<string, object>> load_CSV(string name)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(name);
            return data;
        }


        public void print_pokemon()
        {

            for (int i = 1; i < 152; i++)
            {
                Pokemon TestPokemon = new Pokemon(i, 50, "Flamethrower", "Earthquake", "Wing Attack", "Slash");
                /*                Debug.Log("Name " + TestPokemon.name);
                                Debug.Log("Base Attack " + TestPokemon.base_attack + " Current Attack " + TestPokemon.current_attack);
                                Debug.Log("Type1: " + TestPokemon.type1.type);
                                if (TestPokemon.type2 != null)
                                {
                                    Debug.Log("Type2: " + TestPokemon.type2.type);
                                }*/

                foreach (Learnset learned in TestPokemon.learnset)
                {
                    Debug.Log(learned.ToString());
                    Debug.Log("PP " + learned.get_move().pp);
                    Debug.Log("TYPE " + learned.get_move().move_type.type);

                }

            }
        }
        public void print_moves()
        {
            foreach (KeyValuePair<string, Moves> move in Moves.move_dictionary)
            {
                Debug.Log(move.Key);
            }
        }

        IEnumerator SetupBattle()
        {
            //GameObject pokeMenu = Instantiate(pokemonMenuUI);

            GameObject playerGO = Instantiate(playerPrefab);
            playerUnit = playerGO.GetComponent<Unit>();
            playerUnit.pokemon = new Pokemon(6, 30, "Wing Attack", "Flamethrower", "Earthquake", "Slash");

            PlayerBattle playerTemp = new PlayerBattle();

            playerTemp.name = "Red";
            playerTemp.pokeBalls = true;
            playerTemp.numPokeBalls = 11;
            playerTemp.greatBalls = true;
            playerTemp.numGreatBalls = 0;
            playerTemp.ultraBalls = true;
            playerTemp.numUltraBalls = 0;
            playerTemp.masterBalls = true;
            playerTemp.numMasterBalls = 10;

            player = playerTemp;

            if (!player.pokeBalls) b1GO.SetActive(false);
            if (!player.greatBalls) b2GO.SetActive(false);
            if (!player.ultraBalls) b3GO.SetActive(false);
            if (!player.masterBalls) b4GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[0].move, "default") == 0) b5GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[1].move, "default") == 0) b6GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[2].move, "default") == 0) b7GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[3].move, "default") == 0) b8GO.SetActive(false);

            GameObject enemyGO = Instantiate(enemyPrefab);
            enemyUnit = enemyGO.GetComponent<Unit>();
            enemyUnit.pokemon = new Pokemon(3, 45, "Wing Attack", "Flamethrower", "Earthquake", "Slash");
            enemyUnit.catchRate = 255;

            dialogueText.text = "A wild " + enemyUnit.pokemon.name + " appears!";

            playerHUD.SetHUD(playerUnit, true, player);
            enemyHUD.SetHUD(enemyUnit, false, player);


            yield return new WaitForSeconds(2);

            if (enemyUnit.pokemon.temp_speed > playerUnit.pokemon.temp_speed)
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            }
            else if (enemyUnit.pokemon.temp_speed < playerUnit.pokemon.temp_speed)
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
            else
            {
                System.Random rnd = new System.Random();
                int num = rnd.Next(1, 3);
                if (num == 1)
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    EnemyTurn();
                }
            }


        }
        //This function sets up the battle state for us including the UI

        IEnumerator PlayerAttack(Moves attack)
        {
            playerUnit.SetDamage(enemyUnit.pokemon.temp_defense, attack.base_power);

            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.pokemon.temp_hp);
            dialogueText.text = playerUnit.pokemon.name + " used " + attack.move + "!";
            yield return new WaitForSeconds(2);
            dialogueText.text = enemyUnit.pokemon.name + " took " + playerUnit.damage + " amount of damage...";
            yield return new WaitForSeconds(2);
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            }
        }

        IEnumerator CatchPokemon(int typeOfPokeball)
        {
            if (!enemyUnit.canBeCaught)
            {
                dialogueText.text = "You can't catch other trainer's Pokemon!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }
            System.Random rnd = new System.Random();

            int randomNumber = 0, catchRate = 1, randomNumber2, f, numShakes = 0;
            if (typeOfPokeball == 1)
            {
                if (player.numPokeBalls == 0)
                {
                    dialogueText.text = "You don't have enough Poke Balls!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
                player.numPokeBalls--;
                randomNumber = rnd.Next(256);
                catchRate = 6;
                numShakes = 255;
            }
            if (typeOfPokeball == 2)
            {
                if (player.numGreatBalls == 0)
                {
                    dialogueText.text = "You don't have enough Great Balls!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
                player.numGreatBalls--;
                randomNumber = rnd.Next(201);
                catchRate = 8;
                numShakes = 200;
            }
            if (typeOfPokeball == 3)
            {
                if (player.numUltraBalls == 0)
                {
                    dialogueText.text = "You don't have enough Ultra Balls!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
                player.numUltraBalls--;
                randomNumber = rnd.Next(151);
                catchRate = 12;
                numShakes = 150;
            }
            if (typeOfPokeball == 4)
            {
                if (player.numMasterBalls == 0)
                {
                    dialogueText.text = "You don't have enough Master Balls!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
                player.numMasterBalls--;
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }

            //if (/*pokemon is asleep or frozen and n is < 25 */) { }
            //if (/*pokemon is paralyzed burned poisoned and n is < 12 */) { }
            if (randomNumber < enemyUnit.catchRate)
            {
                dialogueText.text = enemyUnit.name + " broke free!";
                yield return new WaitForSeconds(2);
                EnemyTurn();
                yield break;
            }
            randomNumber2 = rnd.Next(256);
            f = (enemyUnit.pokemon.current_hp * 255 * 4);
            f = f / (enemyUnit.pokemon.temp_hp * catchRate);
            if (f >= randomNumber2)
            {
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }
            else
            {
                int d = (enemyUnit.catchRate * 100) / numShakes;
                dialogueText.text = enemyUnit.name + " broke free!";
                yield return new WaitForSeconds(2);
                EnemyTurn();
                yield break;
            }

        }

        IEnumerator EnemyAttack()
        {
            enemyUnit.SetDamage(playerUnit.pokemon.temp_defense, 10);
            dialogueText.text = enemyUnit.name + " attacks!";
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.pokemon.temp_hp, playerUnit);
            yield return new WaitForSeconds(2);

            dialogueText.text = playerUnit.name + " took " + enemyUnit.damage + " points of damage!";

            yield return new WaitForSeconds(2);

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

        IEnumerator EndBattle()
        {
            if (state == BattleState.WON)
            {
                dialogueText.text = player.name + " won!";
            }
            else if (state == BattleState.LOST)
            {
                dialogueText.text = player.name + " lost! You blacked out!";
            }
            else if (state == BattleState.RUNAWAY)
            {
                dialogueText.text = "Got away safely...";
            }
            else if (state == BattleState.CAUGHTPOKEMON)
            {
                dialogueText.text = "You caught a " + enemyUnit.name + "!";
            }
            else
            {
                dialogueText.text = "What the fuck just happened";
            }
            yield return new WaitForSeconds(2);
        }

        void PlayerTurn()
        {
            dialogueText.text = "Choose an action";
            SetUpButtons();
        }
        void EnemyTurn()
        {
            SetDownButtons();
            StartCoroutine(EnemyAttack());
        }

        public void OnAttackButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            if (attackMenuOpen) CloseMovesMenu();
            else
            {
                ClosePokemonMenu();
                CloseBallsMenu();
                OpenMovesMenu();
            }
            //SetDownButtons();

            //StartCoroutine(PlayerAttack());
        }

        public void OnRunAwayButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            attackMenuUI.SetActive(false);
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            RunAway();
        }

        public void OnPokemonButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            attackMenuUI.SetActive(false);
            OpenPokemonMenu();
            SetDownButtons();
        }

        public void OnBallsButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            attackMenuUI.SetActive(false);
            if (ballsMenuOpen) CloseBallsMenu();
            else
            {
                ClosePokemonMenu();
                CloseMovesMenu();
                OpenBallsMenu();
            }
        }

        public void RunAway()
        {
            state = BattleState.RUNAWAY;
            CloseBallsMenu();
            CloseMovesMenu();
            ClosePokemonMenu();
            StartCoroutine(EndBattle());
        }

        public void SetUpButtons()
        {
            attackButton.interactable = true;
            runAwayButton.interactable = true;
            pokemonButton.interactable = true;
            ballsButton.interactable = true;
        }

        public void SetDownButtons()
        {
            attackButton.interactable = false;
            runAwayButton.interactable = false;
            pokemonButton.interactable = false;
            ballsButton.interactable = false;
        }

        public void OpenPokemonMenu()
        {
            pokeMenuUI.SetActive(true);
            Time.timeScale = 0f;
            poekmonMenuOpen = true;
        }
        public void ClosePokemonMenu()
        {
            pokeMenuUI.SetActive(false);
            Time.timeScale = 1f;
            poekmonMenuOpen = false;
            SetUpButtons();
        }
        public void OpenMovesMenu()
        {
            attackMenuUI.SetActive(true);
            attackMenuOpen = true;
        }
        public void CloseMovesMenu()
        {
            attackMenuUI.SetActive(false);
            attackMenuOpen = false;
        }
        public void OpenBallsMenu()
        {
            ballsMenuUI.SetActive(true);
            ballsMenuOpen = true;
        }
        public void CloseBallsMenu()
        {
            ballsMenuUI.SetActive(false);
            ballsMenuOpen = false;
        }

        public void Attack1()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[0]));
        }
        public void Attack2()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[1]));
        }
        public void Attack3()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[2]));
        }
        public void Attack4()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[3]));
        }
        public void PokeBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(1));
        }
        public void GreatBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(2));
        }
        public void UltraBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(3));
        }
        public void MasterBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(4));
        }
    }
}