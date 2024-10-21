using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileEnemyAi : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=ieyHlYp5SLQ
    // https://www.youtube.com/watch?v=rs7xUi9BqjE
    // references this forum post : https://discussions.unity.com/t/navmesh-flee-ai-flee-from-player/126809

    [Header("References")]
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("State Variables")]
    public float stoppingDistanceOriginal;
    public float sightRange;
    public float tooCloseRange;

    bool inRange;
    bool tooClose;

    [Header("Misc Speed Variables")]
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float pathUpdateDelay = 0.2f;
    private float pathUpdateDeadline;

    // create state machine
    public enum EnemyStates
    {
        GetInShootingRange,
        Shoot,
        RunFromPlayer
    }
    public EnemyStates currentState;

    private void Awake()
    {
        // grab variable references
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Start()
    {
        agent.stoppingDistance = stoppingDistanceOriginal;
        currentState = EnemyStates.GetInShootingRange;
    }

    private void Update()
    {
        switch (currentState)
        { 
            case EnemyStates.GetInShootingRange:
                UpdateGetInShootingRange(); break;
            case EnemyStates.Shoot:
                UpdateShoot(); break;
            case EnemyStates.RunFromPlayer:
                UpdateRunFromPlayer(); break;
        }
    }

    void UpdateGetInShootingRange()
    {
        if (inRange)
        {
            Debug.Log("enemy is locked and loaded");
            currentState = EnemyStates.Shoot;
        }

        ChasePlayer();
    }
    void UpdateShoot()
    {
        LookAtPlayer();

        // if not in shooting range of the player, get in range
        if (!inRange)
        {
            Debug.Log("enemy is getting in range");
            currentState = EnemyStates.GetInShootingRange;
        }
        if (tooClose)
        {
            Debug.Log("too close to enemy---running away");

            // need to change the stopping distance to 0 temporarily until gets out of range of the player
            agent.stoppingDistance = 0;
            // if enemy gets too close, run from the player
            currentState = EnemyStates.RunFromPlayer;
        }
    }
    void UpdateRunFromPlayer()
    {
        RunFromPlayer();

        // iff far away enough, stop running from player
        if (!tooClose) 
        { 
            // if in range, shoot
            if (inRange)
            {
                Debug.Log("switching from running to shooting");
                StopRunAndReset();
                currentState = EnemyStates.Shoot;
            }
            // if not in range, get in range
            else
            {
                Debug.Log("switching from running to getting in range");
                StopRunAndReset();
                currentState = EnemyStates.GetInShootingRange;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            sightRange = stoppingDistanceOriginal;

            if(Vector3.Distance(transform.position, player.position) <= sightRange)
            {
                inRange = true;
            }
            else { inRange = false; }

            if(Vector3.Distance(transform.position, player.position) <= tooCloseRange)
            {
                tooClose = true;
            }
            else {  tooClose = false; } 
        }
    }

    // responsible for following player
    private void ChasePlayer()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + pathUpdateDelay;
            agent.SetDestination(player.position);
        }
    }

    private void RunFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < tooCloseRange)
        {
            Vector3 dirToPlayer = transform.position - player.position;

            Vector3 newPos = transform.position + dirToPlayer;

            agent.SetDestination(newPos);
        }
        else { tooClose = false;}
    }
    // reset variables back to normal
    private void StopRunAndReset()
    {
        agent.stoppingDistance = stoppingDistanceOriginal;
    }

    private void LookAtPlayer()
    {
        // defines lookPos
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        // converts lookPos (direction) into a rotation with quaternion
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        // interpolates between current rotation and the player's position
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
