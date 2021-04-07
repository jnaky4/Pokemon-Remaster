using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class PlayerData : MonoBehaviour
    {
        public string name = "Red";

        public int pokeBalls = 10;
        public int greatBalls = 10;
        public int ultraBalls = 0;
        public int masterBalls = 0;

        public bool displayPokeBalls = true;
        public bool displayGreatBalls = true;
        public bool displayUltraBalls = false;
        public bool displayMasterBalls = false;
    }
}
