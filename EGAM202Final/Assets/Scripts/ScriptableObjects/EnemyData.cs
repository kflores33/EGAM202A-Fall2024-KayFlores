using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Health Thresholds")]
    public float maxHealth;

    public float yellowThresholdMax; // if below this number, enemy is in yellow light state
    public float redThresholdMax; // if below this number, enemy is in red light state

    [Header("Misc Variables")]
    public float moveSpeed;

    public string enemyName;
}
