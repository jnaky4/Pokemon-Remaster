using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RUNAWAY, CAUGHTPOKEMON, CHANGEPOKEMON }

namespace Pokemon
{
    public class BattleSystem : MonoBehaviour
    {
        /**********************************************************************************************************************************************
         * VARIABLES
         **********************************************************************************************************************************************/

        public int activePokemon = 0;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public PlayerBattle player;
        public SpriteRenderer playerSprite;
        public SpriteRenderer enemySprite;
        public SpriteRenderer playerAttackSprite;
        public SpriteRenderer enemyAttackSprite;

        //IDK what these are but they need to stay

        //[SerializeField] List<Sprite> AttackSprites;
        List<Sprite> AttackSprites = new List<Sprite>();
        SpriteAnimator PlayerAttackAnim;
        SpriteAnimator EnemyAttackAnim;
        bool playerAttack;
        bool playerInitialAttack;
        bool enemyAttack;
        bool enemyInitialAttack;
        string playerMoveName;
        string enemyMoveName;
        //all of this stuff is for animation

        Unit playerUnit;
        Unit enemyUnit;
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
        public Button backPoke;

        bool breakOutOfDecision = false;

        /**********************************************************************************************************************************************
         * FUNCTIONS
         **********************************************************************************************************************************************/
        void Start()
        {
            PlayerAttackAnim = new SpriteAnimator(AttackSprites, playerAttackSprite, 0.07f);
            EnemyAttackAnim = new SpriteAnimator(AttackSprites, enemyAttackSprite, 0.07f);

            state = BattleState.START;
            pokeMenuUI.SetActive(false);
            attackMenuUI.SetActive(false);
            ballsMenuUI.SetActive(false);
            SetDownButtons();
            StartCoroutine(SetupBattle());
        }

