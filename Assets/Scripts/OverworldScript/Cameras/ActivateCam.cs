using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ActivateCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        vcam.LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
