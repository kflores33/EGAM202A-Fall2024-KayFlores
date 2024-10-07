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
             case TreasureStates.TryingToCarry:
                UpdateTryingToCarry(); break;
        }
    }

    void FixedUpdate()
    {
        if (numberOfPikminCurrent == numberOfPikminRequired)
        {
            currentState = TreasureStates.BeingCarried;
        }

        Vector3 ourPosition = transform.position;
        Vector3 targetPosition = targetIndicator.position;

        // transform player towards destination and stop if close enough
        Vector3 delta = (ourPosition - targetPosition);

        if (delta.magnitude < 1f)
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        MoveCharacter pikmin = col.gameObject.GetComponent<MoveCharacter>();
        if (pikmin != null) 
        {
            // checks each position to see if it has a child (pikmin)
            // if yes, check next position in the list
            foreach (Transform t in PossiblePositions)
            {
                // if no, make the pikmin a child of that position and break the loop
                if (t.transform.childCount == 0)
                {
                    Debug.Log("set pikmin as child");

                    pikmin.currentState = MoveCharacter.PikminStates.TryingToCarry;
                    pikmin.transform.SetParent(t);

                    clickManager.GetComponent<ClickManager>().activePikmin = null;

                    currentState = TreasureStates.TryingToCarry;

                    ++numberOfPikminCurrent;
                    break;
                }
            }
        }
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

        foreach (Transform t in PossiblePositions)
        {
            MoveCharacter pikmin = t.gameObject.GetComponentInChildren<MoveCharacter>();
            if (t.transform.childCount > 0)
            {   
                pikmin.currentState = MoveCharacter.PikminStates.Idle;
                pikmin.transform.SetParent(null);
            }
            break;
        }
        numberOfPikminCurrent = 0;
    }
    public void SetPositionOfChild()
    {
        foreach (Transform t in PossiblePositions)
        {
            MoveCharacter pikmin = t.gameObject.GetComponentInChildren<MoveCharacter>();
            if (t.transform.childCount > 0)
            {
                // switch pikmin state
                if (pikmin.currentState != MoveCharacter.PikminStates.Carrying) 
                { 
                    pikmin.currentState = MoveCharacter.PikminStates.Carrying;
                }
                // transform pikmin with treasure object
                pikmin.transform.localPosition = new Vector3(0, 0, 0);
            }
            break;
        }
    }

    void UpdateIdle()
    {
        SetTreasureActive(false); 
        agent.enabled = false;
    }
    void UpdateBeingCarried()
    {
        SetTreasureActive(true);
        agent.enabled = true;

        // move children with treasure
        SetPositionOfChild();

        if (Input.GetMouseButtonDown(1))
        {
            DismissPikmin();
            clickManager.GetComponent<ClickManager>().activeTreasure = null;
            currentState = TreasureStates.Idle;
        }
    }
    void UpdateTryingToCarry()
    {
        SetTreasureActive(false);
        agent.enabled = false;

        if (Input.GetMouseButtonDown(1))
        {
            DismissPikmin();
            clickManager.GetComponent<ClickManager>().activeTreasure = null;
            currentState = TreasureStates.Idle;
        }
    }
}
