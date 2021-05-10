using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class PlayerData : MonoBehaviour
    {
        public string name = "Red";

        public int starter;

        public int pokeBalls = 10;
        public int greatBalls = 10;
        public int ultraBalls = 10;
        public int masterBalls = 10;

        public bool displayPokeBalls = true;
        public bool displayGreatBalls = true;
        public bool displayUltraBalls = true;
        public bool displayMasterBalls = true;

        public int money = 0;
        public float time = 0;
    }
}