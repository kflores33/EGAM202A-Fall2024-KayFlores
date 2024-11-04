 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    // this script handles how to spawn fish (go figure)

    public float minTime;
    public float maxTime;

    public PlayerStates playerStates;
    public GameObject fish;

    Coroutine spawnFishCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStates.currentState == PlayerStates.PlayerStateMachine.FishingIdle) 
        { 
            if(spawnFishCoroutine == null)
            {
                spawnFishCoroutine = StartCoroutine(SpawnFishCoroutine());
            }
        }
        else
        {
            if (spawnFishCoroutine != null) 
            {            
                StopCoroutine(spawnFishCoroutine);
                spawnFishCoroutine = null; 
            }

        }
    }

    IEnumerator SpawnFishCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(minTime,maxTime));

        FishBehavior fishScript = fish.GetComponent<FishBehavior>();
        if (fishScript != null) 
        {
            // give player reference
            fishScript.player = playerStates;
            // give random time values
            fishScript.minTime = Random.Range(1, 4);
            fishScript.maxTime = Random.Range(5, 10);
            // give random distance values
            fishScript.minDistance = Random.Range(1, 3);
            fishScript.maxDistance = Random.Range(4, 7);
        }
        Debug.Log("spawn fish");
        GameObject newFish = Instantiate(fish);
        newFish.transform.position = playerStates.lastClickLocation; // change this to be position of click

        // buffer
        yield return new WaitForSeconds(5);
    }
}
