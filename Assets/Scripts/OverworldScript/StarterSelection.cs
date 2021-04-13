using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class StarterSelection : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        [SerializeField] new string name;
        [SerializeField] int dexNum;
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject dialogBox;
        [SerializeField] Text text;

        public void Interact(Transform initial)
        {
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
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
                    starter.pokemon = new Pokemon(1, 18, "Leech Seed", "Vine Whip", "Growl", "Razor Leaf");
                    break;
                case 4:
                    starter.pokemon = new Pokemon(4, 18, "Poison Gas", "Ember", "Tail Whip", "Bite");
                    break;
                case 7:
                    starter.pokemon = new Pokemon(7, 18, "Water Gun", "Bubble", "Splash", "Crabhammer");
                    break;
            }
        }
    }
}
