using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RUNAWAY, CAUGHTPOKEMON, CHANGEPOKEMON }

namespace Pokemon
{
    public class BattleSystem : MonoBehaviour
    {
        #region Declaration of variables

        /**********************************************************************************************************************************************
         * VARIABLES
         **********************************************************************************************************************************************/
        public int runAwayNum = 1;
        public int activePokemon = 0;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public PlayerBattle player;
        public SpriteRenderer playerSprite;
        public SpriteRenderer enemySprite;
        public SpriteRenderer playerAttackSprite;
        public SpriteRenderer enemyAttackSprite;
        public Image background;

        public Camera camera;

        //IDK what these are but they need to stay

        //[SerializeField] List<Sprite> AttackSprites;
        private List<Sprite> AttackSprites = new List<Sprite>();

        private SpriteAnimator PlayerAttackAnim;
        private SpriteAnimator EnemyAttackAnim;
        private bool playerAttack;
        private bool playerInitialAttack;
        private bool endofanimation;
        private bool enemyAttack;
        private bool enemyInitialAttack;
        private string playerMoveName;
        private string enemyMoveName;
        //all of this stuff is for animation

        private Unit playerUnit;
        private Unit enemyUnit;
        //The Pokemon used in the fight

        public Button attackButton;
        public Button runAwayButton;
        public Button pokemonButton;
        public Button ballsButton;
        //Our button used to attack

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;
        //The HUDs (the shit that shows our current hp and stuff like that

        public Text dialogueText;
        //The dialog text to let us know what is happening

        public BattleState state;
        //The current state of the battle

        public static bool poekmonMenuOpen = false;
        public GameObject pokeMenuUI;

        public static bool attackMenuOpen = false;
        public GameObject attackMenuUI;
        public static Button attack1Button;
        public static Button attack2Button;
        public static Button attack3Button;
        public static Button attack4Button;

        public static bool forgetMenuOpen = false;
        public GameObject forgetMenuUI;
        public static Button forget1Button;
        public static Button forget2Button;
        public static Button forget3Button;
        public static Button forget4Button;
        public static Button forget5Button;

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
        public GameObject ball1;
        public GameObject ball2;
        public GameObject ball3;
        public GameObject ball4;
        public GameObject move1;
        public GameObject move2;
        public GameObject move3;
        public GameObject move4;
        public GameObject poke1;
        public GameObject poke2;
        public GameObject poke3;
        public GameObject poke4;
        public GameObject poke5;
        public GameObject poke6;
        public Button backPoke;

        private bool breakOutOfDecision = false;
        private bool deadPokemon = false;
        private bool pWokeUp = false;
        private bool eWokeUp = false;
        private int playerContinuingAttack = 0;
        private int enemyContinuingAttack = 0;
        private Moves playerMoveStorage;
        private Moves enemyMoveStorage;

        private int phasePlayerSprite = 0; //0 means no phasing, 1 means phase out, 2 means phase in
        private int phaseOpponentSprite = 0;

        #endregion Declaration of variables

        /**********************************************************************************************************************************************
         * FUNCTIONS
         **********************************************************************************************************************************************/

        #region Set up battle and update functions

        /// <summary>
        /// Starts the battle.
        /// </summary>
        private void Start()
        {
            PlayerAttackAnim = new SpriteAnimator(AttackSprites, playerAttackSprite, 0.07f);
            EnemyAttackAnim = new SpriteAnimator(AttackSprites, enemyAttackSprite, 0.07f);

            state = BattleState.START;
            pokeMenuUI.SetActive(false);
            attackMenuUI.SetActive(false);
            ballsMenuUI.SetActive(false);
            forgetMenuUI.SetActive(false);
            SetDownButtons();
            StartCoroutine(SetupBattle());
            SetBackground();
        }

        //logic for whether a player or opponent's attack animation plays
        /// <summary>
        /// TO DO: Get levi to comment this code
        /// </summary>
        private void Update()
        {
            if (phasePlayerSprite == 1)
            {
                StartCoroutine(PhaseOut(playerSprite, 0.20));
                //StartCoroutine(SlideInLeft(playerSprite));
                phasePlayerSprite = 0;
            }

            if (phasePlayerSprite == 2)
            {
                StartCoroutine(PhaseIn(playerSprite, 0.20));
                phasePlayerSprite = 0;
            }
            if (phaseOpponentSprite == 1)
            {
                StartCoroutine(PhaseOut(enemySprite, 0.20));
                phaseOpponentSprite = 0;
            }
            if (phaseOpponentSprite == 2)
            {
                StartCoroutine(PhaseIn(enemySprite, 0.20));
                phaseOpponentSprite = 0;
            }

            if (playerInitialAttack == true)
            {
                GetAttackSprites(playerMoveName);
                PlayerAttackAnim.Start();
                playerInitialAttack = false;
                playerAttack = true;
            }
            if (playerAttack == true)
            {
                if (PlayerAttackAnim.CurrentFrame < PlayerAttackAnim.Frames.Count - 1)
                {
                    PlayerAttackAnim.HandleUpdate();
                }
                else
                {
                    //Debug.Log("End Animation now");
                    PlayerAttackAnim.EndAnimation();
                    playerAttack = false;
                    endofanimation = true;
                }
            }

            if (enemyInitialAttack == true)
            {
                GetAttackSprites(enemyMoveName);
                EnemyAttackAnim.Start();
                enemyInitialAttack = false;
                enemyAttack = true;
            }
            if (enemyAttack == true)
            {
                if (EnemyAttackAnim.CurrentFrame < EnemyAttackAnim.Frames.Count - 1)
                {
                    EnemyAttackAnim.HandleUpdate();
                }
                else
                {
                    //Debug.Log("End Animation now");
                    EnemyAttackAnim.EndAnimation();
                    enemyAttack = false;
                    endofanimation = true;
                }
            }
        }

        /// <summary>
        /// Setups the battle.
        /// </summary>
        /// <returns>Returns nothing. IEnumerator is so we can have text and stop the function so the player can read the text. I won't be commenting this anymore</returns>
        private IEnumerator SetupBattle()
        {
            //GameObject pokeMenu = Instantiate(pokemonMenuUI);
            player = new PlayerBattle();
            GameObject playerGO = Instantiate(playerPrefab);
            //playerGO.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, Screen.height - 20, 10));
            playerUnit = playerGO.GetComponent<Unit>();
            playerUnit.transform.localPosition = new Vector3(-50, -50, 0);
            Destroy(playerGO);

            for (int i = 0; i < GameController.playerPokemon.Count(s => s != null); i++)
            {
                if (GameController.playerPokemon[i].current_hp > 0)
                {
                    playerUnit.pokemon = GameController.playerPokemon[i];
                    break;
                }
            }

            player.myName = GameController.player.name;
            player.pokeBalls = GameController.player.displayPokeBalls;
            player.numPokeBalls = GameController.player.pokeBalls;
            player.greatBalls = GameController.player.displayGreatBalls;
            player.numGreatBalls = GameController.player.greatBalls;
            player.ultraBalls = GameController.player.displayUltraBalls;
            player.numUltraBalls = GameController.player.ultraBalls;
            player.masterBalls = GameController.player.displayMasterBalls;
            player.numMasterBalls = GameController.player.masterBalls;

            if (!player.pokeBalls) ball1.SetActive(false);
            if (!player.greatBalls) ball2.SetActive(false);
            if (!player.ultraBalls) ball3.SetActive(false);
            if (!player.masterBalls) ball4.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[0].name, "default") == 0) move1.SetActive(false);
            if (playerUnit.pokemon.currentMoves[1] == null) move2.SetActive(false);
            if (playerUnit.pokemon.currentMoves[2] == null) move3.SetActive(false);
            if (playerUnit.pokemon.currentMoves[3] == null) move4.SetActive(false);

