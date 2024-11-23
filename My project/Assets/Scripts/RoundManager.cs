using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public int roundsComplete;

    public int enemiesLeft;
    public int ogSpawnAmount = 3;
    public int enemiesToSpawn;

    bool canSpawn;
    bool canIncreaseCount;

    public Transform spawnOrigin;
    public Vector3 spawnArea;

    //public int spawnCount;
    public Vector3 boxSize;

    public TMP_Text roundsCompletedText;

    public List<GameObject> enemyTypes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        roundsComplete = 0;

        enemiesToSpawn = ogSpawnAmount;

        roundsCompletedText.text = "rounds completed: " + roundsComplete.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        enemiesLeft = GameObject.FindObjectsOfType<EnemyHealth>().Length;

        if (enemiesLeft == 0 && !canSpawn) // if there are no new enemies, new ones can spawn
        {
            roundsComplete++; // add to # of completed rounds
            roundsCompletedText.text = "rounds completed: " + roundsComplete.ToString();

            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }

        if (canIncreaseCount)
        {
            enemiesToSpawn = enemiesToSpawn + 2; // add 2 to spawn count
            //spawnCount = enemiesToSpawn; // set the temp spawn value to match

            canIncreaseCount = false;
        }

        if (canSpawn == true)
        {
            canIncreaseCount = true; // allows spawn count to increase and spawn temp value to be set

            SpawnEnemy();

            canSpawn =false;
        }

    }

    void SpawnEnemy()
    {
        Vector3 originPoint = spawnOrigin.position;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Debug.Log("for loop started");
            Vector3 newPosition = Vector3.zero;

            bool isPositionOverlap = true;

            int attempts = 100;

            while (isPositionOverlap)
            {
                Debug.Log("while loop started");
                Vector3 randomOffset = Vector3.zero;
                randomOffset.x = Random.Range(-spawnArea.x, spawnArea.x);
                randomOffset.y = Random.Range(-spawnArea.y, spawnArea.y);
                randomOffset.z = Random.Range(-spawnArea.z, spawnArea.z);

                newPosition = originPoint + randomOffset;

                isPositionOverlap = Physics.BoxCast(newPosition, boxSize, Vector3.zero, Quaternion.identity, 0);

                attempts--;

                if (attempts <= 0)
                {
                    break;
                }
            }

            int N = enemyTypes.Count;
            int choice = Random.Range(0, (N + 1));

            GameObject randomEnemy = enemyTypes[choice];

            GameObject newObject = Instantiate(randomEnemy);
            newObject.transform.position = newPosition;

            Debug.Log("enemy spawned");
        }
    }
}
