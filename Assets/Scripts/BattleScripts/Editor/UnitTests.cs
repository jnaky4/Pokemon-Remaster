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
            public int secretField = 1;
            override public int Next()
            {
                return secretField;
            }
            public void updateSecret(int value)
            {
                secretField = value;
            }
        }

        public class FakeHUD : BattleHUD
        {
            override public void SetStatus(Pokemon poke)
            {
                // do nothing
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
            bs.playerUnit.pokemon = new Pokemon(4, 2, "Tackle", "Thunder Wave");
            bs.enemyUnit.pokemon = new Pokemon(4, 3, "Tackle");
            Moves playerMove = bs.playerUnit.pokemon.currentMoves[0];
            Moves enemyMove = bs.enemyUnit.pokemon.currentMoves[0];
            System.Random rnd = new System.Random();

            DialogueTextObject myDialogue = (new GameObject("TextObject")).AddComponent<DialogueTextObject>();
            myDialogue.text = "Test String!";
            bs.dialogueText = myDialogue;

            BattleHUD enemyHUD = (new GameObject("EnemyHUD")).AddComponent<FakeHUD>();
            bs.enemyHUD = enemyHUD;

            bs.state = BattleState.PLAYERTURN;



            return bs;
        }


        [Test]
        public void AttackXYZ_Dialogue_Text_Tests()
        {

            BattleSystem testBench = AttackXYZ_Setup();
            System.Random rnd = new FakeRandom();
            double super = 1;
            String dialogue = "";

            // should deal damage to enemy with neutral damage attacking move
            Moves playerMove = testBench.playerUnit.pokemon.currentMoves[0];

            testBench.AttackDialogue(playerMove, testBench.playerUnit, testBench.enemyUnit, super, null, false, 0);
            dialogue = "Enemy Charmander took damage...";
            Debug.Log("resulting text is: " + testBench.dialogueText.text);
            Debug.Log("expected text is: " + dialogue);
            Assert.IsTrue(testBench.dialogueText.text == dialogue, "Charmander should just do neutral damage to enemy");

            // should create status effect on enemy with status move 
            playerMove = testBench.playerUnit.pokemon.currentMoves[1]; //Thunderwave
            testBench.AttackDialogue(playerMove, testBench.playerUnit, testBench.enemyUnit, super, null, false, 0);
            dialogue = "Enemy Charmander became Paralyzed!";
            Debug.Log("resulting text is: " + testBench.dialogueText.text);
            Debug.Log("expected text is: " + dialogue);
            Assert.IsTrue(testBench.dialogueText.text == dialogue, "Charmander should be paralyzed by Thunder Wave");

            // Thunderwave should fail to re-apply to target if already paralyzed
            playerMove = testBench.playerUnit.pokemon.currentMoves[1]; //Thunderwave
            testBench.enemyUnit.pokemon.statuses = new ArrayList(new[] { "Paralysis"});
            testBench.AttackDialogue(playerMove, testBench.playerUnit, testBench.enemyUnit, super, null, false, 0);
            dialogue = "Enemy Charmander is already Paralyzed!";
            Debug.Log("resulting text is: " + testBench.dialogueText.text);
            Debug.Log("expected text is: " + dialogue);
            Assert.IsTrue(testBench.dialogueText.text == dialogue, "Charmander should be paralyzed by Thunder Wave");

            // status effect doesnt get applied to status typed pokemon (burned - fire, etc.)
            // todo implement IsImmune() in Utility class - this function is just a stub so this test should fail until it is fixed
            playerMove = testBench.playerUnit.pokemon.currentMoves[1]; //Thunderwave
            testBench.enemyUnit.pokemon = new Pokemon(25, 3, "Tackle"); //Pikachu... comment even necessary?
            testBench.AttackDialogue(playerMove, testBench.playerUnit, testBench.enemyUnit, 0, null, false, 0);
            dialogue = "Enemy Pikachu is immune!";
            Debug.Log("resulting text is: " + testBench.dialogueText.text);
            Debug.Log("expected text is: " + dialogue);
            Assert.IsTrue(testBench.dialogueText.text == dialogue, "Pikachu can't be paralyzed by Thunder Wave");


        }

        [Test]
        public void AttackXYZ_Catch_Pokemon_Tests()
        {
            BattleSystem testBench = AttackXYZ_Setup();
            FakeRandom root = new FakeRandom();

            // set rnd to 0, ensure the pokeball does not catch the pokemon
            // BattleState -> enemyTurn
            // 0 shakes
            // caught -> false

            int ball = 1; // pokeball
            testBench.ballShakes = -1;
            int rnd = 0;
            //testBench.DetermineCatchResult(ball, testBench.enemyUnit, rnd);
            Debug.Log("ballShakes: " + testBench.ballShakes);
            Debug.Log("ballShakes: " + testBench.state);
            Assert.IsTrue(testBench.ballShakes == 3, "pokemon should be caught, so 3 ball shakes");
            Assert.IsTrue(testBench.state == BattleState.CAUGHTPOKEMON, "state was updated after pokemon is caught");


            ball = 3; // ultra-ball - modifier of 1.0
            // f = 256 * ballModifier (1 - 0.5 * currentHP / maxHP) * catchRate + statusModifer;
            // f = 256 * 1.0 * 0.5 * 1.0 + 0 = 128
            // set rnd = 129
            // ballShakes = 3*(256-129) / (256 - 128) = 127/128 -> 3 shakes
            testBench.ballShakes = -1;
            rnd = 128;
           // testBench.DetermineCatchResult(ball, testBench.enemyUnit, rnd);
            Assert.IsTrue(testBench.ballShakes == 3, "rnd was set to 128, so 3 ball shakes");
            Assert.IsTrue(testBench.state == BattleState.ONLYENEMYTURN, "state was updated after pokemon breaks free");

            ball = 3; // ultra-ball - modifier of 1.0
            // f = 256 * ballModifier (1 - 0.5 * currentHP / maxHP) * catchRate + statusModifer;
            // f = 256 * 1.0 * 0.5 * 1.0 + 0 = 128
            // set rnd = 200
            // ballShakes = 3*(256-200) / (256 - 128) = 56/128 -> 1.31 -> 2 shakes
            testBench.ballShakes = -1;
            rnd = 200;
            //testBench.DetermineCatchResult(ball, testBench.enemyUnit, rnd);
            Assert.IsTrue(testBench.ballShakes == 2, "rnd was set to 128 for no-status full hp pokemon, so 2 ball shakes");
            Assert.IsTrue(testBench.state == BattleState.ONLYENEMYTURN, "state was updated after pokemon breaks free");

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

        public BattleSystem PokeballSpriteFactory_Setup()
        {
            // Use the Assert class to test conditions
            Main App = (new GameObject("MainLoader")).AddComponent<Main>();
            App.Start();

            BattleSystem bs = (new GameObject("BattleSystemObject")).AddComponent<BattleSystem>();
            bs.playerUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.enemyUnit = (new GameObject("UnitObject")).AddComponent<Unit>();

            bs.PokeballSpriteFactory();
            DialogueTextObject myDialogue = (new GameObject("TextObject")).AddComponent<DialogueTextObject>();
            myDialogue.text = "Test String!";
            bs.dialogueText = myDialogue;

            BattleHUD enemyHUD = (new GameObject("EnemyHUD")).AddComponent<FakeHUD>();
            bs.enemyHUD = enemyHUD;

            bs.state = BattleState.PLAYERTURN;



            return bs;
        }
    }
}