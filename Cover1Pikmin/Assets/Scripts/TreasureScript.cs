using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreasureScript : MonoBehaviour
{
    // Create a list to house position transforms
    public List<MoveCharacter> PikminList = new List<MoveCharacter>();
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
                case TreasureStates.TryingToCarry:
                UpdateTryingToCarry(); break;
        }
    }

    void FixedUpdate()
    {
        Vector3 ourPosition = transform.position;
        Vector3 targetPosition = targetIndicator.position;

        // transform player towards destination and stop if close enough
        Vector3 delta = ((ourPosition - targetPosition) * moveSpeed);

        if (delta.magnitude < 1f)
        {
            targetIndicator.gameObject.SetActive(false);
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

    public void DismissPikmin()
    {
        Debug.Log("pikmin dismissed");

        numberOfPikminCurrent = 0;

        foreach (MoveCharacter pikmin in PikminList) 
        { 

            RemoveFromPikminList(pikmin);

            break;
        }

        currentState = TreasureStates.Idle;
    }
    public void AddToPikminList(MoveCharacter pikmin)
    {
        if (PikminList.Contains(pikmin) == false) 
        { 
            PikminList.Add(pikmin);
            CheckAmountInPikminList();
        }
    }

    public void RemoveFromPikminList(MoveCharacter pikmin)
    {
        PikminList.Remove(pikmin);
        CheckAmountInPikminList();
    }

    void CheckAmountInPikminList()
    {
        if (PikminList.Count >= numberOfPikminRequired)
        {
            currentState = TreasureStates.BeingCarried;
        }
        else
        {
            currentState = TreasureStates.Idle;
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

        if (Input.GetMouseButtonDown(1))
        {
            DismissPikmin();
            clickManager.GetComponent<ClickManager>().activeTreasure = null;
        }
    }

    void UpdateTryingToCarry()
    {
        SetTreasureActive(true);

        agent.enabled = false;

        if (Input.GetMouseButtonDown(1))
        {
            DismissPikmin();
            clickManager.GetComponent<ClickManager>().activeTreasure = null;
        }
    }
}
