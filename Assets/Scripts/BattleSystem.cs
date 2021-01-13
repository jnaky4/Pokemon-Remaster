using System.Collections;
using System.Collections.Generic;
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
    //IDK what these are

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
    /**********************************************************************************************************************************************
     * FUNCTIONS
     **********************************************************************************************************************************************/
    void Start()
    {
        state = BattleState.START;
        SetDownButtons();
        enemyUnit = 
        StartCoroutine(SetupBattle());
    }
    //This function starts the battle sequence

    IEnumerator SetupBattle()
    {
        GameObject player = Instantiate(playerPrefab);
        playerUnit = player.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab);
        enemyUnit = enemy.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.name + " appears!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    //This function sets up the battle state for us including the UI

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
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
            StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator EnemyAttack()
    {

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

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        SetDownButtons();
        StartCoroutine(PlayerAttack());
    }

    public void OnRunAwayButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        SetDownButtons();
        RunAway();
    }

    public void OnPokemonButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        SetDownButtons();
    }

    public void OnBallsButton()
    {
        if (state != BattleState.PLAYERTURN) return;
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

}
