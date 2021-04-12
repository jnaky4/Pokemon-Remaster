using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSelection : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] new string name;

    public void Interact(Transform initial)
    {
        StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
        {
            Debug.Log("Dialog is finished.");
        }));
    }
}
