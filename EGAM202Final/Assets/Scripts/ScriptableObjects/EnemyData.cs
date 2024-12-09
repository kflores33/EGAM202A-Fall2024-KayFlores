using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Health Thresholds")]
    public int maxHealth;

    public int yellowThresholdMax; // if below this number, enemy is in yellow light state
    public int redThresholdMax; // if below this number, enemy is in red light state

    [Header("Misc Variables")]
    public int moveSpeed;

    public string enemyName;
}
