using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class Healing : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        public void Interact(Transform initial)
        {
            if (name == "Mom" && !GameController.starterChosen)
            {
                dialog.Lines[0] = "The professor said he had a pokemon for you in his lab.";
                dialog.Lines[1] = "Make sure you visit him before you try to leave town!";
            }
            character.LookTowards(initial.position);
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, false, () =>
            {
                Heal();
            }));
        }

        public void Heal()
        {
            foreach (var pokemon in GameController.playerPokemon)
            {
                if (pokemon != null)
                {
                    pokemon.current_hp = pokemon.max_hp;
                    pokemon.statuses.Clear();

                    foreach (var move in pokemon.currentMoves)
                    {
                        if (move != null)
                            move.current_pp = move.maxpp;
                    }
                }
            }
        }
    }
}