        //logic for whether a player or opponent's attack animation plays
        private void Update()
        {

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
                }
            }
        }

        public static List<Dictionary<string, object>> load_CSV(string name)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(name);
            return data;
        }


        public void print_pokemon()
        {/*

            for (int i = 1; i < 152; i++)
            {
                Pokemon TestPokemon = new Pokemon(i, 50, "Flamethrower", "Earthquake", "Wing Attack", "Slash");
                              Debug.Log("Name " + TestPokemon.name);
                                Debug.Log("Base Attack " + TestPokemon.base_attack + " Current Attack " + TestPokemon.max_attack);
                                Debug.Log("Type1: " + TestPokemon.type1.type);
                                if (TestPokemon.type2 != null)
                                {
                                    Debug.Log("Type2: " + TestPokemon.type2.type);
                                }

                foreach (Learnset learned in TestPokemon.learnset)
                {
                    Debug.Log(learned.ToString());
                    Debug.Log("PP " + learned.get_move().pp);
                    Debug.Log("TYPE " + learned.get_move().move_type.type);

                }

            }*/
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

            for (int i = 0; i < 5; i++)
            {
                if (GameController.playerPokemon[i].current_hp > 0)
                {
                    playerUnit.pokemon = GameController.playerPokemon[i];
                    break;
                }
            }

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
            if (String.Compare(playerUnit.pokemon.currentMoves[0].name, "default") == 0) b5GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[1].name, "default") == 0) b6GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[2].name, "default") == 0) b7GO.SetActive(false);
            if (String.Compare(playerUnit.pokemon.currentMoves[3].name, "default") == 0) b8GO.SetActive(false);

            if (GameController.playerPokemon[1] == null) poke2.SetActive(false);
            if (GameController.playerPokemon[2] == null) poke3.SetActive(false);
            if (GameController.playerPokemon[3] == null) poke4.SetActive(false);
            if (GameController.playerPokemon[4] == null) poke5.SetActive(false);
            if (GameController.playerPokemon[5] == null) poke6.SetActive(false);

            GameObject enemyGO = Instantiate(enemyPrefab);
            enemyUnit = enemyGO.GetComponent<Unit>();
            enemyUnit.pokemon = GameController.opponentPokemon[0];

            playerHUD.SetHUD(playerUnit, true, player, GameController.playerPokemon);
            enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);
            SetPlayerSprite(playerUnit, playerSprite);
            SetOpponentSprite(enemyUnit, enemySprite);

            if (GameController.isCatchable)
            {
                dialogueText.text = "A wild " + enemyUnit.pokemon.name + " appears!";
                yield return new WaitForSeconds(2);
            }
            else
            {
                dialogueText.text = "Enemy Bug Catcher Joey sends out " + enemyUnit.pokemon.name + "!";
                yield return new WaitForSeconds(2);
            }

            state = BattleState.PLAYERTURN;
            PlayerTurn();

        }
        //This function sets up the battle state for us including the UI

        IEnumerator Decision(int playerMoveNum)
        {
            breakOutOfDecision = false;
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();

            if (playerMoveNum != -1)
            {
                if (playerUnit.pokemon.currentMoves[playerMoveNum].current_pp == 0)
                {
                    dialogueText.text = "No remaining PP for " + playerUnit.pokemon.currentMoves[playerMoveNum].name + "!";
                    yield return new WaitForSeconds(2);
                    PlayerTurn();
                    yield break;
                }
            }
            int moveNum = 0;
            bool struggle = EnemyStruggle();
            Moves enemyMove, playerMove;
            System.Random rnd = new System.Random();
            if (playerMoveNum != -1) playerMove = playerUnit.pokemon.currentMoves[playerMoveNum];
            else playerMove = playerUnit.pokemon.struggle;
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
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            SetDownButtons();
            playerMoveName = playerMove.name;
            enemyMoveName = enemyMove.name;
            if (playerMove.priority > enemyMove.priority)
            {
                state = BattleState.PLAYERTURN;
                if (playerMoveNum != -1) yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
                else yield return StartCoroutine(PlayerAttack(playerMove));
/*                if (breakOutOfDecision)
                {
                    yield return StartCoroutine(SeeIfEndBattle());
                    PlayerTurn();
                    //yield break;
                }*/
                if(!breakOutOfDecision)
                {
                    state = BattleState.ENEMYTURN;
                    yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
                }
            }
            else if (playerMove.priority < enemyMove.priority)
            {
                state = BattleState.ENEMYTURN;
                yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
/*                if (breakOutOfDecision)
                {
                    yield return StartCoroutine(SeeIfEndBattle());
                    PlayerTurn();
                    //yield break;
                }*/
                if (!breakOutOfDecision)
                {
                    state = BattleState.PLAYERTURN;
                    if (playerMoveNum != -1) yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
                    else yield return StartCoroutine(PlayerAttack(playerMove));
                }
            }
            else
            {
                if (enemyUnit.pokemon.current_speed > playerUnit.pokemon.current_speed)
                {
                    state = BattleState.ENEMYTURN;
                    yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
/*                    if (breakOutOfDecision)
                    {
                        yield return StartCoroutine(SeeIfEndBattle());
                        PlayerTurn();
                        //yield break;
                    }*/
                    if (!breakOutOfDecision)
                    {
                        state = BattleState.PLAYERTURN;
                        yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
                    }
                }
                else if (enemyUnit.pokemon.current_speed < playerUnit.pokemon.current_speed)
                {
                    state = BattleState.PLAYERTURN;
                    yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
/*                    if (breakOutOfDecision)
                    {
                        yield return StartCoroutine(SeeIfEndBattle());
                        PlayerTurn();
                        //yield break;
                    }*/
                    if (!breakOutOfDecision)
                    {
                        state = BattleState.ENEMYTURN;
                        yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
                    }
                }
                else
                {
                    int num = rnd.Next(1, 3);
                    if (num == 1)
                    {
                        state = BattleState.PLAYERTURN;
                        yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
/*                        if (breakOutOfDecision)
                        {
                            yield return StartCoroutine(SeeIfEndBattle());
                            PlayerTurn();
                            //yield break;
                        }*/
                        if (!breakOutOfDecision)
                        {
                            state = BattleState.ENEMYTURN;
                            yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
                        }
                    }
                    else
                    {
                        state = BattleState.ENEMYTURN;
                        yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
/*                        if (breakOutOfDecision)
                        {
                            yield return StartCoroutine(SeeIfEndBattle());
                            PlayerTurn();
                            //yield break;
                        }*/
                        if (!breakOutOfDecision)
                        {
                            state = BattleState.PLAYERTURN;
                            yield return StartCoroutine(PlayerAttack(playerMove, playerMoveNum));
                        }
                    }
                }
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
        IEnumerator DecisionSwitch(int pokemon)
        {
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();

            int moveNum = 0;
            bool struggle = EnemyStruggle();
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
            state = BattleState.CHANGEPOKEMON;
            yield return StartCoroutine(SwitchPokemon(pokemon));
            state = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
            if (breakOutOfDecision)
            {
                StartCoroutine(SeeIfEndBattle());
            }
            PlayerTurn();
            yield break;
        }

        IEnumerator DecisionCatch(int ballNum)
        {
            SetDownButtons();
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            if (!GameController.isCatchable)
            {
                SetDownButtons();
                dialogueText.text = "You can't catch other trainer's Pokemon!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }

            int moveNum = 0;
            bool struggle = EnemyStruggle();
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
            state = BattleState.CHANGEPOKEMON;
            yield return StartCoroutine(CatchPokemon(ballNum));
            state = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyAttack(enemyMove, moveNum));
            if (breakOutOfDecision)
            {
                bool isEnd = true;
                for (int j = 0; j < GameController.playerPokemon.Count(s => s != null); j++)
                {
                    state = BattleState.LOST;
                    if (GameController.playerPokemon[j].current_hp > 0)
                    {
                        isEnd = false;
                        break;
                    }
                }
                yield return StartCoroutine(EndBattle());
                if (isEnd) yield return StartCoroutine(EndBattle());
                for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                {
                    if (GameController.opponentPokemon[j].current_hp > 0)
                    {
                        state = BattleState.WON;
                        isEnd = false;
                        break;
                    }
                }
                if (isEnd) yield return StartCoroutine(EndBattle());
            }
            PlayerTurn();
            yield break;
        }

        public bool CriticalHit(Unit unit)
        {
            System.Random rnd = new System.Random();
            int num = rnd.Next(1,100);
            if (unit.pokemon.critical_stage == 0)
            {
                if (num <= 6) return true;
            }
            if (unit.pokemon.critical_stage == 1)
            {
                if (num <= 13) return true;
            }
            if (unit.pokemon.critical_stage == 2)
            {
                if (num <= 25) return true;
            }
            if (unit.pokemon.critical_stage == 3)
            {
                if (num <= 33) return true;
            }
            if (unit.pokemon.critical_stage == 4)
            {
                if (num <= 50) return true;
            }
            return false;
        }

        public string GetCategoryOfMove(Moves move)
        {
            return move.category;
        }

        public double DoDamage(Unit attacker, Unit defender, Moves attack, bool crit)
        {
            double type1 = 1;
            double type2 = 1;

            switch (attack.move_type.type)
            {
                case "Normal":
                    type1 = defender.pokemon.type1.defend_normal;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_normal;
                    break;
                case "Fire":
                    type1 = defender.pokemon.type1.defend_fire;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_fire;
                    break;
                case "Water":
                    type1 = defender.pokemon.type1.defend_water;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_water;
                    break;
                case "Electric":
                    type1 = defender.pokemon.type1.defend_electric;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_electric;
                    break;
                case "Grass":
                    type1 = defender.pokemon.type1.defend_grass;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_grass;
                    break;
                case "Ice":
                    type1 = defender.pokemon.type1.defend_ice;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ice;
                    break;
                case "Fighting":
                    type1 = defender.pokemon.type1.defend_fighting;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_fighting;
                    break;
                case "Poison":
                    type1 = defender.pokemon.type1.defend_poison;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_poison;
                    break;
                case "Ground":
                    type1 = defender.pokemon.type1.defend_ground;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ground;
                    break;
                case "Flying":
                    type1 = defender.pokemon.type1.defend_flying;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_flying;
                    break;
                case "Psychic":
                    type1 = defender.pokemon.type1.defend_psychic;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_psychic;
                    break;
                case "Bug":
                    type1 = defender.pokemon.type1.defend_bug;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_bug;
                    break;
                case "Rock":
                    type1 = defender.pokemon.type1.defend_rock;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_rock;
                    break;
                case "Ghost":
                    type1 = defender.pokemon.type1.defend_ghost;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_ghost;
                    break;
                case "Dragon":
                    type1 = defender.pokemon.type1.defend_dragon;
                    if (defender.pokemon.type2 is null) type2 = 1;
                    else type2 = defender.pokemon.type2.defend_dragon;
                    break;
                case "Dark":
                    break;
                case "Steel":
                    break;
                case "Fairy":
                    break;
                default:
                    break;
            }
            if (attack.base_power > 0)
            {
                if (GetCategoryOfMove(attack).CompareTo("Physical") == 0)
                {
                    attacker.SetDamage(defender.pokemon.current_defense, attacker.pokemon.current_attack, attack.base_power, attack, crit, type1, type2);
                }
                else if (GetCategoryOfMove(attack).CompareTo("Special") == 0)
                {
                    attacker.SetDamage(defender.pokemon.current_sp_defense, attacker.pokemon.current_sp_attack, attack.base_power, attack, crit, type1, type2);
                }
            }
            else
            {
                attacker.SetDamage(1, 0, 0, attack, crit, 1, 1);
            }
            return type1 * type2;
        }

        IEnumerator PlayerAttack(Moves attack, int moveNum)
        {
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            SetDownButtons();

            bool crit = CriticalHit(playerUnit);
            System.Random rnd = new System.Random();
            int num = rnd.Next(1, 100);
            dialogueText.text = playerUnit.pokemon.name + " used " + attack.name + "!";
            yield return new WaitForSeconds(2);
            if (num <= (attack.accuracy*playerUnit.pokemon.current_accuracy * enemyUnit.pokemon.current_evasion))
            {
                if (attack.current_stat_change.CompareTo("null") != 0) playerUnit.SetStages(attack, enemyUnit);
                double super = DoDamage(playerUnit, enemyUnit, attack, crit);
                if (moveNum != -1) playerUnit.DoPP(moveNum);
                bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
                enemyHUD.SetHP(enemyUnit.pokemon.current_hp);
                playerInitialAttack = true;
                yield return new WaitForSeconds(0.97f);
                if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("enemy") == 0) dialogueText.text = "Enemy " + enemyUnit.pokemon.name + "'s " + attack.current_stat_change + " fell!";
                else if (attack.current_stat_change.CompareTo("null") != 0 && attack.target.CompareTo("self") == 0) dialogueText.text = "Your " + playerUnit.pokemon.name + "'s " + attack.current_stat_change + " rose!";
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
                    else if (super < 1)
                    {
                        dialogueText.text = "It's not very effective...";
                    }
                    else dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " took damage...";
                }
                yield return new WaitForSeconds(2);

                if (isDead)
                {
                    breakOutOfDecision = true;
                    bool won = true;
                    for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                    {
                        if (GameController.opponentPokemon[j].current_hp > 0)
                        {
                            won = false;
                            break;
                        }
                    }
                    if (won)
                    {
                        state = BattleState.WON;
                        dialogueText.text = enemyUnit.pokemon.name + " faints!";
                        yield return new WaitForSeconds(2);
                        yield break;
                    }
                    else
                    {
                        state = BattleState.CHANGEPOKEMON;
                        dialogueText.text = enemyUnit.pokemon.name + " faints!";
                        yield return new WaitForSeconds(2);
                        for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                        {
                            if (GameController.opponentPokemon[j].current_hp > 0)
                            {
                                enemyUnit.pokemon = GameController.opponentPokemon[j];
                                dialogueText.text = "They sent out a " + enemyUnit.pokemon.name + "!";
                                yield return new WaitForSeconds(2);
                                enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);
                                SetOpponentSprite(enemyUnit, enemySprite);
                                break;
                            }
                        }
                        PlayerTurn();
                        yield break;
                    }
                }

            }
            else
            {
                dialogueText.text = "Your attack missed!";
                yield return new WaitForSeconds(2);
                state = BattleState.ENEMYTURN;
                yield break;
            }
            yield break;
        }

        IEnumerator PlayerAttack(Moves attack)
        {
            ClosePokemonMenu();
            CloseMovesMenu();
            CloseBallsMenu();
            SetDownButtons();
            dialogueText.text = playerUnit.pokemon + " has no remaining available moves.";
            yield return new WaitForSeconds(2);
            dialogueText.text = playerUnit.pokemon.name + " used " + attack.name + "!";
            playerInitialAttack = true;
            yield return new WaitForSeconds(2);
            double super = DoDamage(playerUnit, enemyUnit, attack, false);
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.pokemon.current_hp);
            if (super > 1)
            {
                dialogueText.text = "It's super effective!";
                yield return new WaitForSeconds(2);
            }
            else if (super < 1)
            {
                dialogueText.text = "It's not very effective...";
                yield return new WaitForSeconds(2);
            }

            dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " took damage...";
            yield return new WaitForSeconds(2);

            bool isPlayerDead = playerUnit.TakeDamage(playerUnit.pokemon.max_hp / 4);
            dialogueText.text = playerUnit.pokemon.name + " got hit with recoil damage!";
            playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
            yield return new WaitForSeconds(2);

            if (isDead)
            {
                bool won = true;
                for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                {
                    if (GameController.opponentPokemon[j].current_hp > 0)
                    {
                        won = false;
                        break;
                    }
                }
                if (won)
                {
                    state = BattleState.WON;
                    dialogueText.text = enemyUnit.pokemon.name + " faints!";
                    yield return new WaitForSeconds(2);
                    yield break;
                }
                else
                {
                    state = BattleState.CHANGEPOKEMON;
                    dialogueText.text = enemyUnit.pokemon.name + " faints!";
                    yield return new WaitForSeconds(2);
                    for (int j = 0; j < GameController.opponentPokemon.Count(s => s != null); j++)
                    {
                        if (GameController.opponentPokemon[j].current_hp > 0)
                        {
                            enemyUnit.pokemon = GameController.opponentPokemon[j];
                            dialogueText.text = "They sent out a " + enemyUnit.pokemon.name + "!";
                            yield return new WaitForSeconds(2);
                            enemyHUD.SetHUD(enemyUnit, false, player, GameController.playerPokemon);
                            SetOpponentSprite(enemyUnit, enemySprite);
                            break;
                        }
                    }
                    yield break;
                }
            }
            yield break;
        }

        IEnumerator CatchPokemon(int typeOfPokeball)
        {
            if (!GameController.isCatchable)
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
            if (randomNumber < GameController.catchRate)
            {
                dialogueText.text = enemyUnit.pokemon.name + " broke free!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }
            randomNumber2 = rnd.Next(256);
            f = (enemyUnit.pokemon.max_hp * 255 * 4);
            f = f / (enemyUnit.pokemon.current_hp * catchRate);
            if (f >= randomNumber2)
            {
                state = BattleState.CAUGHTPOKEMON;
                StartCoroutine(EndBattle());
                yield break;
            }
            else
            {
                int d = (GameController.catchRate * 100) / numShakes;
                dialogueText.text = enemyUnit.pokemon.name + " broke free!";
                yield return new WaitForSeconds(2);
                PlayerTurn();
                yield break;
            }

        }
        IEnumerator SwitchPokemon(int num)
        {
            if (GameController.playerPokemon[num].current_hp <= 0)
            {
                dialogueText.text = "That pokemon has no HP remaining!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.CHANGEPOKEMON) PlayerTurn();
                else SwitchPokemonAfterDeath();
                yield break;
            }
            if (activePokemon == num)
            {
                dialogueText.text = "You already have that pokemon out!";
                yield return new WaitForSeconds(2);
                if (state != BattleState.CHANGEPOKEMON) PlayerTurn();
                else SwitchPokemonAfterDeath();
                yield break;
            }

            dialogueText.text = "Get out of there, " + playerUnit.pokemon.name + "!";
            playerUnit.pokemon.reset_stages();
            GameController.playerPokemon[activePokemon] = playerUnit.pokemon;
            yield return new WaitForSeconds(2);
            playerUnit.pokemon = GameController.playerPokemon[num];
            activePokemon = num;

            playerHUD.SetActivePokemon(GameController.playerPokemon, num, playerUnit);
            SetPlayerSprite(playerUnit, playerSprite);
            dialogueText.text = "Go, " + playerUnit.pokemon.name + "!";
            yield return new WaitForSeconds(2);

            if (playerUnit.pokemon.currentMoves[0] == null)
            {
                b5GO.SetActive(false);
            }
            else
            {
                b5GO.SetActive(true);
            }
            if (playerUnit.pokemon.currentMoves[1] == null)
            {
                b6GO.SetActive(false);
            }
            else
            {
                b6GO.SetActive(true);
            }
            if (playerUnit.pokemon.currentMoves[2] == null)
            {
                b7GO.SetActive(false);
            }
            else
            {
                b7GO.SetActive(true);
            }
            if (playerUnit.pokemon.currentMoves[3] == null)
            {
                b8GO.SetActive(false);
            }
            else
            {
                b8GO.SetActive(true);
            }

            state = BattleState.ENEMYTURN;
            backPoke.interactable = true;
            //EnemyTurn();
        }
        void SwitchPokemonAfterDeath()
        {
            //activePokemon = -1;
            playerHUD.SetPokemon(GameController.playerPokemon);
            backPoke.interactable = false;
            OpenPokemonMenu();
        }
        void SetPlayerSprite(Unit unit, SpriteRenderer sprite)
        {
            Texture2D SpriteTexture = new Texture2D(2, 2);
            byte[] fileData;
            fileData = File.ReadAllBytes(unit.pokemon.image1);
            SpriteTexture.LoadImage(fileData);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));

            sprite.sprite = NewSprite;
        }
        void SetOpponentSprite(Unit unit, SpriteRenderer sprite)
        {
            Texture2D SpriteTexture = new Texture2D(2, 2);
            byte[] fileData;
            fileData = File.ReadAllBytes(unit.pokemon.image2);
            SpriteTexture.LoadImage(fileData);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));

            sprite.sprite = NewSprite;
        }

        bool EnemyStruggle()
        {
            int i;
            bool struggle = false;

            for (i = 0; i < enemyUnit.pokemon.currentMoves.Count(s => s != null); i++)
            {
                if (enemyUnit.pokemon.currentMoves[i].current_pp != 0)
                {
                    struggle = false;
                    break;
                }
                struggle = true;
            }
            return struggle;
        }

        IEnumerator EnemyAttack(Moves move, int moveNum)
        {
            SetDownButtons();
            System.Random rnd = new System.Random();
            bool crit = CriticalHit(enemyUnit);
            int num = rnd.Next(1, 100);
            dialogueText.text = "Enemy " + enemyUnit.pokemon.name + " used " + move.name + "!";
            yield return new WaitForSeconds(2);
            if (num <= move.accuracy * enemyUnit.pokemon.current_accuracy * playerUnit.pokemon.current_evasion)
            {
                enemyInitialAttack = true;
                if (move.current_stat_change.CompareTo("null") != 0) enemyUnit.SetStages(move, playerUnit);
                double super = DoDamage(enemyUnit, playerUnit, move, crit);
                bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
                Debug.Log(enemyUnit.damage.ToString());
                if (moveNum != -1) enemyUnit.DoPP(moveNum);
                playerHUD.SetHP(playerUnit.pokemon.current_hp, playerUnit);
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
                    else if (super < 1)
                    {
                        dialogueText.text = "It's not very effective...";
                    }
                    else dialogueText.text = "Your " + playerUnit.pokemon.name + " took damage...";
                }
                yield return new WaitForSeconds(2);

                if (isDead)
                {
                    breakOutOfDecision = true;
                    for(int j = 0; j < GameController.playerPokemon.Count(s => s != null); j++)
                    {
                        if (GameController.playerPokemon[j].current_hp > 0)
                        {
                            breakOutOfDecision = false;
                            break;
                        }
                    }
                    if (breakOutOfDecision)
                    {
                        state = BattleState.LOST;
                        yield break;
                    }
                    else
                    {
                        state = BattleState.CHANGEPOKEMON;
                        dialogueText.text = playerUnit.pokemon.name + " faints!";
                        yield return new WaitForSeconds(2);
                        SwitchPokemonAfterDeath();
                        yield break;
                    }
                }
            }
            else
            {
                dialogueText.text = "The move failed!";
                yield return new WaitForSeconds(2);
                yield break;
            }
            yield break;
        }

        IEnumerator SeeIfEndBattle()
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
                yield break;
            }
        }
        IEnumerator EndBattle()
        {
            breakOutOfDecision = true;
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
                for(var p = 0; p < 6; p++)
                {
                    if (GameController.playerPokemon[p] == null)
                    {
                        GameController.playerPokemon[p] = enemyUnit.pokemon;
                        break;
                    }
                }
            }
            else
            {
                dialogueText.text = "What the fuck just happened";
            }
            yield return new WaitForSeconds(2);
            GameController.playerPokemon[activePokemon] = playerUnit.pokemon;
            for (int i = 0; i < 5; i++)
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
                    GameController.playerPokemon[i].current_speed = GameController.playerPokemon[i].max_speed;
                    GameController.playerPokemon[i].current_accuracy = 1;
                    GameController.playerPokemon[i].current_evasion = 1;
                }
            }
            GameController.endCombat = true;
            //SceneManager.UnloadSceneAsync("BattleScene");
            //SceneManager.LoadScene("Pallet Town");
        }

        void PlayerTurn()
        {
            state = BattleState.PLAYERTURN;
            dialogueText.text = "Choose an action";
            playerHUD.SetPokemon(GameController.playerPokemon);
            playerHUD.SetMoves(playerUnit);
            SetUpButtons();
        }
