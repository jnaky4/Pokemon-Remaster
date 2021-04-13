using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterButtons : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    public string name = "";

    public void yes()
    {
        Debug.Log(name + " has been chosen.");
        buttons.SetActive(false);
        
    }

    public void no()
    {
        Debug.Log(name + " has been denied");
        buttons.SetActive(false);
    }
}
