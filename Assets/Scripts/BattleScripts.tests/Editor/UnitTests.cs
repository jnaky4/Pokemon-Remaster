using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


namespace Pokemon
{

    [TestFixture]
    public class UnitTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BattleSystemStartsInStartState()
        {
            // Use the Assert class to test conditions
            BattleSystem bs = new BattleSystem();
            Assert.IsTrue(bs.state == BattleState.START, "oops");
            Assert.IsFalse(bs.state == BattleState.ENEMYTURN, "oops");
        }


        [Test]
        public void PriorityTests()
        {
            // Use the Assert class to test conditions
            Main App = (new GameObject("MainLoader")).AddComponent<Main>();
            App.Start();
            BattleSystem bs = (new GameObject("BattleSystemObject")).AddComponent<BattleSystem>();
            bs.playerUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.enemyUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.playerUnit.pokemon = new Pokemon(3, 2, "Quick Attack");
            bs.enemyUnit.pokemon = new Pokemon(3, 3, "Ember");
            Moves playerMove = bs.playerUnit.pokemon.currentMoves[0];
            Moves enemyMove = bs.enemyUnit.pokemon.currentMoves[0];
            int playerMoveNum = 0;
            int enemyMoveNum = 0;
            System.Random rnd = new System.Random();
            Assert.IsTrue(bs.DetermineNextState(playerMove, enemyMove) == BattleState.PLAYERTURN, "player has priority and goes first");
            
            Debug.Log(playerMove.name);
            Debug.Log(enemyMove.name); 

            bs.playerUnit.pokemon = new Pokemon(4, 5, "Ember");
            bs.enemyUnit.pokemon = new Pokemon(4, 4, "Ember");
            playerMoveNum = 0;
            enemyMoveNum = 0;
            playerMove = bs.playerUnit.pokemon.currentMoves[0];
            enemyMove = bs.enemyUnit.pokemon.currentMoves[0];
            Debug.Log(playerMove.name);
            Debug.Log(enemyMove.name);
            Debug.Log(bs.playerUnit.pokemon.current_speed);
            Debug.Log(bs.enemyUnit.pokemon.current_speed);
            Assert.IsTrue(bs.DetermineNextState(playerMove, enemyMove) == BattleState.PLAYERTURN, "higher level clone (player) is faster");


            bs.playerUnit.pokemon = new Pokemon(4, 4, "Ember");
            bs.enemyUnit.pokemon = new Pokemon(4, 5, "Ember");
            playerMoveNum = 0;
            enemyMoveNum = 0;
            playerMove = bs.playerUnit.pokemon.currentMoves[0];
            enemyMove = bs.enemyUnit.pokemon.currentMoves[0];
            Assert.IsTrue(bs.DetermineNextState(playerMove, enemyMove) == BattleState.ENEMYTURN, "higher level clone (enemy) is faster");
            Debug.Log(playerMove.name);
            Debug.Log(enemyMove.name);
            
        }

        //see if end battle should set battlestate to won, lost, runaway, caughtpokemon


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

    }
}