/*        void EnemyTurn()
        {
            SetDownButtons();
            StartCoroutine(EnemyAttack());
        }*/

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
            CloseBallsMenu();
            CloseMovesMenu();
            SetDownButtons();
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
            StartCoroutine(DecisionSwitch(0));
        }
        public void OnPokemon1()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(DecisionSwitch(1));
        }
        public void OnPokemon2()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(DecisionSwitch(2));
        }
        public void OnPokemon3()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(DecisionSwitch(3));
        }
        public void OnPokemon4()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(DecisionSwitch(4));
        }
        public void OnPokemon5()
        {
            ClosePokemonMenu();
            SetDownButtons();
            StartCoroutine(DecisionSwitch(5));
        }

        public void OpenMovesMenu()
        {
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
            StartCoroutine(Decision(0));
            //StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[0], 0));
        }
        public void Attack2()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(1));
            //StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[1], 1));
        }
        public void Attack3()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(2));
            //StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[2], 2));
        }
        public void Attack4()
        {
            CloseMovesMenu();
            SetDownButtons();
            StartCoroutine(Decision(3));
            //StartCoroutine(PlayerAttack(playerUnit.pokemon.currentMoves[3], 3));
        }
        public void PokeBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(DecisionCatch(1));
        }
        public void GreatBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(DecisionCatch(2));
        }
        public void UltraBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(DecisionCatch(3));
        }
        public void MasterBall()
        {
            CloseBallsMenu();
            SetDownButtons();
            StartCoroutine(DecisionCatch(4));
        }

        public void GetAttackSprites(string attack)
        {
            attack = attack.Replace(" ", "_");

            var path = Directory.GetCurrentDirectory();

            if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                path += "/Assets/Sprites/Attack_Animations/" + attack;
            else
                path +=  "\\Assets\\Sprites\\Attack_Animations\\" + attack;

            if (!Directory.Exists(path))
            {
                path = Directory.GetCurrentDirectory();

                if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
                    path += "/Assets/Sprites/Attack_Animations/" + "Bite";
                else
                    path += "\\Assets\\Sprites\\Attack_Animations\\" + "Bite";
            }

            string[] files = Directory.GetFiles(path, "*.png");

            Debug.Log(playerUnit.pokemon.currentMoves[2].name);

            AttackSprites.Clear();
            AttackSprites.Add(null);
            for (int i = 0; i < files.Length - 1; i++)
            {
                Texture2D SpriteTexture = new Texture2D(2, 2);
                byte[] fileData;
                fileData = File.ReadAllBytes(files[i]);
                SpriteTexture.LoadImage(fileData);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0));

                AttackSprites.Add(NewSprite);
            }
            AttackSprites.TrimExcess();

    Debug.Log(path);
        }
    }
}