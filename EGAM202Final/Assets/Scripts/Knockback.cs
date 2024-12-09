using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Rigidbody rb;
    public float knockbackStrengthWall;

    public bool isTouchingGround;
    public bool isTouchingWall;

    public Coroutine kinematicSetter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        isTouchingGround = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnKnockback()
    {
        isTouchingGround = false;

        // starts timer to turn iskinematic on and off
        if (kinematicSetter == null)
        {
            kinematicSetter = StartCoroutine(KinematicSetter());
        }
        else
        {
            StopCoroutine(kinematicSetter);
            kinematicSetter = null;

            kinematicSetter = StartCoroutine(KinematicSetter());
        }

        // if touches wall, bounce off it
        if (isTouchingGround && rb.isKinematic == false)
        {
            Vector3 dir = transform.position.normalized * -1;

            rb.AddForce(dir * knockbackStrengthWall, ForceMode.Impulse);
        }

        // once iskinematic is true, stop the associated coroutine
        if (rb.isKinematic == true)
        {
            StopCoroutine(kinematicSetter);
            kinematicSetter = null;
        }
    }

    public IEnumerator KinematicSetter()
    {
        rb.isKinematic = false;

        // wait to check if grounded
        yield return new WaitForSeconds(0.25f);

        if (isTouchingGround)
        {
            SetKinematic();
        }
    }
    static bool CheckIfGrounded(Vector3 enemyPos, float rad, LayerMask groundLayer)
    {
        return (Physics.CheckSphere(enemyPos, rad, groundLayer));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            //Invoke("SetKinematic", 0.25f);
            isTouchingGround = true;
        }
        else
        {
            //CancelInvoke("SetKinematic");
            isTouchingGround=false;
        }

        //if (collision.gameObject.GetComponent<Wall>() != null)
        //{


        //    isTouchingWall = true;
        //}
        //else isTouchingWall=false;
    }

    void SetKinematic()
    {
        rb.isKinematic = true;
    }
}
