using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Controller : MonoBehaviour, Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public bool dialogActive;

    public void Interact()
    {
        if (dialogBox.activeInHierarchy)
            dialogBox.SetActive(false);
        else
        {
            dialogBox.SetActive(true);
            dialogText.text = dialog;
        }
        Debug.Log(dialog);
    }
}
