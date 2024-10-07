using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreasureScript : MonoBehaviour
{
    // Create a list to house position transforms
    public List<Transform> PossiblePositions = new List<Transform>();
    public Collider Collider;

    public NavMeshAgent agent;

    public Transform activeIndicator;
    public Transform targetIndicator;

    public GameObject clickManager;

    public float moveSpeed;

    public int numberOfPikminRequired;
    public int numberOfPikminCurrent;

    public enum TreasureStates
    {
        Idle,
        BeingCarried,
        TryingToCarry
    }

    public TreasureStates currentState;

    private void Start()
    {
        currentState = TreasureStates.Idle;
    }

    private void Update()
    {
        switch (currentState) {
            case TreasureStates.Idle:
                UpdateIdle();break;
            case TreasureStates.BeingCarried:
                UpdateBeingCarried(); break;
        }
    }

    void FixedUpdate()
    {
        Vector3 ourPosition = transform.position;
        Vector3 targetPosition = targetIndicator.position;

        // transform player towards destination and stop if close enough
        Vector3 delta = (ourPosition - targetPosition);

        if (delta.magnitude < 1f)
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        MoveCharacter pikmin = col.gameObject.GetComponent<MoveCharacter>();

    }

    public void SetTreasureActive(bool isActive)
    {
        activeIndicator.gameObject.SetActive(true);

        if (isActive) { activeIndicator.GetComponent<Outline>().enabled = true; }
        else { activeIndicator.GetComponent<Outline>().enabled = false;}
    }
    public void SetTreasureTarget(Vector3 position) 
    {
        // set target indicator active 
        targetIndicator.gameObject.SetActive(true);
        targetIndicator.position = position;

        // sets the target position to the targetIndicator's transform
        agent.SetDestination(position);
    }

    // doesnt work
    public void DismissPikmin()
    {
        Debug.Log("pikmin dismissed");

        numberOfPikminCurrent = 0;

        currentState = TreasureStates.Idle;
    }
    public void SetPositionOfChild()
    {

    }

    void UpdateIdle()
    {
        SetTreasureActive(false); 

        if (numberOfPikminCurrent >= numberOfPikminRequired)
        {
            currentState = TreasureStates.BeingCarried;
        }
    }
    void UpdateBeingCarried()
    {
        SetTreasureActive(true);

        // move children with treasure
        SetPositionOfChild();

        if (Input.GetMouseButtonDown(1))
        {
            DismissPikmin();
            clickManager.GetComponent<ClickManager>().activeTreasure = null;
        }
    }
}
