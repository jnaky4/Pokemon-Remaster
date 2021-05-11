using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class Healing : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        [SerializeField] new string name;

        public void Interact(Transform initial)
        {
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
