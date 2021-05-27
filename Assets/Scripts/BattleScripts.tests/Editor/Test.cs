using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


namespace Pokemon
{

    [TestFixture]
    public class LinkBattleSystem
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
        public void DecisionPriorityBeatsNothing()
        {
            // Use the Assert class to test conditions
            Main App = new Main();
            App.Start();
            BattleSystem bs = new BattleSystem();
            bs.playerUnit = new Unit();
            bs.enemyUnit = new Unit();
            bs.playerUnit.pokemon = new Pokemon(4, 4, "Quick Attack");
            bs.enemyUnit.pokemon = new Pokemon(4, 4, "Ember");
            int playerMoveNum = 0;
            int enemyMoveNum = 0;
            System.Random rnd = new System.Random();
            Moves playerMove = bs.DeterminePlayerMove(playerMoveNum);
            Moves enemyMove = bs.DetermineEnemyMove(enemyMoveNum, rnd);
            bs.playerAttacksFirst = bs.DeterminePriority(playerMove, enemyMove);
            Assert.IsTrue(bs.playerAttacksFirst, "player has priority and goes first");
            Debug.Log(playerMove.name);
            Debug.Log(enemyMove.name);
            Debug.Log(bs.playerAttacksFirst);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

    }
}