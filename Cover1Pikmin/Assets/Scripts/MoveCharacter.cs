using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MoveCharacter : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform activeIndicator;
    public Transform targetIndicator;

    public GameObject clickManager;

    public Rigidbody rb;

    public float moveSpeed;

    [Header("Line Renderer Variables")]
    public LineRenderer lineRenderer;
    public Color startColor;
    public Color endColor;

    public enum PikminStates
    {
        Idle,
        Selected,
        Carrying,
        TryingToCarry
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
            case PikminStates.Carrying:
                UpdateCarrying(); break;
            case PikminStates.TryingToCarry:
                UpdateTryingToCarry(); break;
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

    // Update is called once per frame
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

    void UpdateIdle()
    {
        SetPikminActive(false);
    }
    void UpdateSelected()
    {
        SetPikminActive(true);
        agent.enabled = true;

        if (Input.GetMouseButtonDown(1))
        {
            if (clickManager.GetComponent<ClickManager>().activePikmin != null)
            {
                clickManager.GetComponent<ClickManager>().activePikmin.currentState = MoveCharacter.PikminStates.Idle;

                clickManager.GetComponent<ClickManager>().activePikmin = null;
            }
        }
    }
    void UpdateCarrying()
    {
        SetPikminActive(false);
        clickManager.GetComponent<ClickManager>().activePikmin = null;
        agent.enabled = false;
    }
    void UpdateTryingToCarry()
    {
        SetPikminActive(false);
        clickManager.GetComponent<ClickManager>().activePikmin = null;
        agent.enabled = false;
    }
}
