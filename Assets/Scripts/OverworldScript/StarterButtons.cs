using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{ 
    public class StarterButtons : MonoBehaviour
    {
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject dialogBox;
        public string name = "";
        public Pokemon pokemon;

        public void yes()
        {
            Debug.Log(name + " has been chosen.");
            Time.timeScale = 1f;
            if (pokemon != null)
            {
                Debug.Log("Set pokemon");
                GameController.playerPokemon[0] = pokemon;
                GameController.player.starter = pokemon.dexnum;
            }

            GameController.state = GameState.Roam;
            dialogBox.SetActive(false);
            buttons.SetActive(false);
        }

        public void no()
        {
            Debug.Log(name + " has been denied");
            Time.timeScale = 1f;
            GameController.state = GameState.Roam;
            dialogBox.SetActive(false);
            buttons.SetActive(false);
        }
    }
}
