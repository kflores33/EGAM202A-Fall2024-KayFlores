using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish",order = 1)]
public class FishData : ScriptableObject
{
    [Header("Values (Customizable)")]
    [Header("Time Between Moving")]
    public float minTime;
    public float maxTime;

    [Header("Distance Variables")]
    public float minDistance;
    public float maxDistance;

    [Header("Speed of Fish")] // the lower the value, the faster the fish is
    public float speed;

    [Header("Health")]
    public float healthValue = 20;

    [Header("Name")]
    public string fishName;
}
