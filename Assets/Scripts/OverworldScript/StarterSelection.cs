using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class StarterSelection : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        [SerializeField] int dexNum;
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject dialogBox;
        [SerializeField] Text text;

        public void Interact(Transform initial)
        {
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, false, () =>
            {
                Time.timeScale = 0f;
                setUpUI();
                chooseStarter();
            }));
        }


        public void setUpUI()
        {
            Time.timeScale = 0f;
            dialogBox.SetActive(true);
            buttons.SetActive(true);
            text.text = "Choose " + name + " as your starter?";
        }

        public void chooseStarter()
        {
            var starter = GetComponentInParent<StarterButtons>();
            starter.name = name;
            switch (dexNum)
            {
                case 1:
                    starter.pokemon = new Pokemon(1, 5, "Tackle", "Growl");
                    //starter.pokemon = new Pokemon(1, 19, "Leech Seed", "Vine Whip", "Poison Powder", "Razor Leaf", 7900);
                    break;
                case 4:
                    starter.pokemon = new Pokemon(4, 5, "Scratch", "Growl", "Ember");
                    //starter.pokemon = new Pokemon(4, 19, "Poison Gas", "Ember", "Tail Whip", "Bite", 7900);
                    break;
                case 7:
                    starter.pokemon = new Pokemon(7, 9, "Tackle", "Tail Whip", "Bubble", null, 990);
                    //starter.pokemon = new Pokemon(7, 19, "Water Gun", "Bubble", "Leer", "Crabhammer", 7900);
                    break;
                    
            }
        }
    }
}
