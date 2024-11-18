using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerActions;

public class AuraTriggerDetection : MonoBehaviour
{
    // referenced this video: https://www.youtube.com/watch?v=LTegHf579no

    public bool hitWhileParry;
    public bool adjustBulletPosition;

    public EnemyAI currentEnemy;
    public Transform player;

    public UnityEvent OnAttackPerformed;

    public float projectileDeflectSpeed;

    public  List<EnemyHealth> enemies = new List<EnemyHealth>();

    // deflector shit
    [SerializeField] private enum DeflectMethod 
    {
        inverse,
        reflected,
        aimed
    }
    [SerializeField] private DeflectMethod deflectMethod;

    MeshCollider thisCollider;
    // maybe use spherecast instead of trigger (and up knockback!!)

    private void Awake()
    {
        thisCollider = GetComponentInChildren<MeshCollider>();
    }

    private void Update()
    {
        //transform.position = player.GetComponent<PlayerMovement>().pivot.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // need to revise how parry works!!!
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();

            health.GetHit(1, this.gameObject);

            OnAttackPerformed?.Invoke();
            hitWhileParry = true;
        }

        // deflecting bullets
        if (collision.gameObject.GetComponent<ProjectileScript>() != null)
        {
            ProjectileScript projectile = collision.gameObject.GetComponent<ProjectileScript>();

            Vector3 direction = ((projectile.transform.position + projectile.transform.forward) - projectile.transform.position).normalized; // gets direction of colliding object
            Vector3 inverse = direction * -1; // find the inverse of the given direction
            Vector3 position = projectile.transform.position;
            switch (deflectMethod) 
            {
                case DeflectMethod.inverse: // redirects the bullet back towards the target
                    Debug.DrawRay(position, inverse, Color.magenta, 100f);
                    projectile.transform.rotation = Quaternion.LookRotation(inverse); // make the object face the opposite direction it was traveling in
                    
                    if (!projectile.IsUpdatingTravel()) // only needs to be done if the travel direction and speed are not constantly updated
                    {
                        projectile.rb.velocity *= -1;
                    }

                    //OnAttackPerformed?.Invoke();
                    hitWhileParry = true;

                    projectile.hasBeenDeflected = true;
                    projectile.canDamageEnemy = true;

                    break;

                case DeflectMethod.reflected: 
                    Debug.DrawRay(position, Vector3.Reflect(direction, transform.forward), Color.magenta, 100f);
                    Vector3 reflected = Vector3.Reflect(direction, transform.forward); // in direction, in normal (quad faces outward/forward)
                    projectile.transform.rotation = Quaternion.LookRotation(reflected);

                    float mag = projectile.rb.velocity.magnitude; // size of the vector

                    if (!projectile.IsUpdatingTravel()) // only needs to be done if the travel direction and speed are not constantly updated
                    {
                        projectile.rb.velocity = reflected.normalized * mag;
                    }

                    //OnAttackPerformed?.Invoke();
                    hitWhileParry = true;

                    projectile.hasBeenDeflected = true;
                    projectile.canDamageEnemy = true;

                    break;
                //do not use this one
                case DeflectMethod.aimed:
                    Vector3 aimDirection = player.GetComponent<PlayerMovement>().pivot.position;
                    projectile.transform.rotation = Quaternion.LookRotation(aimDirection);
                    float mag1 = projectile.rb.velocity.magnitude;
                    if (!projectile.IsUpdatingTravel())
                    {
                        projectile.rb.velocity = aimDirection * mag1;
                    }
                    if (adjustBulletPosition)
                    {
                        projectile.transform.position = transform.position;
                        Debug.DrawRay(projectile.transform.position, aimDirection, Color.magenta, 100f);
                    }
                    else
                    {
                        Debug.DrawRay(position, aimDirection, Color.magenta, 100f);
                    }
                    break;
            }
        }
    }
}
