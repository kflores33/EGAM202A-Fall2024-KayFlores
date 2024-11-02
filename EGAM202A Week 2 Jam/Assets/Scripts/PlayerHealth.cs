using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Collider playerCollider;
    Vector3 playerPosition;
    public Rigidbody rb;
    public int health;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        //playerCollider = GetComponentInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        gameOver = false;
        
        health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = GetComponent<PlayerMovement>().moveDirection;

        if (health <= 0 )
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            health -= 1;
            Debug.Log("player hit");
        }
    }

    void Die() {
        rb.AddExplosionForce(1000f, playerPosition, 0f, 5f, ForceMode.Force);

        //Debug.Log("game over!!");
        gameOver = true;
    }
}
