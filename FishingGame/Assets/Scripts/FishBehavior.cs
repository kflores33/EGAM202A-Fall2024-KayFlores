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

    Coroutine selfDestruct;
    Coroutine moveBehavior;

    // Start is called before the first frame update
    void Awake()
    {
        // start the disappear coroutine
        selfDestruct = StartCoroutine(SelfDestructTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // stop disappear coroutine if player clicked reel in and start movement coroutine
        if (player.currentState == PlayerStates.PlayerStateMachine.FishingActive)
        {
            StopCoroutine(selfDestruct);

            // probably have some coroutine that repeats itself
            if (moveBehavior == null) 
            {
                moveBehavior = StartCoroutine(MoveBehavior());
            }
        }
    }        
    IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }
    IEnumerator MoveBehavior()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
    }
}
