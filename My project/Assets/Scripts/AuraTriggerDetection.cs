using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerActions;

public class AuraTriggerDetection : MonoBehaviour
{
    public bool hitWhileParry;

    public EnemyAI currentEnemy;

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
                hitWhileParry = true;
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                currentEnemy = enemy;
            }
    }
    private void OnTriggerExit(Collider exit)
    {
        if (exit.gameObject.GetComponent<EnemyAI>() != null)
        {
            hitWhileParry = false;
            currentEnemy = null;
        }
    }
}
