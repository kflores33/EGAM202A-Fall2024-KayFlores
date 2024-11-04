using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    LineRenderer lineRenderer;
    PlayerStates playerStates;
    public Transform lineOriginReference;

    public Vector3 lineOrigin;
    public Vector3 lineEnd;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerStates = GetComponentInParent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        // if in a fishing state, enable the line renderer
        if (playerStates.currentState != PlayerStates.PlayerStateMachine.NotFishing)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        // while the line renderer is enabled, set the origin point and end point (if the reference position has changed)
        if (lineRenderer.enabled) 
        {
            // if line origin and lineTarget are not at the same position, update position
            if (lineOrigin != lineOriginReference.position) 
            {
                SetLineOrigin();
            }
            // if in fishing idle, set end point to click location
            if (playerStates.currentState == PlayerStates.PlayerStateMachine.FishingIdle) 
            { 
                Vector3 endPoint = playerStates.lastClickLocation;
                SetLineEndPoint(endPoint);
            }
            // if in active fishing state...
            else if (playerStates.currentState == PlayerStates.PlayerStateMachine.FishingActive)
            {
                // get fish in scene and set as the end point
                if (playerStates.currentFish != null) 
                {
                    Vector3 endPoint = playerStates.currentFish.transform.position;
                    SetLineEndPoint(endPoint);
                }
            }
        }
    }

    void SetLineOrigin()
    {
        lineOrigin = lineOriginReference.position;
        lineRenderer.SetPosition(0, lineOrigin);
    }
    void SetLineEndPoint(Vector3 endPoint) 
    { 
        lineEnd = endPoint;
        lineRenderer.SetPosition(1, lineEnd);
    }
}
