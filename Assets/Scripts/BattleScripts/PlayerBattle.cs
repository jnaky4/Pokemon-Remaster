using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THE PLAYER OBJECT TO USE IN BATTLE. LEGACY CODE BUT IT NEEDS TO STAY.
/// </summary>
public class PlayerBattle : MonoBehaviour
{
    /// <summary>
    /// My name
    /// </summary>
    public string myName;

    /// <summary>
    /// The poke balls
    /// </summary>
    public bool pokeBalls = true;
    /// <summary>
    /// The great balls
    /// </summary>
    public bool greatBalls = true;
    /// <summary>
    /// The ultra balls
    /// </summary>
    public bool ultraBalls = true;
    /// <summary>
    /// The master balls
    /// </summary>
    public bool masterBalls = true;

    /// <summary>
    /// The number poke balls
    /// </summary>
    public int numPokeBalls = 10;
    /// <summary>
    /// The number great balls
    /// </summary>
    public int numGreatBalls = 9;
    /// <summary>
    /// The number ultra balls
    /// </summary>
    public int numUltraBalls = 0;
    /// <summary>
    /// The number master balls
    /// </summary>
    public int numMasterBalls = 0;
}
