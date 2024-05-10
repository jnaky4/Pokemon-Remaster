using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon {
    public class ItemSlotTemplate : MonoBehaviour
    {
        public GameObject UseButton;
        public GameObject TossButton;
        public Items item;

        public void SelectItem()
        {
            DeselectOthers();
            if (item.useable)
            {
                UseButton = transform.Find("Use Button").gameObject;
                UseButton.SetActive(true);
            }
            TossButton = transform.Find("Toss Button").gameObject;
            TossButton.SetActive(true);
        }
        void DeselectOthers()
        {
            var clones = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject itemSlot in clones)
            {
                itemSlot.transform.GetChild(3).gameObject.SetActive(false);
                itemSlot.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
        public void TossItem()
        {
            Items itemInList = GameController.inventory.Find(item => item.name == name);
            bool destroyed = GameController.inventory.Remove(itemInList);
            var panel = GameObject.FindGameObjectWithTag("ItemPanel");
            foreach (Transform itemSlot in panel.transform)
            {
                if(itemSlot.name == name) { Destroy(itemSlot.gameObject); }
            }
            Destroy(this);
        }

        public void UseItem()
        {
            GameObject.FindGameObjectWithTag("ItemPanel").SetActive(false);
            PauseMenu.usingItem = item;
            GameObject MainMenu = GameObject.FindGameObjectWithTag("MainMenuContainer");
            /*PauseMenu.PokeMenu();*/
            MainMenu.transform.GetChild(3).gameObject.SetActive(true);
            if (PauseMenu.usingItem != null)
            {
                var PlayersPokemon = GameObject.FindGameObjectsWithTag("PokemonMenuPlayersPokemon");
                foreach (GameObject pokemon in PlayersPokemon)
                {
                    //activate button on pokemon
                    pokemon.gameObject.GetComponent<Button>().interactable = true;
                    //hide other options for pokemon
                    pokemon.transform.GetChild(3).gameObject.SetActive(false);
                    pokemon.transform.GetChild(4).gameObject.SetActive(false);
                }
            }
        }
    }
}