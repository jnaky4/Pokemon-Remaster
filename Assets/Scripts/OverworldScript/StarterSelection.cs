using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class StarterSelection : MonoBehaviour, Interactable
    {
        [SerializeField] Dialog dialog;
        [SerializeField] new string name;
        [SerializeField] GameObject buttons;

        public void Interact(Transform initial)
        {
            StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
            {
                buttons.SetActive(true);
            }));
        }
    }
}
