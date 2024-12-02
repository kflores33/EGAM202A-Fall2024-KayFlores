using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// handles colliders and stuff
public class AttackHandler : MonoBehaviour
{
    public float damage;

    public BoxCollider triggerBox;

    CombatHandler combatHandler;

    public UnityEvent OnKB, OnKBEnd;

    private Coroutine stopKnockback;

    // Start is called before the first frame update
    void Start()
    {
        combatHandler = GetComponent<CombatHandler>();
        DisableTriggerBox();
    }

    private void OnTriggerEnter(Collider other)
    {            
        // subtract from enemy health

        // if attack brings health to 0, destroy enemy

        var smackableObj = other.gameObject.GetComponent<Knockback>();
        if (smackableObj != null)
        {
            //if(stopKnockback != null)
            //{
            //    StopCoroutine(stopKnockback);
            //    stopKnockback = null;
            //}

            OnKB?.Invoke();
            smackableObj.rb.isKinematic = false;

            // calculate direction of enemy in relation to the player
            Vector3 dir = (smackableObj.transform.position - transform.position).normalized;

            // add knockback based on attack data
            smackableObj.rb.AddForce(dir * combatHandler.combo1[combatHandler.comboCounter].knockbackStrength, ForceMode.Impulse);

            //if (stopKnockback == null)
            //{
            //    stopKnockback = StartCoroutine(StopKnockback(smackableObj.rb));
            //}
        }
    }

    //IEnumerator StopKnockback(Rigidbody smackableObj)
    //{
    //    yield return new WaitForSeconds(0.75f);

    //    smackableObj.velocity = Vector3.zero;
    //    smackableObj.isKinematic = true;

    //    OnKBEnd?.Invoke();
    //}

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }
    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }
    // write separate functions for different attack strings, basically check if the previous attacks make a certain move possible to execute ig
}
