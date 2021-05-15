using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class SelectStarter : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        [SerializeField] int dexNum;
        //[SerializeField] GameObject buttons;
        //[SerializeField] GameObject dialogBox;
        //[SerializeField] Text text;

        public void Interact(Transform initial)
        {
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, true, () =>
            {
                if (GameController.starterChosen)
                    chooseStarter();
            }));
        }

        /*
        public void setUpUI()
        {
            Time.timeScale = 0f;
            dialogBox.SetActive(true);
            buttons.SetActive(true);
            text.text = "Choose " + name + " as your starter?";
        }*/

        public void chooseStarter()
        {
            //var starter = GetComponentInParent<StarterButtons>();
            //var starte
            //starter.name = name;
            switch (dexNum)
            {
                case 1:
                    GameController.playerPokemon[0] = new Pokemon(1, 5, "Tackle", "Growl");
                    GameController.player.starter = 1;
                    break;
                case 4:
                    GameController.playerPokemon[0] = new Pokemon(4, 5, "Scratch", "Growl");
                    GameController.player.starter = 4;
                    break;
                case 7:
                    GameController.playerPokemon[0] = new Pokemon(7, 9, "Tackle", "Tail Whip");
                    //GameController.playerPokemon[0] = new Pokemon(7, 5, "Tackle", "Tail Whip");
                    GameController.player.starter = 7;
                    break;

            }
            Destroy(GameObject.Find("Starters"));
        }
    }
}

