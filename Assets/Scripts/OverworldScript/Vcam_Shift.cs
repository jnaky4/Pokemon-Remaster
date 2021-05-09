using UnityEngine;

namespace Pokemon
{
    public class Vcam_Shift : MonoBehaviour
    {
        //GameObject startCam;
        //GameObject nextCam;
        public string startString;
        public string endString;
        public string newLocation;
        [SerializeField] GameObject startCam;
        [SerializeField] GameObject nextCam;

        public void Start()
        {
            //startCam = GameObject.Find(startString);
            //nextCam = GameObject.Find(endString);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Activate cam shift");
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                nextCam.SetActive(true);

                //if (startCam != null)
                    startCam.SetActive(false);
                GameController.location = newLocation;
                GameController.music = newLocation;
            }
        }
    }
}
