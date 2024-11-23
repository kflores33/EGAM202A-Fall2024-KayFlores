using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]

public class WeaponData : ScriptableObject
{
    public float damageModifier;
    public string weaponName;
    public GameObject weaponPrefab;
}
