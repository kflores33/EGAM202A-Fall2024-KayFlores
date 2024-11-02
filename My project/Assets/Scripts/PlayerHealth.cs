using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=I2Uo8eEmSFQ&list=PLcRSafycjWFcwCxOHnc83yA0p4Gzx0PTM&index=4&t=1s

    [SerializeField]
    public int currentHealth, maxHealth;

    public PlayerActions player;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

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
    public void GetHit(int amount, GameObject sender)
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
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        EnemyHealth enemy = col.gameObject.GetComponent<EnemyHealth>();
        ProjectileScript projectile = col.gameObject.GetComponent<ProjectileScript>();

        if (enemy != null)
        {
            if (player.currentState != PlayerActions.ActionStates.Parry)
            {
                GetHit(1, enemy.gameObject);
            }
        }
        if (projectile != null) 
        {
            if (projectile.canDamageEnemy)
            {
                return;
            }
            else
            {
                GetHit(1, projectile.gameObject);
                Destroy(projectile.gameObject);
            }
        }
    }
}