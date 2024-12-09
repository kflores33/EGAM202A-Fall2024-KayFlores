using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;

    int currentHealth;

    MeshRenderer meshRenderer;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    public enum HealthState
    {
        Max,
        AtRisk
    }
    public HealthState currentHealthState;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentHealth = enemyData.maxHealth;

        currentHealthState = HealthState.Max;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentHealthState)
        {
            case HealthState.Max:
                UpdateMaxHealth(); break;
            case HealthState.AtRisk:
                UpdateAtRisk(); break;
        }
    }

    public void GetHit(GameObject sender)
    {
        if (sender.GetComponent<CombatHandler>() != null)
        {
            int damageTaken = sender.GetComponent<CombatHandler>().currentAttack.damage;
            ChangeColorOnHit();

            currentHealth -= damageTaken;
        }
    }

    public void ChangeColorOnHit()
    {
        meshRenderer.material.color = Color.red;

        Invoke("ResetColor", 0.3f);
    }
    public void ResetColor()
    {
        meshRenderer.material.color = Color.white;
    }

    void UpdateMaxHealth()
    {
        if (currentHealth < enemyData.redThresholdMax)
        {
            meshRenderer.material.color = Color.red;
            currentHealthState = HealthState.AtRisk;
        }
    }
    void UpdateAtRisk()
    {

    }
}
