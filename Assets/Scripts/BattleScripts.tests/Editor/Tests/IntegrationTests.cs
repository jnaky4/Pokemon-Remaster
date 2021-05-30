using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using UnityEngine.UI;


namespace Pokemon
{
    public class IntegrationsTests : MonoBehaviour
    {
        //Stub implementation to bypass actual Extension manager class.  
        public class DialogueTextObject : Text
        {
            public String text;
            public String ToString()
            {
                return text;
            }
        }

        public class FakeUnity : MonoBehaviour
        {
            public Pokemon[] enemyPokemon;
            public Pokemon[] playerPokemon;
        }

        public class FakeHUD : BattleHUD
        {
            public override void CrossOutBall(int i)
            {
                Debug.Log("I am a printer!!");
            }
        }

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        [UnityTest]
        public IEnumerator IfPokemonDeadGivesCorrectExp()
        {
            Main App = (new GameObject("MainLoader")).AddComponent<Main>();
            App.Start();
            BattleSystem bs = (new GameObject("BattleSystemObject")).AddComponent<BattleSystem>();

            bs.playerUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.enemyUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
            bs.playerUnit.pokemon = new Pokemon(3, 4, "Ember");
            bs.enemyUnit.pokemon = new Pokemon(3, 3, "Ember");
            bs.enemyUnit.pokemon.current_hp = 0;

            Pokemon[] playerPokemon = new Pokemon[] { bs.playerUnit.pokemon };
            Pokemon[] enemyPokemon = new Pokemon[] { bs.enemyUnit.pokemon };
            GameController.playerPokemon = playerPokemon;
            GameController.opponentPokemon = enemyPokemon;
            GameController.isCatchable = false;

            Debug.Log("Pokemon exp "+ bs.playerUnit.pokemon.current_exp);
            int startingExp = bs.playerUnit.pokemon.current_exp;

            BattleHUD playerHUD = (new GameObject("PlayerHUD")).AddComponent<FakeHUD>();
            BattleHUD enemyHUD = (new GameObject("EnemyHUD")).AddComponent<FakeHUD>();
            bs.playerHUD = playerHUD;
            bs.enemyHUD = enemyHUD;

            DialogueTextObject myDialogue = (new GameObject("TextObject")).AddComponent<DialogueTextObject>();
            myDialogue.text = "Test String!";
            bs.dialogueText = myDialogue;

            MonoBehaviour myUnity = (new GameObject("UnityObject")).AddComponent<FakeUnity>();
            myUnity.StartCoroutine(bs.IfPokemonDead(bs.enemyUnit.pokemon.IsFainted(), "enemy", GameController.opponentPokemon));
            Debug.Log("Pokemon exp " + bs.playerUnit.pokemon.current_exp);
            int finalExp = bs.playerUnit.pokemon.current_exp;
            int expGained = finalExp - startingExp;

            // let this be a trainer battle
            double multiplier = (GameController.isCatchable) ? 1 : 1.5;
            int exp = bs.playerUnit.pokemon.gain_exp(bs.enemyUnit.pokemon.level, bs.enemyUnit.pokemon.pokedex_entry.base_exp, 1, multiplier);
            Debug.Log("direct exp from gain_exp(): " + exp);
            Assert.IsTrue(expGained == exp, "exp from IfPokemonDead is compared to direct exp calculation");

            yield return null; 

        }
    }
}