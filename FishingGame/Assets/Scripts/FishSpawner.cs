 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    // this script handles how to spawn fish (go figure)

    public float minTime;
    public float maxTime;

    public float minHealth = 20;
    public float maxHealth = 30;

    public float speed = 10;

    public PlayerStates playerStates;
    public GameObject fish;
    public GeneralGameManager GeneralGameManager;

    Coroutine spawnFishCoroutine;

    public List<FishData> fishTypes = new List<FishData>();

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
            //// give scene references
            fishScript.player = playerStates;
            fishScript.manager = GeneralGameManager;
            fishScript.waterOrigin = FindObjectOfType<Water>().transform;

            // give random scriptable object from list
            int N = fishTypes.Count; // length of list
            int choice = Random.Range(0, N);

            fishScript.data = fishTypes[choice];
        }

        Debug.Log("spawn fish");
        GameObject newFish = Instantiate(fish);
        newFish.transform.position = playerStates.lastClickLocation; // change this to be position of click

        // buffer
        yield return new WaitForSeconds(5);
    }
}
