using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles colliders and stuff
public class AttackHandler : MonoBehaviour
{
    public float damage;

    BoxCollider triggerBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //var enemy = other.gameObject.GetComponent<>();
        //if(enemy != null)
        //{
        //    // subtract from enemy health

        //    // if attack brings health to 0, destroy enemy
        //}
    }

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
