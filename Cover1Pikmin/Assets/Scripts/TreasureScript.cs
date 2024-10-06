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

    void FixedUpdate()
    {
        if (numberOfPikminCurrent == numberOfPikminRequired)
        {
            SetPositionOfChild();
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

                    pikmin.transform.SetParent(t);

                    // disable navmeshagent of attatched pikmin (keeps things from moving all wacky)
                    if (pikmin = clickManager.GetComponent<ClickManager>().activePikmin)
                    {
                        pikmin.GetComponent<NavMeshAgent>().enabled = false;

                        clickManager.GetComponent<ClickManager>().activePikmin.SetPikminActive(false);

                        clickManager.GetComponent<ClickManager>().activePikmin = null;
                    }

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
                pikmin.transform.SetParent(null);
                pikmin.GetComponent<NavMeshAgent>().enabled = true;
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
                pikmin.transform.localPosition = new Vector3(0, 0, 0);
            }
            break;
        }
    }
}
