using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public Transform thisTransform;
    public PlayerStates player;

    public float minTime;
    public float maxTime;

    public float speed;

    public float minDistance;
    public float maxDistance;

    public Vector3 targetPos;

    // Start is called before the first frame update
    void Awake()
    {
        // start the disappear coroutine
    }

    // Update is called once per frame
    void Update()
    {
        // stop disappear coroutine if player clicked reel in and start movement coroutine
    }

    // probably have some coroutine that repeats itself
}
