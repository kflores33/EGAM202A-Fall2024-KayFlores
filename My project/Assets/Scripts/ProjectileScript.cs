using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float life = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, life);
    }

}
