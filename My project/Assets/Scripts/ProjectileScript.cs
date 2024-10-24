using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float life = 10f;

    public Transform playerTransform;

    public Rigidbody rb;

    public bool hasBeenDeflected;
    // could probably change collision layer depending on if true or not using a switch
    public bool canDamageEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        rb.useGravity = false;
        canDamageEnemy = false;
        Destroy(gameObject, life);
    }

    public void DeflectProjectile(Vector3 reflected, AuraTriggerDetection playerAura)
    {
        //Debug.Log("fuckin finally deflected omg");

        rb.transform.rotation = Quaternion.LookRotation(reflected);

        rb.AddForce(transform.forward * playerAura.projectileDeflectSpeed, ForceMode.Impulse);

        hasBeenDeflected = true;
        canDamageEnemy = true;
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
}
