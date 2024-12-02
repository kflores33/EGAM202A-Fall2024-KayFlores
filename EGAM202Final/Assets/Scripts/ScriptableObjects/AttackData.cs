using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class AttackData : ScriptableObject
{
    public AnimatorOverrideController animatorOV;

    public float damage;

    public float knockbackStrength;
}