            if (GameController.playerPokemon[1] == null) poke2.SetActive(false);
            if (GameController.playerPokemon[2] == null) poke3.SetActive(false);
            if (GameController.playerPokemon[3] == null) poke4.SetActive(false);
            if (GameController.playerPokemon[4] == null) poke5.SetActive(false);
            if (GameController.playerPokemon[5] == null) poke6.SetActive(false);

            GameObject enemyGO = Instantiate(enemyPrefab);
            enemyUnit = enemyGO.GetComponent<Unit>();
            enemyUnit.pokemon = GameController.opponentPokemon[0];
            Destroy(enemyGO);

            playerHUD.SetHUD(playerUnit, true, player, GameController.playerPokemon);
            enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);

            SetPlayerTrainerSprite(playerSprite);
            phasePlayerSprite = 1;
            SetBackground();

            if (GameController.isCatchable)
            {
                dialogueText.text = "A wild " + enemyUnit.pokemon.name + " appears!";
                SetOpponentSprite(enemyUnit, enemySprite);
            }
            else
            {
                SetOpponentTrainerSprite(enemySprite);
                phaseOpponentSprite = 1;
                dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " wants to battle!";
                yield return new WaitForSeconds(2);
                SetOpponentSprite(enemyUnit, enemySprite);
                phaseOpponentSprite = 2;
                dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " sends out " + enemyUnit.pokemon.name + "!";
            }
            yield return new WaitForSeconds(2);
            dialogueText.text = "Go, " + playerUnit.pokemon.name + "!";
            SetPlayerSprite(playerUnit, playerSprite);
            phasePlayerSprite = 2;
            yield return new WaitForSeconds(2);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        //This function sets up the battle state for us including the UI

        #endregion Set up battle and update functions

        #region Decision functions to see who goes first

        /// <summary>
        /// Decides who attacks first, based on priority of the move, then the speed of each pokemon, then random
        /// </summary>
        /// <param name="playerMoveNum">The player move number. -1 if struggle, -2 if continuing attack.</param>
        /// <returns>Nothing</returns>
        private IEnumerator Decision(int playerMoveNum)
        {
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();

            if (playerMoveNum >= 0)
            {
                if (playerUnit.pokemon.currentMoves[playerMoveNum].current_pp == 0)
                {
                    dialogueText.text = "No remaining PP for " + playerUnit.pokemon.currentMoves[playerMoveNum].name + "!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
            }
            int moveNum = -1;
            bool struggle = false;
            if (enemyContinuingAttack == 0) struggle = Utility.EnemyStruggle(enemyUnit);
            Moves enemyMove, playerMove;
            System.Random rnd = new System.Random();
            if (playerMoveNum == -2) playerMove = playerMoveStorage;
            else if (playerMoveNum != -1) playerMove = playerUnit.pokemon.currentMoves[playerMoveNum];
            else playerMove = playerUnit.pokemon.struggle;

            if (enemyContinuingAttack != 0)
            {
                enemyMove = enemyMoveStorage;
            }
            else if (struggle)
            {
                enemyMove = enemyUnit.pokemon.struggle;
            }
            else
            {
                do
                {
                    moveNum = rnd.Next(enemyUnit.pokemon.currentMoves.Count(s => s != null));
                    enemyMove = enemyUnit.pokemon.currentMoves[moveNum];
                }
                while (enemyUnit.pokemon.currentMoves[moveNum].current_pp == 0);
            }
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            SetDownButtons();
            playerMoveName = playerMove.name;
            enemyMoveName = enemyMove.name;

            if (playerMoveNum != -2 && playerMove.max_turns > 1)
            {
                playerContinuingAttack = rnd.Next(playerMove.min_turns, playerMove.max_turns + 1);
                playerMoveStorage = playerMove;
            }
            if (playerMoveNum == -2) playerContinuingAttack--;

            if (enemyContinuingAttack != 0) enemyContinuingAttack--;
            else if (enemyMove.max_turns > 1)
            {
                enemyContinuingAttack = rnd.Next(enemyMove.min_turns, enemyMove.max_turns + 1);
                enemyMoveStorage = enemyMove;
            }

            int numTimesPlayer = rnd.Next(playerMove.min_per_turn, playerMove.max_per_turn + 1);
            int numTimesEnemy = rnd.Next(enemyMove.min_per_turn, enemyMove.max_per_turn + 1);

            if (playerMoveNum >= 0) playerUnit.DoPP(playerMoveNum);
            if (moveNum >= 0) enemyUnit.DoPP(moveNum); //If it is not struggle, take down some PP.

            if (playerMove.priority > enemyMove.priority)
            {
                state = BattleState.PLAYERTURN;

                for (int k = 0; k < numTimesPlayer; k++)
                {
                    yield return StartCoroutine(PlayerAttack(playerMove));
                    if (breakOutOfDecision) break;
                }

                if (state != BattleState.CHANGEPOKEMON && !breakOutOfDecision)
                {
                    state = BattleState.ENEMYTURN;
                    for (int k = 0; k < numTimesEnemy; k++)
                    {
                        yield return StartCoroutine(EnemyAttack(enemyMove));
                        if (breakOutOfDecision) break;
                    }
                }
            }
            else if (playerMove.priority < enemyMove.priority)
            {
                state = BattleState.ENEMYTURN;
                for (int k = 0; k < numTimesEnemy; k++)
                {
                    yield return StartCoroutine(EnemyAttack(enemyMove));
                    if (breakOutOfDecision) break;
                }
                if (!breakOutOfDecision && !deadPokemon)
                {
                    state = BattleState.PLAYERTURN;
                    for (int k = 0; k < numTimesPlayer; k++)
                    {
                        yield return StartCoroutine(PlayerAttack(playerMove));
                        if (breakOutOfDecision) break;
                    }
                }
            }
            else
            {
                if (enemyUnit.pokemon.current_speed > playerUnit.pokemon.current_speed)
                {
                    state = BattleState.ENEMYTURN;
                    for (int k = 0; k < numTimesEnemy; k++)
                    {
                        yield return StartCoroutine(EnemyAttack(enemyMove));
                        if (breakOutOfDecision) break;
                    }
                    if (!breakOutOfDecision && !deadPokemon)
                    {
                        state = BattleState.PLAYERTURN;
                        for (int k = 0; k < numTimesPlayer; k++)
                        {
                            yield return StartCoroutine(PlayerAttack(playerMove));
                            if (breakOutOfDecision) break;
                        }
                    }
                }
                else if (enemyUnit.pokemon.current_speed < playerUnit.pokemon.current_speed)
                {
                    state = BattleState.PLAYERTURN;
                    for (int k = 0; k < numTimesPlayer; k++)
                    {
                        yield return StartCoroutine(PlayerAttack(playerMove));
                        if (breakOutOfDecision) break;
                    }
                    if (state != BattleState.CHANGEPOKEMON && !breakOutOfDecision)
                    {
                        state = BattleState.ENEMYTURN;
                        for (int k = 0; k < numTimesEnemy; k++)
                        {
                            yield return StartCoroutine(EnemyAttack(enemyMove));
                            if (breakOutOfDecision) break;
                        }
                    }
                }
                else
                {
                    int num = rnd.Next(1, 3);
                    if (num == 1)
                    {
                        state = BattleState.PLAYERTURN;
                        for (int k = 0; k < numTimesPlayer; k++)
                        {
                            yield return StartCoroutine(PlayerAttack(playerMove));
                            if (breakOutOfDecision) break;
                        }
                        if (state != BattleState.CHANGEPOKEMON && !breakOutOfDecision)
                        {
                            state = BattleState.ENEMYTURN;
                            for (int k = 0; k < numTimesEnemy; k++)
                            {
                                yield return StartCoroutine(EnemyAttack(enemyMove));
                                if (breakOutOfDecision) break;
                            }
                        }
                    }
                    else
                    {
                        state = BattleState.ENEMYTURN;
                        for (int k = 0; k < numTimesEnemy; k++)
                        {
                            yield return StartCoroutine(EnemyAttack(enemyMove));
                            if (breakOutOfDecision) break;
                        }
                        if (!breakOutOfDecision && !deadPokemon)
                        {
                            state = BattleState.PLAYERTURN;
                            for (int k = 0; k < numTimesPlayer; k++)
                            {
                                yield return StartCoroutine(PlayerAttack(playerMove));
                                if (breakOutOfDecision) break;
                            }
                        }
                    }
                }
            }
            state = BattleState.PLAYERTURN;
            if (deadPokemon)
            {
                yield return SwitchPokemonAfterDeath();
            }
            if (breakOutOfDecision)
            {
                yield return SeeIfEndBattle();
                PlayerTurn();
                yield break;
            }
            PlayerTurn();
            yield break;
        }

        /// <summary>
        /// Handles getting attacked when switching out Pokemon
        /// </summary>
        /// <returns>Nothing</returns>
        private IEnumerator DecisionYouDontAttack()
        {
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();

            int moveNum = 0;
            bool struggle = Utility.EnemyStruggle(enemyUnit);
            Moves enemyMove;
            System.Random rnd = new System.Random();
            if (struggle) enemyMove = enemyUnit.pokemon.struggle;
            else
            {
                do
                {
                    moveNum = rnd.Next(enemyUnit.pokemon.currentMoves.Count(s => s != null));
                    enemyMove = enemyUnit.pokemon.currentMoves[moveNum];
                }
                while (enemyUnit.pokemon.currentMoves[moveNum].current_pp == 0);
            }
            enemyMoveName = enemyMove.name;
            if (moveNum >= 0) enemyUnit.DoPP(moveNum);

            if (enemyContinuingAttack != 0) enemyContinuingAttack--;
            else if (enemyMove.max_turns > 1)
            {
                enemyContinuingAttack = rnd.Next(enemyMove.min_turns, enemyMove.max_turns + 1);
                enemyMoveStorage = enemyMove;
            }

            int numTimesEnemy = rnd.Next(enemyMove.min_per_turn, enemyMove.max_per_turn + 1);

            state = BattleState.ENEMYTURN;
            for (int k = 0; k < numTimesEnemy; k++)
            {
                yield return StartCoroutine(EnemyAttack(enemyMove));
                if (breakOutOfDecision) break;
            }
            if (breakOutOfDecision)
            {
                StartCoroutine(SeeIfEndBattle());
            }
            state = BattleState.PLAYERTURN;
            PlayerTurn();
            yield break;
        }

        #endregion Decision functions to see who goes first

        #region Player attack functions

        /// <summary>
        /// This function is what we go to before anything happens.
        /// It lets us know what the player chooses to do and will call the appropriate functions from there.
        /// When in doubt, call this function.
        /// </summary>
        private void PlayerTurn()
        {
            deadPokemon = false;
            breakOutOfDecision = false;
            if (playerUnit.pokemon.current_hp == 0)
            {
                state = BattleState.CHANGEPOKEMON;
                SwitchPokemonAfterDeath();
            }
            else
            {
                //state = BattleState.PLAYERTURN;
                dialogueText.text = "Choose an action";
                playerHUD.SetPokemon(GameController.playerPokemon);
                playerHUD.SetMoves(playerUnit);
                playerHUD.SetBalls(player);
                //playerHUD.SetEXP(playerUnit.pokemon);
                enemyHUD.SetStatus(enemyUnit.pokemon);
                playerHUD.SetStatus(playerUnit.pokemon);
                SetUpButtons();
                Debug.Log(playerUnit.pokemon.current_exp);
            }
        }

        /// <summary>
        /// Determines if the Player's attack hits or not, and then does all of the damage calculations and crits and all that.
        /// </summary>
        /// <param name="attack">The move we are attacking with.</param>
        /// <returns>Nothing</returns>
        private IEnumerator PlayerAttack(Moves attack)
        {
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            SetDownButtons();
            if (pWokeUp)
            {
                dialogueText.text = playerUnit.pokemon.name + " woke up!";
                pWokeUp = false;
            }

            if (Status.SeeIfParalyzed(playerUnit.pokemon))
            {
                dialogueText.text = playerUnit.pokemon.name + " is paralyzed!";
                yield return new WaitForSeconds(2);
                yield break;
            }
            if (Status.SeeIfSleep(playerUnit.pokemon))
            {
                dialogueText.text = playerUnit.pokemon.name + " is asleep!";
                Status.ReduceSleep(playerUnit);
                if (playerUnit.pokemon.sleep == 0) pWokeUp = true;
                yield return new WaitForSeconds(2);
                yield break;
            }
            if (Status.SeeIfFreeze(playerUnit.pokemon))
            {
                dialogueText.text = playerUnit.pokemon.name + " is frozen!";
                yield return new WaitForSeconds(2);
                yield break;
            }

            bool crit = Utility.CriticalHit(playerUnit);
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            dialogueText.text = playerUnit.pokemon.name + " used " + attack.name + "!";
            yield return new WaitForSeconds(0.75f);
            if (num <= (attack.accuracy * playerUnit.pokemon.current_accuracy * enemyUnit.pokemon.current_evasion)) //If the attack hits
            {
                playerInitialAttack = true;
                while (!endofanimation)
                {
                    yield return null;
                }
                endofanimation = false;

                if (attack.current_stat_change.CompareTo("null") != 0) playerUnit.SetStages(attack, enemyUnit);
                if (!attack.status.Equals("null")) Status.SeeIfStatusEffect(attack, enemyUnit);
                double super = Utility.DoDamage(playerUnit, enemyUnit, attack, crit);
                Debug.Log(playerUnit.damage);

                bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
                StartCoroutine(Blink(enemySprite, 0.25));
                enemyHUD.SetHP(enemyUnit.pokemon.current_hp);
                //playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
                enemyHUD.SetStatus(enemyUnit.pokemon);

                if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("enemy") == 0)
                {
                    dialogueText.text = "Enemy " + enemyUnit.pokemon.name + "'s " + attack.current_stat_change + " fell!"; //If you lower their stat
                    yield return new WaitForSeconds(2);
                }
                else if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("self") == 0)
                {
                    dialogueText.text = "Your " + playerUnit.pokemon.name + "'s " + attack.current_stat_change + " rose!"; //If you increase your own stat
                    yield return new WaitForSeconds(2);
                }
                if (attack.base_power != -1)//If this move is a damage dealing move.
                {
                    if (crit)
                    {
                        dialogueText.text = "Critical hit!";
                        yield return new WaitForSeconds(2);
                    }
                    if (super > 1)
                    {
                        dialogueText.text = "It's super effective!";
                    }
                    else if (super < 1 && super != 0)
                    {
                        dialogueText.text = "It's not very effective...";
                    }
                    else if (super == 0)
                    {
                        dialogueText.text = enemyUnit.pokemon.name + " is immune!";
                    }
                    else if (attack.status.SeeIfStatus(attack) && enemyUnit.pokemon.statuses.Contains(attack.status.name))
                    {
                        dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " is already " + attack.status.adj;
                    }
                    else if (enemyUnit.pokemon.statuses.Contains(attack.status.name))
                    {
                        dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " became " + attack.status.adj;
                    }
                    else dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " took damage...";
                    yield return new WaitForSeconds(2);
                    bool areYouDead = false;
                    if (playerUnit.pokemon.statuses.Contains(Status.get_status("Burn")))
                    {
                        playerUnit.BurnSelf();
                        StartCoroutine(Blink(playerSprite, 0.25));
                        playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
                        dialogueText.text = playerUnit.pokemon.name + " got burned!";
                        if (playerUnit.pokemon.current_hp <= 0) areYouDead = true;
                        else areYouDead = false;
                        yield return new WaitForSeconds(2);
                        yield return StartCoroutine(EnemyKillsYou(areYouDead));
                    }
                    if (Status.SeeIfPoisoned(playerUnit.pokemon))
                    {
                        playerUnit.PoisonSelf();
                        StartCoroutine(Blink(playerSprite, 0.25));
                        playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
                        dialogueText.text = playerUnit.pokemon.name + " is poisoned!";
                        if (playerUnit.pokemon.current_hp <= 0) areYouDead = true;
                        else areYouDead = false;
                        yield return new WaitForSeconds(2);
                        yield return StartCoroutine(EnemyKillsYou(areYouDead));
                    }
                }

                yield return StartCoroutine(YouKilledThem(isDead));
            }
            else //If your attack missed
            {
                dialogueText.text = "Your attack missed!";
                yield return new WaitForSeconds(2);
                yield break;
            }
        }

        public IEnumerator YouKilledThem(bool isDead)
        {
            if (isDead) //Determines what to do if the opponent's Pokemon died.
            {
                breakOutOfDecision = true;
                bool won = true;
                for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++) //Determines if you win or not by seeing if the other trainer has other usable pokemon.
                {
                    if (GameController.opponentPokemon[j].current_hp > 0)
                    {
                        won = false;
                        break;
                    }
                }
                phaseOpponentSprite = 1;
                if (won) //If you won
                {
                    state = BattleState.WON;
                    dialogueText.text = enemyUnit.pokemon.name + " faints!";
                    int exp = 0;
                    double exp_multiplier;
                    //Debug.Log(playerUnit.pokemon.base_lvl_exp + " " + playerUnit.pokemon.current_exp + " " + playerUnit.pokemon.next_lvl_exp);
                    //is the pokemon catachble? yes its wild, set exp_multiplier to 1, no? 1.5
                    exp_multiplier = (GameController.isCatchable) ? 1 : 1.5;
                    string test = exp_multiplier == 1 ? "pokemon is wild" : "pokemon is a trainers";
                    Debug.Log(test);
                    exp = playerUnit.pokemon.gain_exp(enemyUnit.pokemon.level, enemyUnit.pokemon.pokedex_entry.base_exp, 1, exp_multiplier);

                    yield return new WaitForSeconds(2);
                    dialogueText.text = playerUnit.pokemon.name + " gained " + exp + " EXP!";
                    playerHUD.SetEXP(playerUnit.pokemon, exp);
                    if (playerUnit.pokemon.gained_a_level)
                    {
                        yield return StartCoroutine(LevelUp(playerUnit.pokemon));
                    }
                    //Debug.Log(playerUnit.pokemon.base_lvl_exp + " " + playerUnit.pokemon.current_exp + " " + playerUnit.pokemon.next_lvl_exp);
                    yield return new WaitForSeconds(2);
                    yield break;
                }
                else //If you didn't win, they bring out a new Pokemon
                {
                    state = BattleState.CHANGEPOKEMON;

                    int exp = 0;
                    double exp_multiplier;
                    dialogueText.text = enemyUnit.pokemon.name + " faints!";

                    //is the pokemon catachble? yes its wild, set exp_multiplier to 1, no? 1.5
                    exp_multiplier = (GameController.isCatchable) ? 1 : 1.5;
                    exp = playerUnit.pokemon.gain_exp(enemyUnit.pokemon.level, enemyUnit.pokemon.pokedex_entry.base_exp, 1, exp_multiplier);
                    /*                    string test = exp_multiplier == 1 ? "pokemon is wild" : "pokemon is a trainers";
                                        Debug.Log(test);*/
                    yield return new WaitForSeconds(2);
                    dialogueText.text = playerUnit.pokemon.name + " gained " + exp + " EXP!";
                    playerHUD.SetEXP(playerUnit.pokemon, exp);
                    yield return new WaitForSeconds(2);
                    if (playerUnit.pokemon.gained_a_level)
                    {
                        yield return StartCoroutine(LevelUp(playerUnit.pokemon));
                    }
                    for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                    {
                        if (GameController.opponentPokemon[j].current_hp > 0)
                        {
                            enemyUnit.pokemon = GameController.opponentPokemon[j];
                            dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " sent out a " + enemyUnit.pokemon.name + "!";
                            yield return new WaitForSeconds(2);
                            enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);
                            SetOpponentSprite(enemyUnit, enemySprite);
                            phaseOpponentSprite = 2;
                            break;
                        }
                    }
                    PlayerTurn();
                    yield break;
                }
            }
        }

        /// <summary>
        /// Logic to execute if you try to catch a Pokemon.
        /// </summary>
        /// <param name="typeOfPokeball">The type of pokeball. 1 = Poke, 2 = Great, 3 = Ultra, 4 = Master</param>
        /// <returns>Nothing.</returns>
        private IEnumerator CatchPokemon(int typeOfPokeball)
        {
            SetDownButtons();
            if (!GameController.isCatchable) //If you are playing a trainer, you can't catch their Pokemon.
            {
                dialogueText.text = "You can't catch other trainer's Pokemon!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }
            System.Random rnd = new System.Random();

            int randomNumber = 0, catchRate = GameController.catchRate, randomNumber2, f, numShakes = 0;
            if (typeOfPokeball == 1) //If you have a Poke Ball
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
            if (typeOfPokeball == 2) //Great Ball
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
            if (typeOfPokeball == 3) //Ultra Ball
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
            if (typeOfPokeball == 4) //Master Ball
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

            if ((Status.SeeIfSleep(enemyUnit.pokemon) || Status.SeeIfFreeze(enemyUnit.pokemon)) && randomNumber < 25)
            {
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }
            if ((Status.SeeIfParalyzed(enemyUnit.pokemon) || Status.SeeIfPoisoned(enemyUnit.pokemon) || Status.SeeIfBurned(enemyUnit.pokemon)) && randomNumber < 12)
            {
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }
            int catchRateModifier = 0;
            if (Status.SeeIfSleep(enemyUnit.pokemon) || Status.SeeIfFreeze(enemyUnit.pokemon)) catchRateModifier = 25;
            else if (Status.SeeIfParalyzed(enemyUnit.pokemon) || Status.SeeIfPoisoned(enemyUnit.pokemon) || Status.SeeIfBurned(enemyUnit.pokemon)) catchRateModifier = 12;
            if (randomNumber > GameController.catchRate - catchRateModifier) //If they broke free
            {
                dialogueText.text = enemyUnit.pokemon.name + " broke free!";
                yield return new WaitForSeconds(2);
                StartCoroutine(DecisionYouDontAttack());
                yield break;
            }
            randomNumber2 = rnd.Next(256);
            f = (enemyUnit.pokemon.max_hp * 255 * 4);
            f = f / (enemyUnit.pokemon.current_hp * catchRate);
            if (f >= randomNumber2) //If you actually caught them
            {
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }
            else //If they broke free again.
            {
                int d = (GameController.catchRate * 100) / numShakes;
                if (d < 10) { }//no shakes
                else if (d < 30) { }//one shake
                else if (d < 70) { }//two shakes
                else { }//three shakes
                dialogueText.text = enemyUnit.pokemon.name + " broke free!";
                yield return new WaitForSeconds(2);
                StartCoroutine(DecisionYouDontAttack());
                yield break;
            }
        }

        /// <summary>
        /// Switches the player's active Pokemon.
        /// </summary>
        /// <param name="num">The index of the player's Pokemon in the array in GameController.</param>
        /// <returns>Nothing.</returns>
        private IEnumerator SwitchPokemon(int num)
        {
            if (GameController.playerPokemon[num].current_hp <= 0) //If the Pokemon you selected has no health remaining.
            {
                dialogueText.text = "That Pokemon has no HP remaining!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.CHANGEPOKEMON) PlayerTurn();
                else yield return SwitchPokemonAfterDeath();
                yield break;
            }
            else if (activePokemon == num) //If you try to swap out the current active Pokemon.
            {
                dialogueText.text = "You already have that Pokemon out!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.CHANGEPOKEMON) PlayerTurn();
                else yield return SwitchPokemonAfterDeath();
                yield break;
            }
            else
            {
                playerContinuingAttack = 0;
                phasePlayerSprite = 1;
                dialogueText.text = "Get out of there, " + playerUnit.pokemon.name + "!";
                playerUnit.pokemon.reset_stages();
                GameController.playerPokemon[activePokemon] = playerUnit.pokemon;
                yield return new WaitForSeconds(2);
                playerUnit.pokemon = GameController.playerPokemon[num];
                activePokemon = num;

                playerHUD.SetActivePokemon(GameController.playerPokemon, num, playerUnit);
                playerHUD.SetEXP(playerUnit.pokemon);
                SetPlayerSprite(playerUnit, playerSprite);
                phasePlayerSprite = 2;
                dialogueText.text = "Go, " + playerUnit.pokemon.name + "!";
                yield return new WaitForSeconds(2);

                //Sets the moves buttons up.
                if (playerUnit.pokemon.currentMoves[0] == null)
                {
                    move1.SetActive(false);
                }
                else
                {
                    move1.SetActive(true);
                }
                if (playerUnit.pokemon.currentMoves[1] == null)
                {
                    move2.SetActive(false);
                }
                else
                {
                    move2.SetActive(true);
                }
                if (playerUnit.pokemon.currentMoves[2] == null)
                {
                    move3.SetActive(false);
                }
                else
                {
                    move3.SetActive(true);
                }
                if (playerUnit.pokemon.currentMoves[3] == null)
                {
                    move4.SetActive(false);
                }
                else
                {
                    move4.SetActive(true);
                }
                if (state == BattleState.CHANGEPOKEMON)
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    //backPoke.interactable = true;

                    StartCoroutine(DecisionYouDontAttack());
                }
            }
            yield break;
        }

        /// <summary>
        /// Function that is called after one of your Pokemon dies.
        /// </summary>
        private IEnumerator SwitchPokemonAfterDeath()
        {
            state = BattleState.CHANGEPOKEMON;
            playerHUD.SetPokemon(GameController.playerPokemon);
            SetDownButtons();
            OpenPokemonMenu();
            yield break;
        }

        #endregion Player attack functions

        #region Enemy attack functions

        /// <summary>
        /// The enemy version of player attack.
        /// There is only one function because I cleaned up this code to allow one to be used for struggle.
        /// There is no reason this can't be done for the player functions, I just haven't gotten around to it yet and that is very low on my priority list.
        /// </summary>
        /// <param name="move">The attack the enemy Pokemon chose.</param>
        /// <returns>Nothing.</returns>
        private IEnumerator EnemyAttack(Moves move)
        {
            SetDownButtons();
            if (eWokeUp)
            {
                dialogueText.text = enemyUnit.pokemon.name + " woke up!";
                eWokeUp = false;
            }

            if (Status.SeeIfParalyzed(enemyUnit.pokemon))
            {
                dialogueText.text = enemyUnit.pokemon.name + " is paralyzed!";
                yield return new WaitForSeconds(2);
                yield break;
            }
            if (Status.SeeIfSleep(enemyUnit.pokemon))
            {
                dialogueText.text = enemyUnit.pokemon.name + " is asleep!";
                Status.ReduceSleep(enemyUnit);
                if (enemyUnit.pokemon.sleep == 0) eWokeUp = true;
                yield return new WaitForSeconds(2);
                yield break;
            }
            if (Status.SeeIfFreeze(enemyUnit.pokemon))
            {
                dialogueText.text = enemyUnit.pokemon.name + " is frozen!";
                yield return new WaitForSeconds(2);
                yield break;
            }

            System.Random rnd = new System.Random();
            bool crit = Utility.CriticalHit(enemyUnit);
            int num = rnd.Next(1, 100);
            dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " used " + move.name + "!";
            if (num <= move.accuracy * enemyUnit.pokemon.current_accuracy * playerUnit.pokemon.current_evasion) //If the move hits
            {
                yield return new WaitForSeconds(0.75f);
                enemyInitialAttack = true;
                while (!endofanimation) //Animation shit, ask levi
                {
                    yield return null;
                }
                endofanimation = false;

                if (move.current_stat_change.CompareTo("null") != 0) enemyUnit.SetStages(move, playerUnit);
                if (!move.status.Equals("null")) Status.SeeIfStatusEffect(move, playerUnit);
                double super = Utility.DoDamage(enemyUnit, playerUnit, move, crit);
                bool isDead = playerUnit.TakeDamage(enemyUnit.damage); //Forgot to comment this earlier, but this is where the damage actually gets applied.
                StartCoroutine(Blink(playerSprite, 0.25));
                //Debug.Log(enemyUnit.damage.ToString());

                playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
                //enemyHUD.SetHP(enemyUnit.pokemon.current_hp);

                if (move.current_stat_change.CompareTo("null") != 0 && move.target.CompareTo("enemy") == 0) dialogueText.text = "Your " + playerUnit.pokemon.name + "'s " + move.current_stat_change + " fell!";
                else if (move.current_stat_change.CompareTo("null") != 0 && move.target.CompareTo("self") == 0) dialogueText.text = "Enemy " + enemyUnit.pokemon.name + "'s " + move.current_stat_change + " rose!";
                else
                {
                    if (crit)
                    {
                        dialogueText.text = "Critical hit!";
                        yield return new WaitForSeconds(2);
                    }
                    if (super > 1)
                    {
                        dialogueText.text = "It's super effective!";
                    }
                    else if (super < 1 && super != 0)
                    {
                        dialogueText.text = "It's not very effective...";
                    }
                    else if (move.status.SeeIfStatus(move) && playerUnit.pokemon.statuses.Contains(move.status.name))
                    {
                        dialogueText.text = "Your " + playerUnit.pokemon.name + " is already " + move.status.adj + "!";
                    }
                    else if (playerUnit.pokemon.statuses.Contains(move.status.name))
                    {
                        dialogueText.text = "Your " + playerUnit.pokemon.name + " became " + move.status.adj + "!";
                    }
                    else if (super == 0)
                    {
                        dialogueText.text = "Your " + playerUnit.pokemon.name + " is immune!";
                    }
                    else dialogueText.text = "Your " + playerUnit.pokemon.name + " took damage...";
                }
                yield return new WaitForSeconds(2);
                bool isYouDead;
                if (enemyUnit.pokemon.statuses.Contains(Status.get_status("Burn")))
                {
                    enemyUnit.BurnSelf();
                    StartCoroutine(Blink(enemySprite, 0.25));
                    enemyHUD.SetHP(enemyUnit.pokemon.current_hp);
                    dialogueText.text = enemyUnit.pokemon.name + " got burned!";
                    if (enemyUnit.pokemon.current_hp <= 0) isYouDead = true;
                    else isYouDead = false;
                    yield return new WaitForSeconds(2);
                    yield return StartCoroutine(YouKilledThem(isYouDead));
                }

                if (Status.SeeIfPoisoned(enemyUnit.pokemon))
                {
                    enemyUnit.PoisonSelf();
                    StartCoroutine(Blink(enemySprite, 0.25));
                    enemyHUD.SetHP(enemyUnit.pokemon.current_hp);
                    dialogueText.text = enemyUnit.pokemon.name + " is poisoned!";
                    if (enemyUnit.pokemon.current_hp <= 0) isYouDead = true;
                    else isYouDead = false;
                    yield return new WaitForSeconds(2);
                    yield return StartCoroutine(YouKilledThem(isYouDead));
                }

                yield return StartCoroutine(EnemyKillsYou(isDead));
            }
            else //If the enemy pokemon misses.
            {
                yield return new WaitForSeconds(2);
                dialogueText.text = "The move failed!";
                yield return new WaitForSeconds(2);
                yield break;
            }
            yield break;
        }

        public IEnumerator EnemyKillsYou(bool isDead)
        {
            if (isDead) //If your Pokemon died.
            {
                breakOutOfDecision = true;
                for (int j = 0; j < GameController.playerPokemon.Count(s => s != null); j++) //See if you have any other usable Pokemon.
                {
                    if (GameController.playerPokemon[j].current_hp > 0)
                    {
                        breakOutOfDecision = false;
                        break;
                    }
                }
                if (breakOutOfDecision) //If you lost the battle.
                {
                    state = BattleState.LOST;
                    yield break;
                }
                else //If you have other Pokemon.
                {
                    deadPokemon = true;
                    state = BattleState.CHANGEPOKEMON;
                    dialogueText.text = playerUnit.pokemon.name + " faints!";
                    yield return new WaitForSeconds(2);
                    yield return SwitchPokemonAfterDeath();
                    yield break;
                }
            }
        }

        #endregion Enemy attack functions

        #region End of Battle functions

        /// <summary>
        /// Sees if our battle is over.
        /// To be honest, I forgot why I had to split this out into its own function.
        /// It probably has to do with the decision tree, but until I have time to figure out why I did this,
        /// please keep this function so we don't have errors.
        /// </summary>
        /// <returns>Nothing</returns>
        private IEnumerator SeeIfEndBattle()
        {
            bool isEnd = true;
            for (int j = 0; j < GameController.playerPokemon.Count(s => s != null); j++)
            {
                if (GameController.playerPokemon[j].current_hp > 0)
                {
                    isEnd = false;
                    break;
                }
            }
            if (isEnd)
            {
                state = BattleState.LOST;
                yield return StartCoroutine(EndBattle());
                yield break;
            }
            isEnd = true;
            for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
            {
                if (GameController.opponentPokemon[j].current_hp > 0)
                {
                    isEnd = false;
                    break;
                }
            }
            if (isEnd)
            {
                state = BattleState.WON;
                yield return StartCoroutine(EndBattle());
            }
        }

        /// <summary>
        /// Ends the battle.
        /// </summary>
        /// <returns>Nothing.</returns>
        private IEnumerator EndBattle()
        {
            breakOutOfDecision = true;
            SetDownButtons();
            GameController.update_level_cap();
            if (state == BattleState.WON) //If you won
            {
                if (GameController.isCatchable)
                {
                    GameController.music = "Battle Victory Wild";
                    dialogueText.text = player.myName + " won!";
                }
                else
                {
                    if (GameController.opponentName == "Brock")
                        GameController.music = "Battle Victory Gym";
                    else
                        GameController.music = "Battle Victory Trainer";
                    phasePlayerSprite = 1;

                    yield return new WaitForSeconds(2);
                    SetPlayerTrainerSprite(playerSprite);
                    SetOpponentTrainerSprite(enemySprite);
                    phasePlayerSprite = 2;
                    phaseOpponentSprite = 2;
                    dialogueText.text = player.myName + " defeated " + GameController.opponentType + " " + GameController.opponentName + "!";
                    yield return new WaitForSeconds(2);
                    dialogueText.text = "\"" + GameController.endText + "\"";
                    yield return new WaitForSeconds(2);
                    dialogueText.text = player.myName + " got ?" + GameController.winMoney + " for winning!";
                    GameController.player.money += GameController.winMoney;
                }
            }
            else if (state == BattleState.LOST) //If you lost
            {
                dialogueText.text = player.myName + " lost! You blacked out!";
            }
            else if (state == BattleState.RUNAWAY) //If you ran away. TO DO: Make it so you don't automatically run away.
            {
                dialogueText.text = "Got away safely...";
            }
            else if (state == BattleState.CAUGHTPOKEMON) //If you were successful in catching the pokemon
            {
                GameController.music = "Battle Victory Wild";
                dialogueText.text = "You caught a " + enemyUnit.pokemon.name + "!";
                for (var p = 0; p < 6; p++)
                {
                    if (GameController.playerPokemon[p] == null) //Right now, if you have 6 or more pokemon, the game just throws it away. Otherwise it gets added to
                    {                                            //the end of your pokemon array. TO DO: Need to fix that.
                        GameController.playerPokemon[p] = enemyUnit.pokemon;
                        break;
                    }
                }
            }
            else //If something else happened to end the battle: should never happen.
            {
                dialogueText.text = "What the fuck just happened";
            }
            yield return new WaitForSeconds(2);
            GameController.playerPokemon[activePokemon] = playerUnit.pokemon; //Places your active pokemon back in the array - this is so its stats gets updated.
            for (int i = 0; i < 5; i++) //Resets the stages and attacks of all of your pokemon.
            {
                if (GameController.playerPokemon[i] != null)
                {
                    GameController.playerPokemon[i].critical_stage = 0;
                    GameController.playerPokemon[i].attack_stage = 0;
                    GameController.playerPokemon[i].defense_stage = 0;
                    GameController.playerPokemon[i].sp_attack_stage = 0;
                    GameController.playerPokemon[i].sp_defense_stage = 0;
                    GameController.playerPokemon[i].speed_stage = 0;
                    GameController.playerPokemon[i].accuracy_stage = 0;
                    GameController.playerPokemon[i].evasion_stage = 0;
                    GameController.playerPokemon[i].current_attack = GameController.playerPokemon[i].max_attack;
                    GameController.playerPokemon[i].current_defense = GameController.playerPokemon[i].max_defense;
                    GameController.playerPokemon[i].current_sp_attack = GameController.playerPokemon[i].max_sp_attack;
                    GameController.playerPokemon[i].current_sp_defense = GameController.playerPokemon[i].max_sp_defense;
                    if (!Status.SeeIfParalyzed(GameController.playerPokemon[i])) GameController.playerPokemon[i].current_speed = GameController.playerPokemon[i].max_speed;
                    else GameController.playerPokemon[i].current_speed = GameController.playerPokemon[i].max_speed * 0.5;
                    GameController.playerPokemon[i].current_accuracy = 1;
                    GameController.playerPokemon[i].current_evasion = 1;
                    GameController.player.pokeBalls = player.numPokeBalls;
                    GameController.player.greatBalls = player.numGreatBalls;
                    GameController.player.ultraBalls = player.numUltraBalls;
                    GameController.player.masterBalls = player.numMasterBalls;
                }
            }
            GameController.endCombat = true; //Something for levi.
            Destroy(playerSprite);
            Destroy(enemySprite);
            //SceneManager.UnloadSceneAsync("BattleScene");
            //SceneManager.LoadScene("Pallet Town");
        }

        #endregion End of Battle functions

        #region utility functions for the ui and picking options

        /// <summary>
        /// Called when [attack button] is pressed. Pulls up the attack menu.
        /// </summary>
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

        /// <summary>
        /// Called when [run away button] is pressed. Allows the player to run away.
        /// TO DO: Make it so that running away is not automatic.
        /// </summary>
        public void OnRunAwayButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            attackMenuUI.SetActive(false);
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            StartCoroutine(RunAway());
        }

        /// <summary>
        /// Called when [pokemon button] is pressed. Pulls up pokemon menu so you can swap out pokemon.
        /// </summary>
        public void OnPokemonButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            if (poekmonMenuOpen) ClosePokemonMenu();
            else
            {
                attackMenuUI.SetActive(false);
                OpenPokemonMenu();
            }
        }

        /// <summary>
        /// Called when [balls button] is pressed. Pulls up the balls menu.
        /// I kept losing my balls in development.
        /// </summary>
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

        /// <summary>
        /// Runs away. If you are in a trainer battle, you can't run away.
        /// TO DO: said it other places here but make this not automatic.
        /// </summary>
        /// <returns>Nothing.</returns>
        public IEnumerator RunAway()
        {
            if (!GameController.isCatchable)
            {
                CloseBallsMenu();
                CloseMovesMenu();
                ClosePokemonMenu();
                SetDownButtons();
                dialogueText.text = "You hear Prof Oak say \"Don't run away.\"";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }
            else
            {
                CloseBallsMenu();
                CloseMovesMenu();
                ClosePokemonMenu();
                SetDownButtons();
                double a = playerUnit.pokemon.current_speed;
                double b = a / 4;
                b %= 256;
                if (b == 0)
                {
                    state = BattleState.RUNAWAY;
                    StartCoroutine(EndBattle());
                }
                double f = (((a * 32) / b) + 30) * runAwayNum;
                runAwayNum++;
                if (f > 255)
                {
                    state = BattleState.RUNAWAY;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    int r = GameController._rnd.Next(256);
                    if (r < f)
                    {
                        state = BattleState.RUNAWAY;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        StartCoroutine(FailRunAway());
                    }
                }
            }
        }

        /// <summary>
        /// Sets up buttons. Makes it so you can click on them.
        /// </summary>
        public void SetUpButtons()
        {
            attackButton.interactable = true;
            runAwayButton.interactable = true;
            pokemonButton.interactable = true;
            ballsButton.interactable = true;
        }

        /// <summary>
        /// Sets down buttons. Makes it so you can NOT click on them.
        /// </summary>
        public void SetDownButtons()
        {
            attackButton.interactable = false;
            runAwayButton.interactable = false;
            pokemonButton.interactable = false;
            ballsButton.interactable = false;
        }

        /// <summary>
        /// Opens the pokemon menu. Allows you to select a pokemon to bring out and change.
        /// </summary>
        public void OpenPokemonMenu()
        {
            pokeMenuUI.SetActive(true);
            Time.timeScale = 0f;
            poekmonMenuOpen = true;
            CloseBallsMenu();
            CloseMovesMenu();
            //SetDownButtons();
        }

        /// <summary>
        /// Closes the pokemon menu.
        /// </summary>
        public void ClosePokemonMenu()
        {
            pokeMenuUI.SetActive(false);
            Time.timeScale = 1f;
            poekmonMenuOpen = false;
            SetUpButtons();
        }

        /// <summary>
        /// Called when [pokemon0] button is pressed. Brings out the first pokemon in the player's array.
        /// </summary>
        public void OnPokemon0()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(0));
        }

        /// <summary>
        /// Called when [pokemon1] button is pressed. Brings out the second pokemon in the player's array.
        /// </summary>
        public void OnPokemon1()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(1));
        }

        /// <summary>
        /// Called when [pokemon2] button is pressed. Brings out the third pokemon in the player's array.
        /// </summary>
        public void OnPokemon2()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(2));
        }

        /// <summary>
        /// Called when [pokemon3] button is pressed. Brings out the fourth pokemon in the player's array.
        /// </summary>
        public void OnPokemon3()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(3));
        }

        /// <summary>
        /// Called when [pokemon4] button is pressed. Brings out the fifth pokemon in the player's array.
        /// </summary>
        public void OnPokemon4()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(4));
        }

        /// <summary>
        /// Called when [pokemon5] button is pressed. Brings out the sixth pokemon in the player's array.
        /// </summary>
        public void OnPokemon5()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(SwitchPokemon(5));
        }

        /// <summary>
        /// Opens the moves menu. If you have no remaining PP left, clicking on the moves menu will automatically select struggle and advance to the decision function.
        /// </summary>
        public void OpenMovesMenu()
        {
            if (playerContinuingAttack != 0)
            {
                StartCoroutine(Decision(-2));
                return;
            }

            bool struggle = false;
            for (int i = 0; i < playerUnit.pokemon.currentMoves.Count(s => s != null); i++)
            {
                if (playerUnit.pokemon.currentMoves[i].current_pp == 0)
                {
                    struggle = true;
                }
                struggle = false;
            }
            if (struggle)
            {
                StartCoroutine(Decision(-1));
                return;
            }
            attackMenuUI.SetActive(true);
            attackMenuOpen = true;
        }

        /// <summary>
        /// Closes the moves menu.
        /// </summary>
        public void CloseMovesMenu()
        {
            attackMenuUI.SetActive(false);
            attackMenuOpen = false;
        }

        /// <summary>
        /// Opens the balls menu.
        /// </summary>
        public void OpenBallsMenu()
        {
            ballsMenuUI.SetActive(true);
            ballsMenuOpen = true;
        }

        /// <summary>
        /// Closes the balls menu.
        /// </summary>
        public void CloseBallsMenu()
        {
            ballsMenuUI.SetActive(false);
            ballsMenuOpen = false;
        }

        /// <summary>
        /// Selects the first attack. Top left.
        /// </summary>
        public void Attack1()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(0));
        }

        /// <summary>
        /// Selects the second attack. bottom left.
        /// </summary>
        public void Attack2()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(1));
        }

        /// <summary>
        /// Selects the third attack. bottom right.
        /// </summary>
        public void Attack3()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(2));
        }

        /// <summary>
        /// Selects the fourth attack. Top right.
        /// </summary>
        public void Attack4()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(3));
        }

        /// <summary>
        /// Pokes the ball. Lol no that was the autogenerated comment. Sends out a pokeball to catch the other pokemon. Decision Catch handles if you cant.
        /// </summary>
        public void PokeBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(1));
        }

        /// <summary>
        /// Greats the ball.
        /// </summary>
        public void GreatBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(2));
        }

        /// <summary>
        /// Ultras the ball.
        /// </summary>
        public void UltraBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(3));
        }

        /// <summary>
        /// Masters the ball.
        /// </summary>
        public void MasterBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(CatchPokemon(4));
        }

        public void Forget1()
        {
            StartCoroutine(ForgetMove(1, playerUnit.pokemon));
        }

        public void Forget2()
        {
            StartCoroutine(ForgetMove(2, playerUnit.pokemon));
        }

        public void Forget3()
        {
            StartCoroutine(ForgetMove(3, playerUnit.pokemon));
        }

        public void Forget4()
        {
            StartCoroutine(ForgetMove(4, playerUnit.pokemon));
        }

        public void Forget5()
        {
            StartCoroutine(ForgetMove(5, playerUnit.pokemon));
        }

        public IEnumerator FailRunAway()
        {
            dialogueText.text = "Can't Escape!";
            yield return new WaitForSeconds(2);
            StartCoroutine(DecisionYouDontAttack());
        }

        #endregion utility functions for the ui and picking options

        #region Sprite functions

        private void SetPlayerTrainerSprite(SpriteRenderer spriteRenderer)
        {
            string path = "Player_Rival/Player1";
            var sprite = Resources.Load<Sprite>(path);

            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 0.30f;
                y = 0.25f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.20f;
                y = 0.30f;
            }
            spriteRenderer.sprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.SmoothStep(255, 0, 2.0f));
        }

        private void SetPlayerSprite(Unit unit, SpriteRenderer sprite)
        {
            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 0.58f;
                y = 0.20f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.00f;
                y = 0.50f;
            }
            Texture2D SpriteTexture = new Texture2D(0, 0);
            byte[] fileData = File.ReadAllBytes(unit.pokemon.image1);
            SpriteTexture.LoadImage(fileData);

            sprite.sprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(x, y));

            GameController.soundFX = unit.pokemon.dexnum.ToString();
        }

        private void SetOpponentTrainerSprite(SpriteRenderer spriteRenderer)
        {
            string type = GameController.opponentType;
            string path;

            if (type != "Gym Leader")
                path = "Final_NPC/" + type;
            else
                path = "GymLeaders/" + GameController.opponentName;

            var sprite = Resources.Load<Sprite>(path);

            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 0.00f;
                y = 0.20f;
            }
            /*if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.00f;
                y = 0.35f;
            }*/
            spriteRenderer.sprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
        }

        private void SetOpponentSprite(Unit unit, SpriteRenderer sprite)
        {
            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 0.10f;
                y = 0.40f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.00f;
                y = 0.35f;
            }
            Texture2D SpriteTexture = new Texture2D(0, 0);
            byte[] fileData = File.ReadAllBytes(unit.pokemon.image2);
            SpriteTexture.LoadImage(fileData);
            sprite.sprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(x, y));
            GameController.soundFX = unit.pokemon.dexnum.ToString();
            Debug.Log("DexNum: " + unit.pokemon.dexnum.ToString());
        }

        public void SetBackground()
        {
            Texture2D SpriteTexture = new Texture2D(2, 2);
            var path = "Assets/Resources/Images/Backgrounds/";

            byte[] fileData = File.ReadAllBytes(path + GameController.location + ".png");
            SpriteTexture.LoadImage(fileData);
            background.sprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));
        }

        public void GetAttackSprites(string attack)
        {
            bool isPlayer = false;
            if (state == BattleState.PLAYERTURN) isPlayer = true;
            attack = attack.Replace(" ", "_");
            string path = "Attack_Animations/" + attack;

            AttackSprites.Clear();
            AttackSprites.Add(null);
            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0 && isPlayer)
            {
                x = 1.24f;
                y = 1.125f;
            }
            if (GameController.location.CompareTo("Route 1") == 0 && !isPlayer)
            {
                x = 0.30f;
                y = 0.40f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0 && isPlayer)
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0 && !isPlayer)
            {
                x = -0.25f;
                y = 0.65f;
            }

            var sprites = Resources.LoadAll<Sprite>(path);

            if (sprites.Length == 0)
            {
                path = "Attack_Animations/Tackle";
                sprites = Resources.LoadAll<Sprite>(path);
            }

            foreach (var sprite in sprites)
            {
                Sprite s = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
                AttackSprites.Add(s);
            }
            AttackSprites.TrimExcess();
        }

        public IEnumerator PhaseOut(SpriteRenderer sprite, double time)
        {
            var x = time / 128;
            for (int i = 255; i >= 0; i -= 2)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (float)i / 255f);
                yield return new WaitForSeconds((float)x);
            }
        }

        public IEnumerator PhaseIn(SpriteRenderer sprite, double time)
        {
            var x = time / 128;
            for (int i = 0; i < 256; i += 2)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (float)i / 255f);
                yield return new WaitForSeconds((float)x);
            }
        }

        public IEnumerator Blink(SpriteRenderer sprite, double time)
        {
            int numTimes = 2;
            var x = time / numTimes;
            bool visible = true;
            var pos = camera.transform.position;

            for (int i = 0; i < numTimes * 2; i++)
            {
                if (i % 2 == 0) ShakeLeft(pos);
                else ShakeRight(pos);
                if (visible)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
                    visible = false;
                }
                else
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                    visible = true;
                }
                yield return new WaitForSeconds((float)x);
                StopShake(pos);
            }
        }

        public void ShakeCamera(Vector3 pos)
        {
            camera.transform.position = pos + UnityEngine.Random.insideUnitSphere * 0.5f;
        }

        public void StopShake(Vector3 pos)
        {
            camera.transform.position = pos;
        }

        public void ShakeRight(Vector3 pos)
        {
            camera.transform.position = pos + new Vector3(pos.x + 0.25f, 0) * 1;
        }

        public void ShakeLeft(Vector3 pos)
        {
            camera.transform.position = pos + new Vector3(pos.x - 0.25f, 0) * 1;
        }

        public void ShakeLeftRightRandom(Vector3 pos)
        {
            camera.transform.position = pos + new Vector3(UnityEngine.Random.insideUnitSphere.x, 0) * 1;
        }

        public void ShakeLeftRight(Vector3 pos)
        {
            var numTimes = 2;
            for (int i = 0; i < numTimes * 2; i++)
            {
                if (i % 2 == 0) ShakeLeft(pos);
                else ShakeRight(pos);
            }
        }

        public IEnumerator SlideInLeft(SpriteRenderer sprite)
        {
            int numberOfSteps = 10;
            float finalX = sprite.sprite.pivot.x;
            float finalY = sprite.sprite.pivot.y;
            for (float i = 0; i < finalX; i += finalX / numberOfSteps)
            {
                sprite.sprite = Sprite.Create(sprite.sprite.texture, sprite.sprite.rect, new Vector2(i, finalY));
                yield return new WaitForSeconds(2 / numberOfSteps);
            }
        }

        #endregion Sprite functions

        #region Jake's functions

        public static List<Dictionary<string, object>> load_CSV(string name)
        {
            return CSVReader.Read(name);
        }

        #endregion Jake's functions

        #region leveling up

        public IEnumerator LevelUp(Pokemon poke)
        {
            poke.gained_a_level = false;
            dialogueText.text = poke.name + " is now level " + poke.level + "!";
            playerHUD.SetHUD(playerUnit, true, player, GameController.playerPokemon);
            yield return new WaitForSeconds(2);
            if (poke.learned_new_move)
            {
                if (playerUnit.pokemon.currentMoves.Count(s => s != null) == 4)
                {
                    dialogueText.text = poke.name + " is trying to learn " + poke.learned_move.name + "!";
                    yield return new WaitForSeconds(2);
                    dialogueText.text = "But they can only learn four moves. Forget which move?";
                    forgetMenuUI.SetActive(true);
                    forgetMenuOpen = true;
                    playerHUD.SetForgetMoves(playerUnit);
                    while (forgetMenuOpen)
                    {
                        yield return null;
                    }
                }
                else
                {
                    dialogueText.text = poke.name + " learned " + poke.learned_move.name + "!";
                    yield return new WaitForSeconds(2);
                    if (playerUnit.pokemon.currentMoves.Count(s => s != null) == 3) poke.currentMoves[3] = poke.learned_move;
                    else if (playerUnit.pokemon.currentMoves.Count(s => s != null) == 2) poke.currentMoves[2] = poke.learned_move;
                    else poke.currentMoves[1] = poke.learned_move;
                }
            }
        }

        public IEnumerator ForgetMove(int move, Pokemon poke)
        {
            forgetMenuUI.SetActive(false);
            if (move == 5)
            {
                dialogueText.text = poke.name + " won't learn " + poke.learned_move.name + "!";
                yield return new WaitForSeconds(2);
            }
            else if (move == 1)
            {
                dialogueText.text = poke.name + " forgot " + poke.currentMoves[0].name + " and learned " + poke.learned_move.name + "!";
                poke.currentMoves[0] = poke.learned_move;
                yield return new WaitForSeconds(2);
            }
            else if (move == 2)
            {
                dialogueText.text = poke.name + " forgot " + poke.currentMoves[1].name + " and learned " + poke.learned_move.name + "!";
                poke.currentMoves[1] = poke.learned_move;
                yield return new WaitForSeconds(2);
            }
            else if (move == 3)
            {
                dialogueText.text = poke.name + " forgot " + poke.currentMoves[2].name + " and learned " + poke.learned_move.name + "!";
                poke.currentMoves[2] = poke.learned_move;
                yield return new WaitForSeconds(2);
            }
            else if (move == 4)
            {
                dialogueText.text = poke.name + " forgot " + poke.currentMoves[3].name + " and learned " + poke.learned_move.name + "!";
                poke.currentMoves[3] = poke.learned_move;
                yield return new WaitForSeconds(2);
            }
            forgetMenuOpen = false;
            poke.learned_new_move = false;
        }

        #endregion leveling up
    }
}