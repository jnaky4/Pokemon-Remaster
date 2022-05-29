using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon {
    public class ItemSlotTemplate : MonoBehaviour
    {
        public GameObject UseButton;
        public GameObject TossButton;
        public void SelectItem()
        {
            DeselectOthers();
            UseButton = transform.Find("Use Button").gameObject;
            UseButton.SetActive(true);
            TossButton = transform.Find("Toss Button").gameObject;
            TossButton.SetActive(true);
        }
        void DeselectOthers()
        {
            var clones = GameObject.FindGameObjectsWithTag(this.tag);
            foreach (GameObject itemSlot in clones)
            {
                itemSlot.transform.GetChild(3).gameObject.SetActive(false);
                itemSlot.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
        public void TossItem()
        {
            Debug.Log("CLICKED");
            string name = transform.GetChild(0).gameObject.GetComponent<Text>().text;
            Items itemInList = GameController.inventory.Find(item => item.name == name);
            Debug.Log(itemInList.name);
            bool destroyed = GameController.inventory.Remove(itemInList);
            Debug.Log(destroyed);
            var panel = GameObject.FindGameObjectWithTag("ItemPanel");
            foreach (Transform itemSlot in panel.transform)
            {
                if(itemSlot.name == name) { Destroy(itemSlot.gameObject); }
            }
            Destroy(this);
        }
    }
}