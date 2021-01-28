using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RUNAWAY }

public class BattleSystem : MonoBehaviour
{
    /**********************************************************************************************************************************************
     * VARIABLES
     **********************************************************************************************************************************************/
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
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

    public string path = "..\\..\\CSV";
    public string moves = "\\MOVES.csv";

    /**********************************************************************************************************************************************
     * FUNCTIONS
     **********************************************************************************************************************************************/
    void Start()
    {
        state = BattleState.START;
        pokeMenuUI.SetActive(false);
        attackMenuUI.SetActive(false);
        SetDownButtons();

        //enemyUnit = 
        StartCoroutine(SetupBattle());
    }
    //This function starts the battle sequence

    IEnumerator SetupBattle()
    {
        //GameObject pokeMenu = Instantiate(pokemonMenuUI);


        GameObject player = Instantiate(playerPrefab);
        playerUnit = player.GetComponent<Unit>();
        playerUnit.attack = 84;
        playerUnit.defense = 78;
        playerUnit.speed = 82;
        Pkm_Moves pkm = new Pkm_Moves();
        pkm.name = "Fireball";
        pkm.damage = 60;
        playerUnit.move1 = pkm;
        Pkm_Moves pkm2 = new Pkm_Moves();
        pkm2.name = "Fly";
        pkm2.damage = 30;
        playerUnit.move2 = pkm2;
        Pkm_Moves pkm3 = new Pkm_Moves();
        pkm3.name = "Rage";
        pkm3.damage = 10;
        playerUnit.move3 = pkm3;
        Pkm_Moves pkm4 = new Pkm_Moves();
        pkm4.name = "Splash";
        pkm4.damage = 0;
        playerUnit.move4 = pkm4;
        //playerUnit.moves.Add

        GameObject enemy = Instantiate(enemyPrefab);
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.attack = 82;
        enemyUnit.defense = 50;
        enemyUnit.speed = 80;

        dialogueText.text = "A wild " + enemyUnit.name + " appears!";

        playerHUD.SetHUD(playerUnit, true);
        enemyHUD.SetHUD(enemyUnit, false);


        yield return new WaitForSeconds(2);

        if (enemyUnit.speed > playerUnit.speed)
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
        else if (enemyUnit.speed < playerUnit.speed)
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

    IEnumerator PlayerAttack(Pkm_Moves attack)
    {
        playerUnit.SetDamage(enemyUnit.defense, attack.damage);

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.name + " used " + attack.name + "!";
        yield return new WaitForSeconds(2);
        dialogueText.text = enemyUnit.name + " took " + playerUnit.damage + " amount of damage...";
        yield return new WaitForSeconds(2);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator EnemyAttack()
    {
        enemyUnit.SetDamage(playerUnit.defense, 10);
        dialogueText.text = enemyUnit.name + " attacks!";
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        yield return new WaitForSeconds(2);

        dialogueText.text = playerUnit.name + " took " + enemyUnit.damage + " points of damage!";

        yield return new WaitForSeconds(2);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You lost! You blacked out!";
        }
        else if (state == BattleState.RUNAWAY)
        {
            dialogueText.text = "Got away safely...";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action";
        SetUpButtons();
    }
    void EnemyTurn()
    {
        StartCoroutine(EnemyAttack());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        if (attackMenuUI.activeSelf) attackMenuUI.SetActive(false);
        else attackMenuUI.SetActive(true);
        //SetDownButtons();

        //StartCoroutine(PlayerAttack());
    }

    public void OnRunAwayButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        attackMenuUI.SetActive(false);
        SetDownButtons();
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
        SetDownButtons();
    }

    public void RunAway()
    {
        state = BattleState.RUNAWAY;
        EndBattle();
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

    public void Attack1()
    {
        attackMenuUI.SetActive(false);
        SetDownButtons();
        StartCoroutine(PlayerAttack(playerUnit.move1));
    }
    public void Attack2()
    {
        attackMenuUI.SetActive(false);
        SetDownButtons();
        StartCoroutine(PlayerAttack(playerUnit.move2));
    }
    public void Attack3()
    {
        attackMenuUI.SetActive(false);
        SetDownButtons();
        StartCoroutine(PlayerAttack(playerUnit.move3));
    }
    public void Attack4()
    {
        attackMenuUI.SetActive(false);
        SetDownButtons();
        StartCoroutine(PlayerAttack(playerUnit.move4));
    }
}
