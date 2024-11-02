using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    // referenced this video: https://www.youtube.com/watch?v=RXhTD8YZnY4

    [SerializeField] 
    private Rigidbody rb;

    [SerializeField]
    private float knockbackStrength, delay = 0.15f;

    private Coroutine stopKnockback;

    public UnityEvent OnBegin, OnDone;

    public void PlayFeedback(GameObject sender)
    {
        if (stopKnockback != null) 
        { 
            StopCoroutine(stopKnockback);
            stopKnockback = null;
        }

        OnBegin?.Invoke();

        //rb.isKinematic = false;

        // calculates the direction the enemy is in in relation to player
        Vector3 direction = (transform.position - sender.transform.position).normalized;

        // adds knockback
        rb.AddForce(direction * knockbackStrength, ForceMode.Impulse);

        if (stopKnockback == null)
        {
            stopKnockback = StartCoroutine(StopKnockback());
        }
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(delay);

        rb.velocity = Vector3.zero;
        //rb.isKinematic = true;

        OnDone?.Invoke();
    }
}
