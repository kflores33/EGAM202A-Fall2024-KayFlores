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

    public float waitToEnable = 0.2f;

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
            OnKB?.Invoke();
            smackableObj.rb.isKinematic = false;

            // calculate direction of enemy in relation to the player
            Vector3 dir = (smackableObj.transform.position - transform.position).normalized;

            // add knockback based on attack data
            smackableObj.rb.AddForce(dir * combatHandler.combo1[combatHandler.comboCounter].knockbackStrength, ForceMode.Impulse);
        }
    }

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }
    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }
}