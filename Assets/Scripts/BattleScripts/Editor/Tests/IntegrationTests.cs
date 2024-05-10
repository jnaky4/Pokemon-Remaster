// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NUnit.Framework;
// using UnityEngine.TestTools;
// using UnityEditor.SceneManagement;
// using UnityEngine.UI;


// namespace Pokemon
// {
//     public class IntegrationsTests : MonoBehaviour
//     {
//         //Stub implementation to bypass actual Extension manager class.  
//         public class DialogueTextObject : Text
//         {
//             public String text;
//             public String ToString()
//             {
//                 return text;
//             }
//         }

//         public class FakeUnity : MonoBehaviour
//         {

//         }

//         [UnityTest]
//         public IEnumerator DecisionRuns()
//         {
//             /*
//             StartMenu App = new StartMenu();
//             App.Start();

//             GameController.scene = "C:\\Users\\dadam\\source\\repos\\Pokemon-Remaster\\Assets\\Scenes\\BattleScene.unity";
//             EditorSceneManager.OpenScene(GameController.scene);

//             BattleSystem bs = (new GameObject("BattleSystemObject")).AddComponent<BattleSystem>();
//             bs.playerUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
//             bs.enemyUnit = (new GameObject("UnitObject")).AddComponent<Unit>();
//             bs.playerUnit.pokemon = new Pokemon(3, 4, "Ember");
//             bs.enemyUnit.pokemon = new Pokemon(3, 3, "Ember");
//             bs.playerUnit.pokemon.currentMoves[0].accuracy = 100;
//             bs.enemyUnit.pokemon.currentMoves[0].accuracy = 100;
//             Debug.Log("health of player pokemon starting is " + bs.playerUnit.pokemon.current_hp.ToString());


//             DialogueTextObject myDialogue = (new GameObject("TextObject")).AddComponent<DialogueTextObject>();
//             myDialogue.text = "Test String!";
//             Debug.Log("myText in test script is " + myDialogue.text);
//             Text myText = (Text)myDialogue;

//             bs.dialogueText = myText;
//             Debug.Log(bs.enemyUnit.pokemon.name);
//             Debug.Log(bs.playerUnit.pokemon.name);
//             Debug.Log("in test script dialogue text is " + bs.dialogueText);


//             MonoBehaviour myUnity = (new GameObject("UnityObject")).AddComponent<FakeUnity>();
//             myUnity.StartCoroutine(bs.Decision(0));
//             Debug.Log("bs has finished execution");
//             Debug.Log("1 health of player pokemon is " + bs.enemyUnit.pokemon.current_hp.ToString());
//             myUnity.StartCoroutine(bs.Decision(0));
//             Debug.Log("bs has finished execution");
//             Debug.Log("2 health of player pokemon is " + bs.enemyUnit.pokemon.current_hp.ToString());
//             myUnity.StartCoroutine(bs.Decision(0));
//             Debug.Log("bs has finished execution");
//             Debug.Log("3 health of player pokemon is " + bs.enemyUnit.pokemon.current_hp.ToString());
//             myUnity.StartCoroutine(bs.Decision(0));
//             Debug.Log("bs has finished execution");
//             Debug.Log("health of player pokemon is " + bs.enemyUnit.pokemon.current_hp.ToString());

//             yield return new WaitForSecondsRealtime(4); */
//             yield return null;
//         }
//     }
// }