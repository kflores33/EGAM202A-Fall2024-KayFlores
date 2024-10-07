using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MoveCharacter : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Visual Indicator Variables")]
    public Transform activeIndicator;
    public Transform targetIndicator;

    public GameObject carryIndicator;
    public GameObject tryingToCarryIndicator;

    public GameObject clickManager;

    public float moveSpeed;

    [Header("Target Positions")]
    public Transform treasurePosition;

    public enum PikminStates
    {
        Idle,
        Selected,
        MoveToTreasure,
        MoveToTarget
    }

    public PikminStates currentState;
    void Start()
    {
        currentState = PikminStates.Idle;
    }
    void Update()
    {
        switch (currentState) 
        { 
            case PikminStates.Idle:
                UpdateIdle() ; break;
            case PikminStates.Selected:
                UpdateSelected() ; break;
            case PikminStates.MoveToTreasure:
                UpdateMoveToTreasure(); break;
            case PikminStates.MoveToTarget:
                UpdateMoveToTarget(); break;
        }
    }

    public void SetPikminActive(bool isActive)
    {
        activeIndicator.gameObject.SetActive(true);

        if (isActive) { activeIndicator.GetComponent<Outline>().enabled = true; }
        else { activeIndicator.GetComponent<Outline>().enabled = false; }
    }

    public void SetPikminTarget(Vector3 position)
    {
        // set target indicator active 
        targetIndicator.gameObject.SetActive(true);
        targetIndicator.position = position;

        // sets the target position to the targetIndicator's transform
        agent.SetDestination(position);
    }

    public void SetPikminTargetTreasure(Transform transformTreasure)
    {
        treasurePosition = transformTreasure;

        // sets the target position to the targetIndicator's transform
        agent.SetDestination(transformTreasure.position);
    }
    public void ForgetTreasureTarget()
    {
        currentState = PikminStates.Idle;

        if (treasurePosition != null) 
        { 
            treasurePosition.GetComponent<TreasureScript>().RemoveFromPikminList(this);
        }

        treasurePosition = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 ourPosition = transform.position;
        Vector3 targetPosition = targetIndicator.position;

        // transform player towards destination and stop if close enough
        Vector3 delta = ((ourPosition - targetPosition)*moveSpeed);

        if (delta.magnitude < 1f)
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }

    void UpdateIdle()
    {
        // visual indicators
        SetPikminActive(false);
        tryingToCarryIndicator.SetActive(false);
        carryIndicator.SetActive(false);
    }
    void UpdateSelected()
    {
        // visual indicators
        SetPikminActive(true);
        tryingToCarryIndicator.SetActive(false);
        carryIndicator.SetActive(false);

        // deselect
        if (Input.GetMouseButtonDown(1))
        {
            if (clickManager.GetComponent<ClickManager>().activePikmin != null)
            {
                clickManager.GetComponent<ClickManager>().activePikmin.currentState = MoveCharacter.PikminStates.Idle;

                clickManager.GetComponent<ClickManager>().activePikmin = null;
            }

            ForgetTreasureTarget();
        }
    }

    // should be called follow treasure (more accurate tbh)
    void UpdateMoveToTreasure()
    {
        // visual indicators
        tryingToCarryIndicator.SetActive(false);
        SetPikminActive(true);
        carryIndicator.SetActive(true);

        treasurePosition = clickManager.GetComponent<ClickManager>().treasureTarget.transform;

        // follow position of target treasure if there is one
        if (treasurePosition != null) {
            SetPikminTargetTreasure(treasurePosition);
        }
    }
    // regular point and click movement
    void UpdateMoveToTarget()
    {
        // visual indicators
        carryIndicator.SetActive(false);
        tryingToCarryIndicator.SetActive(true);
        SetPikminActive(true);

        // follow position of target treasure if there is one
        if (treasurePosition != null)
        {
            SetPikminTargetTreasure(treasurePosition);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        TreasureScript treasure = col.gameObject.GetComponent<TreasureScript>();
        Debug.Log("collided at all");
        // switches the state on collision
        switch (currentState)
        {
            case PikminStates.MoveToTarget:        
                 Debug.Log("collided with treasure");
            // if collided with target treasure, move towards it
            if (treasure == treasurePosition) 
            {
 
                currentState = PikminStates.MoveToTreasure;
                treasurePosition.GetComponent<TreasureScript>().AddToPikminList(this);
            }
            break;
        }
    }
}
