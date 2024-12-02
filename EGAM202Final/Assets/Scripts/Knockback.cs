using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Rigidbody rb;
    public float knockbackStrengthWall;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            rb.isKinematic = true;
        }
        if (collision.gameObject.GetComponent<Wall>() != null)
        {
            Vector3 dir = transform.position.normalized * -1;

            rb.AddForce(dir * knockbackStrengthWall, ForceMode.Impulse);
        }
    }
}
