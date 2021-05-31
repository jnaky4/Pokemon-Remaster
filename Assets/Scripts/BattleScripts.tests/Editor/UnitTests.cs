using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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


        //Stub implementation to bypass actual Extension manager class.  
        public class DialogueTextObject : Text
        {
            public String text;
            public String ToString()
            {
                return text;
            }
        }

        public class FakeRandom : System.Random
        {

            public int next()
            {
                return 1;
            }
        }


        [Test]
        public void PriorityTests()
        {
            // Use the Assert class to test conditions

            /*
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
            */
        }

        public BattleSystem AttackXYZ_Setup()
        {
            // Use the Assert class to test conditions
            Main App = (new GameObject("MainLoader")).AddComponent<Main>();
            App.Start();

            BattleSystem bs = (new GameObject("BattleSystemObject")).AddComponent<BattleSystem>();
            bs.playerUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.enemyUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.playerUnit.pokemon = new Pokemon(4, 2, "Tackle");
            bs.enemyUnit.pokemon = new Pokemon(4, 3, "Tackle");
            Moves playerMove = bs.playerUnit.pokemon.currentMoves[0];
            Moves enemyMove = bs.enemyUnit.pokemon.currentMoves[0];
            int playerMoveNum = 0;
            int enemyMoveNum = 0;
            System.Random rnd = new System.Random();

            DialogueTextObject myDialogue = (new GameObject("TextObject")).AddComponent<DialogueTextObject>();
            myDialogue.text = "Test String!";
            bs.dialogueText = myDialogue;

            bs.state = BattleState.PLAYERTURN;

            return bs;
        }


        [Test]
        public void AttackXYZ_Dialogue_Text_Tests()
        {

            BattleSystem testBench = AttackXYZ_Setup();


            // should deal damage to enemy with neutral damage attacking move
            Moves playerMove = testBench.playerUnit.pokemon.currentMoves[0];
            System.Random rnd = new FakeRandom();
            double super = 1;
            testBench.UpdateDialogueForDamageAndStatus(playerMove, testBench.playerUnit, testBench.enemyUnit, rnd, false, super);
            String dialogue = "Enemy Charmander took damage...";
            Debug.Log("resulting text is: " + testBench.dialogueText.text);
            Debug.Log("expected text is: " + dialogue);
            Assert.IsTrue(testBench.dialogueText.text == dialogue, "Charmander should just do neutral damage to enemy");


            // should create status effect on enemy with status move

            // should miss on move with 0 accuracy

            // status effect doesnt get applied to status typed pokemon (burned - fire, etc.)


        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

    }
}