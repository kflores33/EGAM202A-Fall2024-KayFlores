using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=I2Uo8eEmSFQ&list=PLcRSafycjWFcwCxOHnc83yA0p4Gzx0PTM&index=4&t=1s

    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<AuraTriggerDetection> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    public bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }
    public void GetHit(int amount, AuraTriggerDetection sender)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(this.gameObject);
        }
    }
}
