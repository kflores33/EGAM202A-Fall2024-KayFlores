using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=I2Uo8eEmSFQ&list=PLcRSafycjWFcwCxOHnc83yA0p4Gzx0PTM&index=4&t=1s

    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<AuraTriggerDetection> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    public bool isDead = false;

    [Header("UI")]
    public Slider healthBar;
    private void Start()
    {

        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;

        healthBar.value = currentHealth;
    }

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
        healthBar.value = currentHealth;

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
