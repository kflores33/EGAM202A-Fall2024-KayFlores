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

    public  List<EnemyHealth> enemies = new List<EnemyHealth>();

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
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();

            enemies.Add(health);

            foreach (EnemyHealth enemy in enemies)
            {
                enemy.GetHit(1, this.gameObject.GetComponent<AuraTriggerDetection>());

                break;
            }

            OnAttackPerformed?.Invoke();
            hitWhileParry = true;
        }
    }
}
