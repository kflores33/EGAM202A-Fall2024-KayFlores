using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;

    public int currentHealth;

    MeshRenderer meshRenderer;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    public bool canGetHit;

    public enum HealthState
    {
        Max,
        AtRisk
    }
    public HealthState currentHealthState;

    Color currentColor;
    Color maxColor = Color.green;
    Color halfColor = Color.yellow;
    Color atRiskColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentHealth = enemyData.maxHealth;
        canGetHit = true;

        currentColor = maxColor;
        meshRenderer.material.color = currentColor;

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
        if (canGetHit)
        {
            if (sender.GetComponent<CombatHandler>() != null)
            {
                int damageTaken = sender.GetComponent<CombatHandler>().currentAttack.damage;
                OnHit();

                currentHealth -= damageTaken;
                canGetHit = false;
            }
        }
    }

    void CanGetHit()
    {
        canGetHit = true;
    }

    public void OnHit()
    {
        meshRenderer.material.color = Color.red;

        Invoke("CanGetHit", 0.3f);
        Invoke("ResetColor", 0.3f);
    }
    public void ResetColor()
    {
        meshRenderer.material.color = currentColor;
    }

    void UpdateMaxHealth()
    {
        if (currentHealth < enemyData.redThresholdMax)
        {
            //meshRenderer.material.color = Color.red;

            currentColor = atRiskColor;
            GetComponentInChildren<ButtonPromptController>().ShowButtonPrompt();
            currentHealthState = HealthState.AtRisk;
        }
        else if (currentHealth < enemyData.yellowThresholdMax)
        {
            currentColor = halfColor;
        }
    }
    void UpdateAtRisk()
    {
        canGetHit = false;

        if (Input.GetKeyDown(GetComponentInChildren<ButtonPromptController>().currentKey))
        {
            Destroy(this.gameObject);
        }
    }
}
