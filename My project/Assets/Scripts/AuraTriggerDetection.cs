using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerActions;

public class AuraTriggerDetection : MonoBehaviour
{
    public bool hitWhileParry;

    public EnemyAI currentEnemy;

    public UnityEvent OnAttackPerformed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

            EnemyHealth health;
            if (health = enemy.GetComponent<EnemyHealth>()) 
            {
                health.GetHit(1, this.gameObject.GetComponent<AuraTriggerDetection>());
            }

            hitWhileParry = true;
        }
    }
}
