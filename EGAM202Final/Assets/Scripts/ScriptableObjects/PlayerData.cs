using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerData : ScriptableObject
{
    public float maxHealth;

    public float moveSpeed;
}
