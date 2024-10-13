using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=ieyHlYp5SLQ
    // https://www.youtube.com/watch?v=rs7xUi9BqjE

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("State Variables")]
    public float sightRange;
    public bool playerInSightRange;

    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    public float pathUpdateDelay = 0.2f;

    private float pathUpdateDeadline;

    private void Awake()
    {
        // grab variable references
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange) {
            Patrolling();
        }
        else if (playerInSightRange) {
            transform.LookAt(player, Vector3.up);
            ChasePlayer();
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) 
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        
        // transform enemy towards destination
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // when destination reached, set walkPointSet back to false
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // raycast checks if walk point is in bounds
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + pathUpdateDelay;
            agent.SetDestination(player.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
