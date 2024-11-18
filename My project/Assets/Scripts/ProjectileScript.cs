using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float life = 10f;
    public float bulletSpeed;

    public Transform playerTransform;
    public Rigidbody rb;

    public bool hasBeenDeflected;

    public bool canDamageEnemy;

    [SerializeField] private bool useGravity, updateTravel, useVelocity;

    // could probably change collision layer depending on if true or not using a switch

    // Start is called before the first frame update
    void Awake()
    {
        rb.useGravity = useGravity;
        canDamageEnemy = false;
        Destroy(gameObject, life);

        if (!updateTravel) rb.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {
        if (updateTravel)
        {
            if (useVelocity)
            {
                rb.velocity = transform.forward * bulletSpeed;
            }
            else
            {
                transform.position += transform.forward * bulletSpeed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            if (!canDamageEnemy) { return; }
            else
            {
                enemy.GetHit(3, this.gameObject);
                Destroy(this.gameObject);
            }
        }
        if (collision.collider.GetComponent<PlayerHealth>() != null)
        {
            PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
            if (canDamageEnemy) { return; }
            else 
            {
                Destroy(this.gameObject);   
            }
        }
    }

    public bool IsUpdatingTravel()
    {
        return updateTravel;
    }
}
