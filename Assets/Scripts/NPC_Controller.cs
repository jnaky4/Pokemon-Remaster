using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour, Interactable
{
    public string Dialog;

    public void Interact()
    {
        Debug.Log(Dialog);
    }
}
