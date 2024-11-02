using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
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
        transform.LookAt(player, Vector3.up);

        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
