using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerActions;

public class AuraTriggerDetection : MonoBehaviour
{
    // referenced this video: https://www.youtube.com/watch?v=LTegHf579no

    public bool hitWhileParry;

    public EnemyAI currentEnemy;
    public Transform player;

    public UnityEvent OnAttackPerformed;

    public float projectileDeflectSpeed;

    public  List<EnemyHealth> enemies = new List<EnemyHealth>();


    // maybe use spherecast instead of trigger (and up knockback!!)

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();

            //enemies.Add(health);

            //foreach (EnemyHealth enemy in enemies)
            //{
            //    enemy.GetHit(1, this.gameObject);
            //    OnAttackPerformed?.Invoke();

            //    break;
            //}

            health.GetHit(1, this.gameObject);

            OnAttackPerformed?.Invoke();
            hitWhileParry = true;
        }
        // handles projectile deflection
        if (collision.gameObject.GetComponent<ProjectileScript>() != null) 
        { 
            ProjectileScript projectile = collision.gameObject.GetComponent<ProjectileScript>();

            Vector3 direction = ((projectile.transform.position + projectile.transform.forward) - projectile.transform.position).normalized;

            //// reflects direction of player's vector3
            //Vector3 reflected = Vector3.Reflect(direction, player.transform.forward);

            // does the actual reflecting
            projectile.DeflectProjectile( this);

            // after projectile is reflected,
            if (projectile.hasBeenDeflected)
            {
                OnAttackPerformed?.Invoke();
                hitWhileParry = true;
            }
        }
    }
}
