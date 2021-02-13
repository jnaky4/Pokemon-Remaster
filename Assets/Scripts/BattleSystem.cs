using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RUNAWAY, CAUGHTPOKEMON }

namespace Pokemon
{
    public class BattleSystem : MonoBehaviour
    {
        /**********************************************************************************************************************************************
         * VARIABLES
         **********************************************************************************************************************************************/
        public static Pokemon[] playerPokemon = new Pokemon[6];

        public static Pokemon[] opponentPokemon = new Pokemon[6];
        public int activePokemon = 0;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public PlayerBattle player;
        public SpriteRenderer playerSprite;
        public SpriteRenderer enemySprite;

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

        public Button pokemon1Button;
        public Button pokemon2Button;
        public Button pokemon3Button;
        public Button pokemon4Button;
        public Button pokemon5Button;
        public Button pokemon6Button;

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
        public GameObject poke1;
        public GameObject poke2;
        public GameObject poke3;
        public GameObject poke4;
        public GameObject poke5;
        public GameObject poke6;


        /**********************************************************************************************************************************************
         * FUNCTIONS
         **********************************************************************************************************************************************/
        void Start()
        {
            //COMMENT THIS SHIT OUT LEVI

            /*Pokemon.all_base_stats = load_CSV("BASE_STATS");
            Moves.all_moves = load_CSV("MOVES");
            Type.type_attack = load_CSV("TYPE_ATTACK");
            Type.type_defend = load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = load_CSV("LEARNSET");
            Pokedex.all_pokedex = load_CSV("POKEMON");
            Type.load_type();
            Moves.load_moves();
            */

            //END COMMENTING OUT LEVI

            state = BattleState.START;
            pokeMenuUI.SetActive(false);
            attackMenuUI.SetActive(false);
            ballsMenuUI.SetActive(false);
            SetDownButtons();

            StartCoroutine(SetupBattle());
        }

        public static List<Dictionary<string, object>> load_CSV(string name)
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
            for (var i = 0; i < 6; i++)
            {
                playerPokemon[i] = null;
            }

            GameObject playerGO = Instantiate(playerPrefab);
            playerUnit = playerGO.GetComponent<Unit>();
            playerPokemon[0] = new Pokemon(6, 30, "Wing Attack", "Flamethrower", "Earthquake", "Slash");
            playerPokemon[1] = new Pokemon(9, 35, "Water Gun", "Hydro Pump", "Blizzard", "Slash");

            playerUnit.pokemon = playerPokemon[0];

            PlayerBattle playerTemp = new PlayerBattle();

            playerTemp.myName = "Red";
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

            if (playerPokemon[1] == null) poke2.SetActive(false);
            if (playerPokemon[2] == null) poke3.SetActive(false);
            if (playerPokemon[3] == null) poke4.SetActive(false);
            if (playerPokemon[4] == null) poke5.SetActive(false);
            if (playerPokemon[5] == null) poke6.SetActive(false);

            GameObject enemyGO = Instantiate(enemyPrefab);
            enemyUnit = enemyGO.GetComponent<Unit>();
            //opponentPokemon[0] = new Pokemon(3, 5, "Wing Attack", "Flamethrower", "Earthquake", "Slash"); //COMMENT OUT THIS ONE LINE LEVI
            enemyUnit.pokemon = opponentPokemon[0];


            enemyUnit.catchRate = 3;

            dialogueText.text = "A wild " + enemyUnit.pokemon.name + " appears!";

            playerHUD.SetHUD(playerUnit, true, player, playerPokemon);
            enemyHUD.SetHUD(enemyUnit, false, player, playerPokemon);
            SetSprite(playerUnit, playerSprite);
            SetSprite(enemyUnit, enemySprite);


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
                dialogueText.text = enemyUnit.pokemon.name + " broke free!";
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
        IEnumerator SwitchPokemon(int num)
        {
            if (activePokemon == num)
            {
                dialogueText.text = "You already have that pokemon out!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }
            dialogueText.text = "Get out of there, " + playerUnit.pokemon.name + "!";
            yield return new WaitForSeconds(2);
            playerUnit.pokemon = playerPokemon[num];
            activePokemon = num;

            playerHUD.SetActivePokemon(playerPokemon, num, playerUnit);
            SetSprite(playerUnit, playerSprite);
            dialogueText.text = "Go, " + playerUnit.pokemon.name + "!";
            yield return new WaitForSeconds(2);


            state = BattleState.ENEMYTURN;
            EnemyTurn();
            yield break;
        }
        void SetSprite(Unit unit, SpriteRenderer sprite)
        {
            Texture2D SpriteTexture = new Texture2D(2, 2);
            byte[] fileData;
            fileData = File.ReadAllBytes(unit.pokemon.image1);
            SpriteTexture.LoadImage(fileData);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));

            sprite.sprite = NewSprite;
        }

        IEnumerator EnemyAttack()
        {
            enemyUnit.SetDamage(playerUnit.pokemon.temp_defense, 10);
            dialogueText.text = enemyUnit.pokemon.name + " attacks!";
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.pokemon.temp_hp, playerUnit);
            yield return new WaitForSeconds(2);

            dialogueText.text = playerUnit.pokemon.name + " took " + enemyUnit.damage + " points of damage!";

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
            SetDownButtons();
            if (state == BattleState.WON)
            {
                dialogueText.text = player.myName + " won!";
            }
            else if (state == BattleState.LOST)
            {
                dialogueText.text = player.myName + " lost! You blacked out!";
            }
            else if (state == BattleState.RUNAWAY)
            {
                dialogueText.text = "Got away safely...";
            }
            else if (state == BattleState.CAUGHTPOKEMON)
            {
                dialogueText.text = "You caught a " + enemyUnit.pokemon.name + "!";
            }
            else
            {
                dialogueText.text = "What the fuck just happened";
            }
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Pallet Town");
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



        public void OnPokemon0()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(0));
        }
        public void OnPokemon1()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(1));
        }
        public void OnPokemon2()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(2));
        }
        public void OnPokemon3()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(3));
        }
        public void OnPokemon4()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(4));
        }
        public void OnPokemon5()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(5));
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