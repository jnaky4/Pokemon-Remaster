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

        public void Interact(Transform initial)
        {
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
            {
                buttons.SetActive(true);
                chooseStarter();
            }));
        }

        public void chooseStarter()
        {
            var starter = GetComponentInParent<StarterButtons>();
            starter.name = name;
        }
    }
}
