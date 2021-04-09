using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class SceneSpawn : MonoBehaviour
    {
        public string prevScene;
        public Vector3 newPosition;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(GameObject.Find("Player").transform.position);
            Debug.Log("Spawn Position: " + transform.position);

            if (GameController.location == prevScene)
            {
                GameController.location = SceneManager.GetActiveScene().name;

                Debug.Log("Setting new position");
                GameObject.Find("Player").transform.position = newPosition;
            }
            
        }
    }
}
