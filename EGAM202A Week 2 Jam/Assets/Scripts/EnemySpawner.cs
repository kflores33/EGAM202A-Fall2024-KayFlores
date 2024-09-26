using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject spawnPrefab;
    public GameObject timer;

    public Coroutine spawnEnemyCoroutine;

    public Transform spawnOrigin;
    public Vector3 spawnArea;

    public int spawnCount;
    public Vector3 boxSize;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemyCoroutine = StartCoroutine(SpawnCoroutine());
    }
    // Update is called once per frame
    void Update()
    {
        if (timer.GetComponent<GameTimer>().timeRemaining <= 0f)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnCoroutine()
    {
        Debug.Log("coroutine started");

        Vector3 originPoint = spawnOrigin.position;
        for (int i = 0; i < spawnCount; i++)
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

            if (spawnPrefab.GetComponent<EnemyAI>() != null)
            {
                spawnPrefab.GetComponent<EnemyAI>().player = playerTransform;
                Debug.Log("player transform assigned to intstantiated object");
            }

            GameObject newObject = Instantiate(spawnPrefab);
            newObject.transform.position = newPosition;

            Debug.Log("enemy spawned");

            yield return new WaitForSeconds(10f);
        }
    }
}
