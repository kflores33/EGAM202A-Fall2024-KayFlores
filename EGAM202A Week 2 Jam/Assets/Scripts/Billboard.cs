using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // script references this unity discussion post: https://discussions.unity.com/t/how-i-can-create-an-sprite-that-always-look-at-the-camera/16891/8

    // only turns on renderer if the object is visible
    private void OnEnable()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            enabled = false;
        }
    }

    // have object face camera (called after update)
    private void LateUpdate()
    {
        transform.LookAt(transform.position, Vector3.up);
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }
}
