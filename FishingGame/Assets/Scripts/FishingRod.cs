using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    // this script is in charge of handling visual indicators associated with the fishing rod

    LineRenderer lineRenderer;
    PlayerStates playerStates;
    public Transform lineOriginReference;

    public Vector3 lineOrigin;
    public Vector3 lineEnd;

    public int pointCount = 10;

    // have length determine curve strength (inverse relationship)
    public float minCurveStrength;
    public float maxCurveStrength;

    public float minDistance;
    public float maxDistance;

    public AnimationCurve lineCurve;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerStates = GetComponentInParent<PlayerStates>();

        lineRenderer.positionCount = pointCount +1 ;
    }

    // Update is called once per frame
    void Update()
    {
        // if in a fishing state, enable the line renderer
        if (playerStates.currentState != PlayerStates.PlayerStateMachine.NotFishing)
        {
            lineRenderer.enabled = true;

            // allow for rod to move based on mouse position
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
        lineEnd = endPoint ;
        lineRenderer.SetPosition(10, lineEnd);
        LineCurve(lineOrigin, lineEnd);
    }

    void TiltFishingRod()
    {
        // stuff that dictated the rotation(?) of the fishing rod depending on mouse position
    }

    void LineCurve(Vector3 start, Vector3 end)
    {
        Vector3 delta = end - start;
        float distance = delta.magnitude;   

        // the smaller the distance, the stronger the curve
        float distanceInterp = (distance - minDistance) / maxDistance;
        float curveStrength = Mathf.Lerp(minCurveStrength, maxCurveStrength, distanceInterp);

        for (int i = 0; i <= pointCount; i++)
        {
            // create interp float variable
            float interp = i / ((float)pointCount);

            Vector3 position = Vector3.Lerp(start, end, interp);

            float curveValue = lineCurve.Evaluate(interp);
            curveValue *= curveStrength;

            position.y -= curveValue;

            lineRenderer.SetPosition(i, position);
        }
    }
}
