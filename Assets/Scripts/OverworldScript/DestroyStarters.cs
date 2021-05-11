using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class DestroyStarters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (GameController.starterChosen)
            {
                Destroy(this);
            }
    }
    }
}
