using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float life = 10f;

    public Transform playerTransform;

    public Rigidbody rb;

    public bool hasBeenDeflected;

    // Start is called before the first frame update
    void Awake()
    {
        rb.useGravity = false;
        Destroy(gameObject, life);
    }

    public void DeflectProjectile(Vector3 reflected, AuraTriggerDetection playerAura)
    {
        Debug.Log("fuckin finally deflected omg");

        rb.transform.rotation = Quaternion.LookRotation(reflected);
        float mag = rb.velocity.magnitude;

        rb.velocity = reflected.normalized * mag;

        hasBeenDeflected = true;
    }
}
