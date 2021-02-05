using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Vcam_Shift : MonoBehaviour
{
    GameObject startCam;
    GameObject nextCam;
    public string startString;
    public string endString;

    public void Start()
    {
        startCam = GameObject.Find(startString);
        nextCam = GameObject.Find(endString);
    }

    public void OnTriggerEnter2D(Collider2D other)
    { 

        if (other.CompareTag("Player") && !other.isTrigger)
        {
            nextCam.SetActive(true);
            startCam.SetActive(false);
        }
    }
}
