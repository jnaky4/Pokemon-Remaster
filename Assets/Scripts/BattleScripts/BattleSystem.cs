using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, ONLYENEMYTURN, WON, LOST, RUNAWAY, CAUGHTPOKEMON, POKEMONFAINTED, ENDCOMBATPHASE }

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



        //[SerializeField] List<Sprite> AttackSprites;
        private List<Sprite> AttackSprites = new List<Sprite>();
        public String pathLeft;
        public String pathRight;
        public String pathBreak;
        public Sprite[] spritesLeft;
        public Sprite[] spritesRight;
        public Sprite[] spritesBreak;


        private SpriteAnimator PlayerAttackAnim;
        private SpriteAnimator EnemyAttackAnim;
        private bool playerAttack;

        //Used for dictating animation turn
        private bool playerInitialAttack;
        private bool enemyInitialAttack;

        private bool playerInitialStatus;
        private bool enemyInitialStatus;

        private bool playerStatus;
        private bool endofanimation;
        private bool enemyAttack;


        public bool isCaught;
        public int ballShakes;
        private bool enemyStatus;
        private bool beginCatch;
        private bool playCatch;
        private bool breakOut;
        private int breakOutFrame;
        private string playerMoveName;
        private string enemyMoveName;
        //all of this stuff is for animation

        public Unit playerUnit;
        public Unit enemyUnit;
        //The Pokemon used in the fight

        public Button attackButton;
        public Button runAwayButton;
        public Button pokemonButton;
        public Button ballsButton;

        public GameObject mainUI;
        public GameObject backUI;
        public Button backButton;
        //Our button used to attack

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;
        //The HUDs (the shit that shows our current hp and stuff like that

        public Text dialogueText;
        public Text levelUpText;
        public GameObject levelUpUI;
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

        public GameObject yesno;
        public Button yesButton;
        public Button noButton;

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

        public System.Random rnd = new System.Random();


      
        private int playerContinuingAttack = 0;
        private int enemyContinuingAttack = 0;
        private Moves playerMoveStorage;
        private Moves enemyMoveStorage;

        private int phasePlayerSprite = 0; //0 means no phasing, 1 means phase out, 2 means phase in
        private int phaseOpponentSprite = 0;

        private bool wantsToEvolve;

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
            PlayerAttackAnim = new SpriteAnimator(AttackSprites, playerAttackSprite, 0.04f);
            EnemyAttackAnim = new SpriteAnimator(AttackSprites, enemyAttackSprite, 0.04f);


            state = BattleState.START;
            pokeMenuUI.SetActive(false);
            attackMenuUI.SetActive(false);
            ballsMenuUI.SetActive(false);
            forgetMenuUI.SetActive(false);
            yesno.SetActive(false);
            backUI.SetActive(false);
            levelUpUI.SetActive(false);
            SetDownButtons();
            PokeballSpriteFactory();
            try
            {
                StartCoroutine(SetupBattle());
                SetBackground();
            }
            catch
            {
                dialogueText.text = "Error. Going back to main menu....";
                SceneManager.LoadSceneAsync("Start Menu");
            }

        }

        //logic for whether a player or opponent's attack animation plays
        /// <summary>
        /// TO DO: Get levi to comment this code
        /// </summary>
        private void Update()
        {
            if (GameController.inCombat)
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
            }

            if (beginCatch == true)
            {
                PlayerAttackAnim.Start();
                beginCatch = false;
                playCatch = true;
                MakeSpriteInvisible(enemySprite);
            }
            if (playCatch == true)
            {
                if (PlayerAttackAnim.CurrentFrame < PlayerAttackAnim.Frames.Count - 1)
                {
                    breakOut = state != BattleState.CAUGHTPOKEMON; 
                    if (breakOut == true && breakOutFrame == PlayerAttackAnim.CurrentFrame)
                        GameController.soundFX = "Break Out";
                    if (PlayerAttackAnim.CurrentFrame == 14 || PlayerAttackAnim.CurrentFrame == 45 || PlayerAttackAnim.CurrentFrame == 78)
                        GameController.soundFX = "Shake";
                    PlayerAttackAnim.HandleUpdate();
                }
                else
                {
                    //Debug.Log("End Animation now");
                    PlayerAttackAnim.EndAnimation();
                    playCatch = false;
                    endofanimation = true;
                    if (state == BattleState.CAUGHTPOKEMON)
                    {
                        DisplayPokeball();
                        GameController.soundFX = "Caught";
                    }
                    else MakeSpriteVisible(enemySprite);
                }
            }

            if (enemyInitialStatus == true)
            {
                PlayerAttackAnim.Start();
                enemyInitialStatus = false;
                enemyStatus = true;
            }
            if (enemyStatus == true)
            {
                if (PlayerAttackAnim.CurrentFrame < PlayerAttackAnim.Frames.Count - 1)
                {
                    PlayerAttackAnim.HandleUpdate();
                }
                else
                {
                    //Debug.Log("End Animation now");
                    PlayerAttackAnim.EndAnimation();
                    enemyStatus = false;
                    endofanimation = true;
                }
            }

            if (playerInitialStatus == true)
            {
                EnemyAttackAnim.Start();
                playerInitialStatus = false;
                playerStatus = true;
            }
            if (playerStatus == true)
            {
                if (EnemyAttackAnim.CurrentFrame < EnemyAttackAnim.Frames.Count - 1)
                {
                    EnemyAttackAnim.HandleUpdate();
                }
                else
                {
                    //Debug.Log("End Animation now");
                    EnemyAttackAnim.EndAnimation();
                    playerStatus = false;
                    endofanimation = true;
                }
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


            //grab initial player pokemon
            for (int i = 0; i < GameController.playerPokemon.Count(s => s != null); i++)
            {
                if (GameController.playerPokemon[i].current_hp > 0)
                {
                    activePokemon = i;
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
            //if (String.Compare(playerUnit.pokemon.currentMoves[0].name, "default") == 0) move1.SetActive(false);
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
            enemyHUD.SetUpEnemy();
            playerHUD.SetUpPlayer();

            SetPlayerTrainerSprite(playerSprite);
            SetBackground();

            if (GameController.isCatchable)
            {
                dialogueText.text = "A wild " + enemyUnit.pokemon.name + " appears!";
                SetOpponentSprite(enemyUnit, enemySprite);
                yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            }
            else
            {
                SetOpponentTrainerSprite(enemySprite);
                dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " wants to battle!";
                yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
                phaseOpponentSprite = 1;
                SetOpponentSprite(enemyUnit, enemySprite);
                phaseOpponentSprite = 2;
                dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " sends out " + enemyUnit.pokemon.name + "!";
            }
            phasePlayerSprite = 1;
            yield return new WaitForSeconds(2);
            dialogueText.text = "Go, " + playerUnit.pokemon.name + "!";
            SetPlayerSprite(playerUnit, playerSprite);
            phasePlayerSprite = 2;
            yield return new WaitForSeconds(2);
            
            if (GameController.isCatchable) GameController.catchRate = enemyUnit.pokemon.pokedex_entry.catch_rate;
            //Debug.Log("Catch rate: " + GameController.catchRate.ToString());



            state = BattleState.PLAYERTURN;
            PlayerMakesDecision();
        }
        #endregion Set up battle and update functions

        #region Decision functions to see who goes first

        /// <summary>
        /// This function is what we go to before anything happens.
        /// It lets us know what the player chooses to do and will call the appropriate functions from there.
        /// When in doubt, call this function.
        /// </summary>



        private void PlayerMakesDecision()
        {

            if(state == BattleState.POKEMONFAINTED)
            {
                state = BattleState.POKEMONFAINTED;
                SwapPokemonOnHUD();
            }


            if (playerUnit.pokemon.IsFainted())
            {


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
                //Debug.Log(playerUnit.pokemon.current_exp);
            }
        }

        public BattleState WhoGoesFirst(Moves playerMove, Moves enemyMove)
        {

            //priorities arent the same
            //1,0 or 0,1
            if (playerMove.priority != enemyMove.priority)
            {
                return playerMove.priority > enemyMove.priority ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;

            }
            //priorities are the same, do additional checks
            //1,1, or 0,0
            else
            {
                return playerUnit.pokemon.current_speed > enemyUnit.pokemon.current_speed ? BattleState.PLAYERTURN : BattleState.ENEMYTURN;
            }


        }


        /// <summary>
        /// Decides who attacks first, based on priority of the move, then the speed of each pokemon, then random
        /// </summary>
        /// <param name="playerMoveNum">The player move number. -1 if struggle, -2 if continuing attack.</param>
        /// <returns>Nothing</returns>
        private IEnumerator CombatPhase(int playerMoveNum)
        {

            Debug.Log("START <COMBAT PHASE>");

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
                    PlayerMakesDecision();
                    yield break;
                }
            }


            Debug.Log("MOVE NUMBER" + playerMoveNum);


            int moveNum = -1;
            bool struggle = false;
            if (enemyContinuingAttack == 0) struggle = Utility.EnemyStruggle(enemyUnit);
            Moves enemyMove, playerMove;
            if (playerMoveNum == -2) playerMove = playerMoveStorage;

            else if (playerMoveNum >= 0) playerMove = playerUnit.pokemon.currentMoves[playerMoveNum];
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
                    //Pick move for AI decision
                    //enemyUnit.pokemon.decide_move();
                    moveNum = rnd.Next(enemyUnit.pokemon.CountMoves());
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



            if (playerMoveNum >= 0) playerUnit.DoPP(playerMoveNum);
            
            if (moveNum >= 0) enemyUnit.DoPP(moveNum); //If it is not struggle, take down some PP.



            // is this for pokemon switches?
            if (playerMoveNum == -3) state = BattleState.ONLYENEMYTURN;
            else
            {
                state = WhoGoesFirst(playerMove, enemyMove);
            }
            


            
            switch (state)
            {

                case BattleState.PLAYERTURN:
                    yield return StartCoroutine(MultiAttackPerTurn(playerMove, playerUnit, enemyUnit));
                    //did a pokemon die
                    if (state == BattleState.POKEMONFAINTED) break;
                    state = BattleState.ENEMYTURN;
                    yield return StartCoroutine(MultiAttackPerTurn(enemyMove, enemyUnit, playerUnit));
                    break;
    
                case BattleState.ENEMYTURN:
                    yield return StartCoroutine(MultiAttackPerTurn(enemyMove, enemyUnit, playerUnit));
                    if (state == BattleState.POKEMONFAINTED) break;
                    state = BattleState.PLAYERTURN;
                    yield return StartCoroutine(MultiAttackPerTurn(playerMove, playerUnit, enemyUnit));
                    break;
  
                case BattleState.ONLYENEMYTURN:
                    yield return StartCoroutine(MultiAttackPerTurn(enemyMove, enemyUnit, playerUnit));
                    break;
                default:
                    break;
            }



            Debug.Log("END <COMBAT PHASE>");
            Debug.Log("START <END OF BOTH TURNS PHASE>");

            if (!playerUnit.pokemon.IsFainted())
            {
                yield return StartCoroutine(EndRoundStatusUpdate("Player", playerUnit, enemyUnit));
            }
            if (!enemyUnit.pokemon.IsFainted())
            {
                yield return StartCoroutine(EndRoundStatusUpdate("Enemy", enemyUnit, playerUnit));
            }



            //if one of the pokemon died
            if (state == BattleState.POKEMONFAINTED)
            {
                if (playerUnit.pokemon.IsFainted())
                {
                    //SWAP POKEMON
                    yield return StartCoroutine(SwapPokemonOnHUD());
                }  

                if (enemyUnit.pokemon.IsFainted())
                {
                    //Gain EXP
                    if(!playerUnit.pokemon.IsFainted()) yield return StartCoroutine(gainEXP());

                    dialogueText.text = enemyUnit.pokemon.name + " faints!";
                    //is the pokemon catachble? yes its wild, set exp_multiplier to 1, no? 1.5
                    yield return new WaitForSeconds(2);

                    for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                    {
                        if (GameController.opponentPokemon[j].current_hp > 0)
                        {
                            enemyUnit.pokemon = GameController.opponentPokemon[j];
                            //GameController.soundFX = GameController.opponentPokemon[j].dexnum.ToString();
                            dialogueText.text = GameController.opponentType + " " + GameController.opponentName + " sent out a " + enemyUnit.pokemon.name + "!";

                            yield return new WaitForSeconds(0.75f);
                            GameController.soundFX = enemyUnit.pokemon.dexnum.ToString();
                            enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);
                            SetOpponentSprite(enemyUnit, enemySprite);
                            phaseOpponentSprite = 2;
                            break;
                        }
                    }

                }
                //neither dead, do status effects
            }



            state = BattleState.PLAYERTURN;
            PlayerMakesDecision();

        }


        #endregion Decision functions to see who goes first

        #region Attack functions

        public IEnumerator MultiAttackPerTurn(Moves Attack, Unit Attacking, Unit Defending)
        {
            //number of moves per turn the player/enemy can attack
            int NumTimesAttack = rnd.Next(Attack.min_per_turn, Attack.max_per_turn + 1);

            for (int k = 0; k < NumTimesAttack; k++)
            {
                yield return StartCoroutine(AttackXYZ(Attack, Attacking, Defending));

                if (state == BattleState.POKEMONFAINTED) yield break;

            }

        }

        public void UpdateDialogueForDamageAndStatus(Moves attack, Unit Attacker, Unit Defender, System.Random rnd, bool crit, double effectivenessMultiplier)
        {

            bool appliesStatus = attack.status.RollToApplyStatus(attack);
            // handle damage text
            if (attack.base_power != -1)//If this move is a damage dealing move.
            {
                switch (effectivenessMultiplier)
                {
                    case double s when (s > 1):
                        {
                            GameController.soundFX = "Super Effective";
                            dialogueText.text = "It's super effective!";
                            break;
                        }
                    case double s when ((s < 1) && (s != 0)):
                        {
                            GameController.soundFX = "Not Very Effective";
                            dialogueText.text = "It's not very effective...";
                            break;
                        }
                    case 0:
                        {
                            // todo this function is just a stub
                            if (Utility.IsImmune(attack, Defender))
                            {
                                dialogueText.text = Defender.pokemon.name + " is immune!";
                            }
                            break;
                        }
                    case 1:
                    default:
                        {
                            Debug.Log("dialogueText" + dialogueText.text);
                            Debug.Log("defender" + Defender.pokemon.name);
                            GameController.soundFX = "Damage";
                            if (state == BattleState.PLAYERTURN) dialogueText.text = "Enemy " + Defender.pokemon.name + " took damage...";
                            else dialogueText.text = "Your " + Defender.pokemon.name + " took damage...";
                        }
                        break;
                }
            }

            // todo split into new function. add delay. status information, maybe this comes like a second after damage information?
            // todo split tests in UnitTests if this method is split
            if (appliesStatus)
            {
           
                if (Defender.pokemon.statuses.Contains(attack.status.name))
                {
                    dialogueText.text = "Enemy " + Defender.pokemon.name + " is already " + attack.status.adj + "!";
                }
                // todo IsImmune is a stub, and needs to implemented, return false currently
                else if (Utility.IsImmune(attack, Defender))
                {
                    dialogueText.text = Defender.pokemon.name + " is immune!";
                }
                else
                {
                    dialogueText.text = "Enemy " + Defender.pokemon.name + " became " + attack.status.adj + "!";
                    enemyHUD.SetStatus(Defender.pokemon);
                }
            }
        }

        /// <summary>
        /// Determines if the Player's attack hits or not, and then does all of the damage calculations and crits and all that.
        /// </summary>
        /// <param name="attack">The move we are attacking with.</param>
        /// <returns>Nothing</returns>
        private IEnumerator AttackXYZ(Moves attack, Unit Attacker, Unit Defender)
        {
            if (state == BattleState.PLAYERTURN)
            {
                ClosePokemonMenu();
                CloseMovesMenu();
                CloseBallsMenu();
            }

            SetDownButtons();      

            //BEGIN TURN STATUS UPDATE
            yield return StartCoroutine(BeginTurnStatusUpdate(Attacker));

            //ABLE TO ATTACK
            //checks all statuses if able to attack, if unable to attack, displays animation
            //if unable, stores the attack that affected them in: pokemon.UnableToAttackStatusName
            yield return StartCoroutine(AbleToAttack(Attacker));

            bool crit = Utility.CriticalHit(Attacker);
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);

            dialogueText.text = Attacker.pokemon.name + " used " + attack.name + "!";
            yield return new WaitForSeconds(0.75f);

            //If the attack hits
            if (num <= (attack.accuracy * Attacker.pokemon.current_accuracy * Defender.pokemon.current_evasion))
            {

                if(state == BattleState.PLAYERTURN)
                {
                    playerInitialAttack = true;
                }
                else
                {
                    enemyInitialAttack = true;
                }
                    
                if (attack.name == "Growl") GameController.soundFX = Attacker.pokemon.dexnum.ToString();
                else GameController.soundFX = attack.name;
                while (!endofanimation)
                {
                    yield return null;
                }
                endofanimation = false;

                if (attack.current_stat_change.CompareTo("null") != 0) Attacker.SetStages(attack, Defender);
                if (!attack.status.Equals("null")) Status.Apply_Attack_Status_Effect(attack, Defender);
                double super = Utility.DoDamage(Attacker, Defender, attack, crit);
                //Debug.Log(playerUnit.damage);

                bool isDead = Defender.TakeDamage(Attacker.damage);
                if (attack.heal > 0) Attacker.TakeDamage(-Attacker.damage * attack.heal);
                if (attack.heal < 0) Attacker.TakeDamage(Attacker.damage * -attack.heal);

                if(state == BattleState.PLAYERTURN)
                {
                    playerHUD.SetHP(Attacker.pokemon.current_hp, Attacker, "player");
                    if (attack.base_power <= 0) StartCoroutine(ShakeLeftRight());
                    else StartCoroutine(Blink(enemySprite, 0.25));
                    enemyHUD.SetHP(Defender.pokemon.current_hp, Attacker, "player");
                    //playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
                    enemyHUD.SetStatus(Defender.pokemon);
                }
                else
                {
                    enemyHUD.SetHP(enemyUnit.pokemon.current_hp, Defender, "enemy");
                    if (attack.base_power <= 0) StartCoroutine(ShakeLeftRight());
                    else StartCoroutine(Blink(playerSprite, 0.25));
                    playerHUD.SetHP(Defender.pokemon.current_hp, Defender, "enemy");
                }

                if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("enemy") == 0)
                {
                    dialogueText.text = "Enemy " + Defender.pokemon.name + "'s " + attack.current_stat_change + " fell!"; //If you lower their stat
                    yield return new WaitForSeconds(1);
                }
                else if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("self") == 0)
                {
                    dialogueText.text = "Your " + Attacker.pokemon.name + "'s " + attack.current_stat_change + " rose!"; //If you increase your own stat
                    yield return new WaitForSeconds(1);
                }
                if ((attack.base_power != -1) && (crit)) yield return new WaitForSeconds(0.75f);
                UpdateDialogueForDamageAndStatus(attack, Attacker, Defender, rnd, crit, super);
                yield return new WaitForSeconds(2);
            }
            else //If your attack missed
            {

                dialogueText.text = "Your attack missed!";
                yield return new WaitForSeconds(2);
            }

            yield return StartCoroutine(IsEitherPokemonDead());
        }

        public IEnumerator gainEXP()
        {
            if (enemyUnit.pokemon.IsFainted() && !playerUnit.pokemon.IsFainted())
            {
                int exp = 0;
                double exp_multiplier;
                exp_multiplier = (GameController.isCatchable) ? 1 : 1.5;
                exp = playerUnit.pokemon.gain_exp(enemyUnit.pokemon.level, enemyUnit.pokemon.pokedex_entry.base_exp, 1, exp_multiplier);
                /*                    string test = exp_multiplier == 1 ? "pokemon is wild" : "pokemon is a trainers";
                                    Debug.Log(test);*/

                dialogueText.text = playerUnit.pokemon.name + " gained " + exp + " EXP!";
                playerHUD.SetEXP(playerUnit.pokemon, exp);
                yield return new WaitForSeconds(2);
                if (playerUnit.pokemon.gained_a_level)
                {
                    yield return StartCoroutine(LevelUp(playerUnit.pokemon));
                }
            }

        }

        public IEnumerator BeginTurnStatusUpdate(Unit Player)
        {
            string removed_status = "none";
            //start_turn_statuses returns "" if no removed statuses
            while ((removed_status = Player.pokemon.StartTurnStatusUpdate()) != "")
            {
                switch (removed_status)
                {
                    case "Sleep":
                        dialogueText.text = Player.pokemon.name + " woke up!";
                        yield return new WaitForSeconds(2);
                        break;
                    case "Freeze":
                        dialogueText.text = Player.pokemon.name + " thawed out!";
                        yield return new WaitForSeconds(2);
                        break;
                    case "Confusion":
                        dialogueText.text = Player.pokemon.name + " is no longer confused!";
                        yield return new WaitForSeconds(2);
                        break;
                }
            }
        }

        //END TURN STATUS UPDATE
        //looks for statuses that apply at end of turn
        //Seeded, Burn, Poison
        public IEnumerator EndRoundStatusUpdate(string whosattacking, Unit AttackingPlayer, Unit DefendingPlayer)
        {
            //AnimateStatus required boolean, true to animate on the player
            bool animate_on_player = whosattacking == "player" ? true : false;
            
            //if dead no need to do status affects
            if (!AttackingPlayer.pokemon.IsFainted())
            {
                foreach (Status status in AttackingPlayer.pokemon.statuses)
                {
                    //if status doesnt do damage skip
                    if(status.self_damage > 0)
                    {
                        AnimateStatus(status.name, animate_on_player);

                        GameController.soundFX = status.name;
                        while (!endofanimation)
                        {
                            yield return null;
                        }
                        endofanimation = false;

                        //Take Damage: unit has Seeded on them
                        AttackingPlayer.TakeDamage(AttackingPlayer.pokemon.max_hp * status.self_damage);

                        if (whosattacking == "player")
                        {
                            StartCoroutine(Blink(playerSprite, 0.25));
                            playerHUD.SetHP(AttackingPlayer.pokemon.current_hp, AttackingPlayer, whosattacking);
                        }
                        else
                        {
                            StartCoroutine(Blink(enemySprite, 0.25));
                            enemyHUD.SetHP(AttackingPlayer.pokemon.current_hp, playerUnit, whosattacking);
                        }
                        //code for specific moves
                        switch (status.name)
                        {
                            case "Seeded":
                                //Heal Damage: oponnent pokemon heals damage
                                DefendingPlayer.TakeDamage(-AttackingPlayer.pokemon.max_hp * status.self_damage);
                                //update hud with damage
                                if (whosattacking == "player") enemyHUD.SetHP(DefendingPlayer.pokemon.current_hp, AttackingPlayer, whosattacking);
                                else playerHUD.SetHP(DefendingPlayer.pokemon.current_hp, DefendingPlayer, whosattacking);

                                dialogueText.text = AttackingPlayer.pokemon.name + " got leeched by " + DefendingPlayer.pokemon.name + "!";
                                break;
                            case "Burn":
                                dialogueText.text = AttackingPlayer.pokemon.name + " got " + status.adj + "!";
                                break;
                            case "Posion":
                                dialogueText.text = AttackingPlayer.pokemon.name + " is " + status.adj + "!";
                                break;
                        }
                        
                        yield return new WaitForSeconds(2);
                    }

                }
            }
            //decrements all counters
            AttackingPlayer.pokemon.EndTurnStatusUpdate();                         

        }
        /// <summary> 
        /// checks all statuses if able to attack, if unable to attack sets global bool unableToAttack, animates status effect.
        /// </summary>
        /// <param name="Player">Player object that stores pokemon</param>
        /// <param name="whosattacking">specifies which player to check statuses for able to attack</param>
        /// if unable, stores the attack that affected them in: pokemon.UnableToAttackStatusName
        /// should be TriesToAttack
        public IEnumerator AbleToAttack(Unit Player)
        {
            bool animate_on_player = state == BattleState.PLAYERTURN;
            if (!Player.pokemon.CheckAbleAttack())
            {
                //Debug.Log("Unable to attack becasue of :" + playerUnit.pokemon.UnableToAttackStatusName);
                switch (Player.pokemon.UnableToAttackStatusName)
                {

                    case "Paralysis":
                        //Todo change for all other cases to be the same
                        AnimateStatus(Player.pokemon.UnableToAttackStatusName, animate_on_player);
                        GameController.soundFX = Player.pokemon.UnableToAttackStatusName;
                        while (!endofanimation)
                        {
                            yield return null;
                        }
                        endofanimation = false;
                        dialogueText.text = Player.pokemon.name + " is " + Player.pokemon.UnableToAttackStatusName + "!";
                        yield return new WaitForSeconds(2);
                        break;

                    case "Sleep":
                        AnimateStatus("Sleep", animate_on_player);
                        //TODO change sound name to Sleep
                        GameController.soundFX = "Sleeping";
                        while (!endofanimation)
                        {
                            yield return null;
                        }

                        dialogueText.text = Player.pokemon.name + " is asleep!";
                        yield return new WaitForSeconds(2);
                        break;

                    case "Freeze":
                        //TODO Add Animation
                        dialogueText.text = Player.pokemon.name + " is frozen!";

                        yield return new WaitForSeconds(2);
                        break;

                    default:
                        break;
                }
                
            }
            //reset name
            Player.pokemon.UnableToAttackStatusName = "";
           
        }

        /// <summary>
        /// checks if either pokemon is dead and changes battlestate to change pokemon
        /// </summary>
        /// <returns></returns>
        public IEnumerator IsEitherPokemonDead()
        {
            if (playerUnit.pokemon.IsFainted() || enemyUnit.pokemon.IsFainted())
            {
                state = BattleState.POKEMONFAINTED;
                for (int i = 0; i < GameController.playerPokemon.Count(s => s != null); i++)
                { 
                    if (GameController.playerPokemon[i].IsFainted())
                    {
                        playerHUD.CrossOutPlayerBall(i + 1);
                    }
                }
                for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                {
                    if (GameController.opponentPokemon[j].IsFainted())
                    {
                        playerHUD.CrossOutEnemyBall(j + 1);
                    }
                }
            }
            yield return StartCoroutine(SeeIfEndBattle());
        }

        public void DetermineCatchResult(int ball, Unit enemyUnit, int rnd)
        {
            Double statusModifier = 1.0;
            foreach (String status in enemyUnit.pokemon.statuses)
            {
                switch (status)
                {
                    case "Sleep":
                    case "Freeze":
                        statusModifier = 1.3;
                        break;
                    case "Paralysis":
                    case "Poison":
                    case "Burn":
                        statusModifier = 1.15;
                        break;
                    default:
                        break;
                } 
            }
            Double ballModifier = 0.0;
            switch (ball)
            {
                case 1: // pokeball
                    ballModifier = 0.5;
                    break;
                case 2: // greatball
                    ballModifier = 0.75;
                    break;
                case 3: // ultraball
                    ballModifier = 1.0;
                    break;
                case 4: // masterball
                    ballModifier = 256.0;
                    break;

            }

            // (f/256) = probability of catching the pokemon 
            Double currentHP = enemyUnit.pokemon.current_hp;
            Double maxHP = enemyUnit.pokemon.max_hp;
            // todo add field on pokemon called catchRate
            Double catchRate = 1.0;
            Double f = 256 * ballModifier * (1 - 0.5 * currentHP / maxHP) * catchRate * statusModifier;
            // if f > 255, set it to 255
            f = (f > 255) ? 255 : f;
            Debug.Log("f: " + f);
            ballShakes = (int)Math.Floor(3 * (256 - rnd) / (256 - f)) + 1;
            Debug.Log("catch f:" + f + " rnd: " + rnd + " shakes halfway calc:" +ballShakes);
            ballShakes = (ballShakes > 3) ? 3 : ballShakes;
            state = (rnd < f) ? BattleState.CAUGHTPOKEMON : BattleState.ONLYENEMYTURN;
            
        }

        public IEnumerator HandleNotEnoughPokeBalls(int typeOfPokeball)
        {
            switch(typeOfPokeball)
            {
                case 1:
                    dialogueText.text = "You don't have enough Poke Balls!";
                    break;
                case 2:
                    dialogueText.text = "You don't have enough Great Balls!";
                    break;
                case 3:
                    dialogueText.text = "You don't have enough Ultra Balls!";
                    break;
                case 4:
                    dialogueText.text = "You don't have enough Master Balls!";
                    break;
            }
            yield return new WaitForSeconds(2);
            PlayerMakesDecision();
            yield break;
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
                PlayerMakesDecision();
                yield break;
            }
            dialogueText.text = "";

            // set dialogue if used ball and has ball in inventory
            // otherwise break and have player do turn again
            if ((typeOfPokeball == 1) && (player.numPokeBalls > 0)) //If you have a Poke Ball
            {
                player.numPokeBalls--;
                dialogueText.text = "Used Poke Ball!";
            }
            else if ((typeOfPokeball == 2) && (player.numGreatBalls > 0))
            {
                player.numGreatBalls--;
                dialogueText.text = "Used Great Ball!";
            }
            else if ((typeOfPokeball == 3) && (player.numUltraBalls > 0))
            {
                player.numUltraBalls--;
                dialogueText.text = "Used Ultra Ball!";
            }
            else if ((typeOfPokeball == 4) && (player.numMasterBalls > 0))
            {
                player.numMasterBalls--;
                dialogueText.text = "Used Master Ball!";
            }
            else
            {
                yield return StartCoroutine(HandleNotEnoughPokeBalls(typeOfPokeball));
                yield break;
            }

            // catch chance works out to 25% for a full health pokemon with a pokeball
            // up to 3 shakes, it is possible to have 3 shakes and catch / not catch pokemon
            // shakes are scaled so that if the roll is far away from the catch threshold it is 1 shake
            // if the roll is very close to the catch threshold it is 3 shakes
            int catchRoll = (int) rnd.Next(256);
            DetermineCatchResult(typeOfPokeball, enemyUnit, catchRoll);
            PokeballShakes(ballShakes); //beginCatch set to true
            yield return new WaitUntil(() => (beginCatch == false && playCatch == false));
            if (state == BattleState.CAUGHTPOKEMON)
            {
                dialogueText.text = enemyUnit.pokemon.name + " was caught!";
                DisplayPokeball();
                yield return new WaitForSeconds(2);
                yield return StartCoroutine(EndBattle());
                yield break;
            }
            else if (state == BattleState.ONLYENEMYTURN)
            {
                dialogueText.text = enemyUnit.pokemon.name + " will not submit to your authority!";
                yield return new WaitForSeconds(2);
                yield return StartCoroutine(CombatPhase(-3));
            }
        }

        /// <summary>
        /// Switches the player's active Pokemon.
        /// </summary>
        /// <param name="num">The index of the player's Pokemon in the array in GameController.</param>
        /// <returns>Nothing.</returns>
        private IEnumerator TryToSwitchPokemon(int num)
        {
            if (GameController.playerPokemon[num].IsFainted())
            {
                dialogueText.text = "That Pokemon has no HP remaining!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.POKEMONFAINTED) PlayerMakesDecision();
                else yield return SwapPokemonOnHUD();
                yield break;
            }
            else if (activePokemon == num) //If you try to swap out the current active Pokemon.
            {
                dialogueText.text = "You already have that Pokemon out!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.POKEMONFAINTED) PlayerMakesDecision();
                else yield return SwapPokemonOnHUD();
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
                GameController.soundFX = playerUnit.pokemon.dexnum.ToString();
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
                if (state == BattleState.POKEMONFAINTED)
                {
                    Debug.Log("CHANGE POKEMON 1482");
                    state = BattleState.PLAYERTURN;
                    PlayerMakesDecision();
                }
                else
                {
                    state = BattleState.ONLYENEMYTURN;
                    StartCoroutine(CombatPhase(-3));
                }
            }
            yield break;
        }


        /// <summary>
        /// Actuall Swaps the Pokemon on
        /// </summary>
        /// <returns></returns>
        private IEnumerator SwapPokemonOnHUD()
        {
            state = BattleState.POKEMONFAINTED;
            playerHUD.SetPokemon(GameController.playerPokemon);
            SetDownButtons();
            OpenPokemonMenu();
            yield break;
        }



        /// <summary>
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
            GameController.update_level_cap();

            SetDownButtons();
            if (state == BattleState.WON) //If you won
            {
                yield return StartCoroutine(gainEXP());
                //Check Evolve
                for (int i = 0; i < GameController.playerPokemon.Count(s => s != null); i++)
                {
                    yield return StartCoroutine(Evolve(GameController.playerPokemon[i], i));
                }

                if (GameController.isCatchable)
                {
                    GameController.music = "Battle Victory Wild";
                    dialogueText.text = player.myName + " won!";
                }
                else
                {
                    if (GameController.opponentName == "Brock" || GameController.opponentName == "Gary")
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
                    yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
                    dialogueText.text = player.myName + " got $" + GameController.winMoney + " for winning!";
                    GameController.player.money += GameController.winMoney;
                }
            }
            else if (state == BattleState.LOST) //If you lost
            {
                dialogueText.text = player.myName + " lost! You blacked out!";
                yield return new WaitForSeconds(2);
                dialogueText.text = "Going back to the start menu....";
                yield return new WaitForSeconds(2);
                GameController.endCombat = true; //Something for levi.a
                Destroy(playerSprite);
                Destroy(enemySprite);
                Destroy(GameObject.Find("Sound Controller"));
                SceneManager.LoadSceneAsync("Start Menu");
            }
            else if (state == BattleState.RUNAWAY) //If you ran away.
            {
                dialogueText.text = "Got away safely...";
            }
            else if (state == BattleState.CAUGHTPOKEMON) //If you were successful in catching the pokemon
            {
                DisplayPokeball();
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
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));

            GameController.playerPokemon[activePokemon] = playerUnit.pokemon; //Places your active pokemon back in the array - this is so its stats gets updated.
            for (int i = 0; i < 6; i++) //Resets the stages and attacks of all of your pokemon.
            {
                if (GameController.playerPokemon[i] != null)
                {
                    GameController.playerPokemon[i].reset_battle_stats();
/*                    GameController.playerPokemon[i].critical_stage = 0;
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
                    else GameController.playerPokemon[i].current_speed = (int)(GameController.playerPokemon[i].max_speed * 0.5);
                    GameController.playerPokemon[i].current_accuracy = 1;
                    GameController.playerPokemon[i].current_evasion = 1;*/
                    GameController.player.pokeBalls = player.numPokeBalls;
                    GameController.player.greatBalls = player.numGreatBalls;
                    GameController.player.ultraBalls = player.numUltraBalls;
                    GameController.player.masterBalls = player.numMasterBalls;
                    if (GameController.playerPokemon[i].statuses.Contains(Status.get_status("Seeded"))) GameController.playerPokemon[i].statuses.Remove(Status.get_status("Seeded"));
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

        public void OnBackButton()
        {
            if (state != BattleState.PLAYERTURN) return;
            CloseBallsMenu();
            CloseMovesMenu();
            ClosePokemonMenu();
            backUI.SetActive(false);
            mainUI.SetActive(true);
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
                PlayerMakesDecision();
                yield break;
            }
            else
            {
                CloseBallsMenu();
                CloseMovesMenu();
                ClosePokemonMenu();
                SetDownButtons();
                if (playerUnit.pokemon.current_speed > enemyUnit.pokemon.current_speed)
                {
                    state = BattleState.RUNAWAY;
                    StartCoroutine(EndBattle());
                    yield break;
                }
                double a = playerUnit.pokemon.current_speed;
                double b = a / 4;
                b %= 256;
                if (b == 0)
                {
                    state = BattleState.RUNAWAY;
                    StartCoroutine(EndBattle());
                    yield break;
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
            mainUI.SetActive(true);
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
            backUI.SetActive(false);
            mainUI.SetActive(false);
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
            backUI.SetActive(true);
            mainUI.SetActive(false);
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
            StartCoroutine(TryToSwitchPokemon(0));
        }

        /// <summary>
        /// Called when [pokemon1] button is pressed. Brings out the second pokemon in the player's array.
        /// </summary>
        public void OnPokemon1()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(TryToSwitchPokemon(1));
        }

        /// <summary>
        /// Called when [pokemon2] button is pressed. Brings out the third pokemon in the player's array.
        /// </summary>
        public void OnPokemon2()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(TryToSwitchPokemon(2));
        }

        /// <summary>
        /// Called when [pokemon3] button is pressed. Brings out the fourth pokemon in the player's array.
        /// </summary>
        public void OnPokemon3()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(TryToSwitchPokemon(3));
        }

        /// <summary>
        /// Called when [pokemon4] button is pressed. Brings out the fifth pokemon in the player's array.
        /// </summary>
        public void OnPokemon4()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(TryToSwitchPokemon(4));
        }

        /// <summary>
        /// Called when [pokemon5] button is pressed. Brings out the sixth pokemon in the player's array.
        /// </summary>
        public void OnPokemon5()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(TryToSwitchPokemon(5));
        }

        /// <summary>
        /// Opens the moves menu. If you have no remaining PP left, clicking on the moves menu will automatically select struggle and advance to the decision function.
        /// </summary>
        public void OpenMovesMenu()
        {
            if (playerContinuingAttack != 0)
            {
                StartCoroutine(CombatPhase(-2));
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
                StartCoroutine(CombatPhase(-1));
                return;
            }
            attackMenuUI.SetActive(true);
            backUI.SetActive(true);
            mainUI.SetActive(false);
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
            backUI.SetActive(true);
            mainUI.SetActive(false);
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
            StartCoroutine(CombatPhase(0));
        }

        /// <summary>
        /// Selects the second attack. bottom left.
        /// </summary>
        public void Attack2()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(CombatPhase(1));
        }

        /// <summary>
        /// Selects the third attack. bottom right.
        /// </summary>
        public void Attack3()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(CombatPhase(2));
        }

        /// <summary>
        /// Selects the fourth attack. Top right.
        /// </summary>
        public void Attack4()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(CombatPhase(3));
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

        public void Yes()
        {
            wantsToEvolve = true;
            yesno.SetActive(false);
        }

        public void No()
        {
            wantsToEvolve = false;
            yesno.SetActive(false);
        }

        public IEnumerator FailRunAway()
        {
            dialogueText.text = "Can't Escape!";
            yield return new WaitForSeconds(2);
            StartCoroutine(CombatPhase(-3));
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
            if (GameController.location == "Viridian Forest")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Route 22")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Route 2")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Gym")
            {
                x = 0.00f;
                y = 0.45f;
            }
            spriteRenderer.sprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.SmoothStep(255, 0, 2.0f));
        }

        private void SetPlayerSprite(Unit unit, SpriteRenderer sprite)
        {
            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 0.25f;
                y = 0.20f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.00f;
                y = 0.50f;
            }
            if (GameController.location == "Viridian Forest")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Route 22")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Route 2")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Gym")
            {
                x = 0.00f;
                y = 0.45f;
            }

            var s = Resources.Load<Sprite>("Images/PokemonImages/" + (playerUnit.pokemon.dexnum).ToString().PadLeft(3, '0') + playerUnit.pokemon.name);
            sprite.sprite = Sprite.Create(s.texture, s.rect, new Vector2(x, y));

            GameController.soundFX = unit.pokemon.dexnum.ToString();
        }

        private void SetOpponentTrainerSprite(SpriteRenderer spriteRenderer)
        {
            string type = GameController.opponentType;
            string path;

            if (type == "Rival")
                path = "Player_Rival/Blue";
            else if (type != "Gym Leader")
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
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.opponentType == "Rival")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Viridian Forest")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Route 22")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Route 2")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Gym")
            {
                x = 0.00f;
                y = 0.45f;
            }
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
            if (GameController.opponentType == "Rival")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Viridian Forest")
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Route 22")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Route 2")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Gym")
            {
                x = 0.00f;
                y = 0.45f;
            }

            var s = Resources.Load<Sprite>("Images/PokemonImages/" + (enemyUnit.pokemon.dexnum).ToString().PadLeft(3, '0') + enemyUnit.pokemon.name + "2");
            sprite.sprite = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
            GameController.soundFX = unit.pokemon.dexnum.ToString();
        }

        public void SetBackground()
        {
            string type = GameController.opponentType;

            var path = "Images/Backgrounds/";

            background.sprite = Resources.Load<Sprite>(path + GameController.location);
        }

        public void DisplayPokeball()
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
            if (GameController.location.CompareTo("Viridian Forest") == 0)
            {
                x = 0.00f;
                y = 0.35f;
            }
            if (GameController.location == "Route 22")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Route 2")
            {
                x = 0.00f;
                y = 0.45f;
            }
            if (GameController.location == "Gym")
            {
                x = 0.00f;
                y = 0.45f;
            }

            var s = Resources.Load<Sprite>("Attack_Animations/Pokeball_Left/Pokeball_Left_000");
            enemySprite.sprite = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
            enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1);
        }

        //isPlayer sets position of animation to player
        public void AnimateStatus(string status, bool isPlayer)
        {
            string path = "Attack_Animations/" + status;
            AttackSprites.Clear();
            AttackSprites.Add(null);
            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0 && isPlayer)
            {
                x = 0.00f;
                y = 0.40f;
            }
            if (GameController.location.CompareTo("Route 1") == 0 && !isPlayer)
            {
                x = 1.40f;
                y = 1.125f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0 && isPlayer)
            {
                x = -0.25f;
                y = 0.65f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0 && !isPlayer)
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Viridian Forest" && isPlayer)
            {
                x = -0.25f;
                y = 0.50f;
            }
            if (GameController.location == "Viridian Forest" && !isPlayer)
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Route 22" && isPlayer)
            {
                x = 1.25f;
                y = 1.175f;
            }
            if (GameController.location == "Route 22" && !isPlayer)
            {
                x = 0.00f;
                y = 0.65f;
            }
            if (GameController.location == "Route 2" && isPlayer)
            {
                x = 2.75f;
                y = 2.025f;
            }
            if (GameController.location == "Route 2" && !isPlayer)
            {
                x = 1.50f;
                y = 1.50f;
            }
            if (GameController.location == "Gym" && isPlayer)
            {
                x = 1.25f;
                y = 1.175f;
            }
            if (GameController.location == "Gym" && !isPlayer)
            {
                x = 0.00f;
                y = 0.65f;
            }

            var sprites = Resources.LoadAll<Sprite>(path);
            foreach (var sprite in sprites)
            {
                Sprite s = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
                AttackSprites.Add(s);
            }

            //AttackSprites.AddRange(sprites);
            AttackSprites.TrimExcess();

            if (isPlayer)
                playerInitialStatus = true;
            else
                enemyInitialStatus = true;
        }

        public void PokeballSpriteFactory()
        {
            pathLeft = "Attack_Animations/Pokeball_Left";
            pathRight = "Attack_Animations/Pokeball_Right";
            pathBreak = "Attack_Animations/Pokeball_Break";
            spritesLeft =  Resources.LoadAll<Sprite>(pathLeft);
            spritesBreak =  Resources.LoadAll<Sprite>(pathBreak);
            spritesRight = Resources.LoadAll<Sprite>(pathRight);
        }

        public void PokeballShakes(int shakes)
        {
            Debug.Log("Shakes: " + shakes.ToString());
            AttackSprites.Clear();
            AttackSprites.Add(null);

            float x = 0, y = 0;
            if (GameController.location.CompareTo("Route 1") == 0)
            {
                x = 1.24f;
                y = 1.125f;
            }
            if (GameController.location.CompareTo("Pallet Town") == 0)
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Viridian Forest")
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Route 22")
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Route 2")
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Gym")
            {
                x = 1.24f;
                y = 1.175f;
            }

            if (shakes == 0)
            {
                Sprite sprite = spritesLeft[0];
                Sprite p = Sprite.Create(sprite.texture, sprite.rect, new Vector2(x, y));
                AttackSprites.Add(p);
            }
            if (shakes >= 1)
            {
                foreach (var s in spritesLeft)
                {
                    Sprite p = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
                    AttackSprites.Add(p);
                }
            }
            if (shakes >= 2)
            {
                foreach (var s in spritesRight)
                {
                    Sprite p = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
                    AttackSprites.Add(p);
                }
            }
            if (shakes >= 3)
            {
                foreach (var s in spritesLeft)
                {
                    Sprite p = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
                    AttackSprites.Add(p);
                }
            }
            if (state == BattleState.ONLYENEMYTURN)
            {
                breakOutFrame = AttackSprites.Count(s => s != null);
                foreach (var s in spritesBreak)
                {
                    Sprite p = Sprite.Create(s.texture, s.rect, new Vector2(x, y));
                    AttackSprites.Add(p);
                }
                breakOut = true;
            }
            else
            {
                breakOut = false;
            }

            AttackSprites.TrimExcess();
            beginCatch = true;
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
                x = 1.40f;
                y = 1.125f;
            }
            if (GameController.location.CompareTo("Route 1") == 0 && !isPlayer)
            {
                x = 0.00f;
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
            if (GameController.location == "Viridian Forest" && isPlayer)
            {
                x = 1.24f;
                y = 1.175f;
            }
            if (GameController.location == "Viridian Forest" && !isPlayer)
            {
                x = -0.25f;
                y = 0.50f;
            }
            if (GameController.location == "Route 22" && isPlayer)
            {
                x = 1.25f;
                y = 1.175f;
            }
            if (GameController.location == "Route 22" && !isPlayer)
            {
                x = 0.00f;
                y = 0.65f;
            }
            if (GameController.location == "Route 2" && isPlayer)
            {
                x = 1.25f;
                y = 1.175f;
            }
            if (GameController.location == "Route 2" && !isPlayer)
            {
                x = 0.00f;
                y = 0.65f;
            }
            if (GameController.location == "Gym" && isPlayer)
            {
                x = 1.25f;
                y = 1.175f;
            }
            if (GameController.location == "Gym" && !isPlayer)
            {
                x = 0.00f;
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
                //if (i % 2 == 0) ShakeLeft(pos);
                //else ShakeRight(pos);
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

        public void MakeSpriteInvisible(SpriteRenderer sprite)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        }

        public void MakeSpriteVisible(SpriteRenderer sprite)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
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

        public IEnumerator ShakeLeftRight()
        {
            var pos = camera.transform.position;
            float x = 0.25f / 2f;
            var numTimes = 2;
            for (int i = 0; i < numTimes * 2; i++)
            {
                if (i % 2 == 0) ShakeLeft(pos);
                else ShakeRight(pos);
                yield return new WaitForSeconds((float)x);
            }
            StopShake(pos);
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
                    mainUI.SetActive(false);
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
                    Debug.Log("Name of move: " + poke.learned_move.name);
                    if (playerUnit.pokemon.currentMoves.Count(s => s != null) == 3) poke.currentMoves[3] = poke.learned_move;
                    else if (playerUnit.pokemon.currentMoves.Count(s => s != null) == 2) poke.currentMoves[2] = poke.learned_move;
                    else poke.currentMoves[1] = poke.learned_move;
                    playerHUD.SetMoves(playerUnit);
                    for (int i = 0; i < playerUnit.pokemon.currentMoves.Count(s => s != null); i++)
                    {
                        Debug.Log("Move " + i + ": " + playerUnit.pokemon.currentMoves[i].name);
                    }
                    yield return new WaitForSeconds(2);
                    poke.learned_new_move = false;
                }
                mainUI.SetActive(true);
            }
            levelUpText.text = "\n   " + playerUnit.pokemon.name + " Stats: \n";
            dialogueText.text = "";
            //this.image1 = path + (this.dexnum).ToString().PadLeft(3, '0') + this.name + ".png";
            //this.image2 = path + (this.dexnum).ToString().PadLeft(3, '0') + this.name + "2.png";
            levelUpText.text += " HP:".PadRight(30) + "+" + playerUnit.pokemon.change_hp + "\n";
            levelUpText.text += " ATK:".PadRight(29) + "+" + playerUnit.pokemon.change_attack + "\n";
            levelUpText.text += " DFN:".PadRight(29) + "+" + playerUnit.pokemon.change_defense + "\n";
            levelUpText.text += " SPD:".PadRight(29) + "+" + playerUnit.pokemon.change_speed + "\n";
            levelUpText.text += " S ATK:".PadRight(27) + "+" + playerUnit.pokemon.change_sp_attack + "\n";
            levelUpText.text += " S DFN:".PadRight(27) + "+" + playerUnit.pokemon.change_sp_defense + "\n";
            levelUpUI.SetActive(true);
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            levelUpUI.SetActive(false);
            playerUnit.pokemon = poke;
            playerHUD.SetMoves(playerUnit);
            TryToSwitchPokemon(activePokemon);
        }

        public IEnumerator Evolve(Pokemon poke, int i)
        {

            if (poke.time_to_evolve && poke.current_hp > 0)
            {
                dialogueText.text = poke.name + " is trying to evolve!";
                yield return new WaitForSeconds(2);
                dialogueText.text = "Do you let them?";
                yesno.SetActive(true);
                while (yesno.activeSelf)
                {
                    yield return null;
                }
                //poke = playerUnit.pokemon;
                Debug.Log(poke.want_to_evolve.ToString());
                poke.want_to_evolve = wantsToEvolve;
                //Pokemon p = poke.ask_to_evolve();
                if (wantsToEvolve)
                {

                    //Pokemon p = poke.evolve_pokemon();
                    GameController.playerPokemon[i].evolve();
                    dialogueText.text = "Your Pokemon evolved into a " + GameController.playerPokemon[i].name + "!!";
                    GameController.playerPokemon[i].want_to_evolve = false;
                    //p.want_to_evolve = false;

                }
                else
                {
                    dialogueText.text = "Your Pokemon will wait on evolving for now.";
                }
                wantsToEvolve = false;
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
            playerUnit.pokemon = poke;
            playerHUD.SetMoves(playerUnit);
        }

        #endregion leveling up
    }
}
