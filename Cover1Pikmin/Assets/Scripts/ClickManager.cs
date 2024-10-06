using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ClickManager : MonoBehaviour
{
    public Camera gameCamera;

    public GameObject selectIndicator;

    public MoveCharacter activePikmin = null;
    public TreasureScript activeTreasure = null;

    // regular update is ok for this cause movement uses transform instead of add force/physics
    void Update()
    {
        // select pikmin with mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // mouse into world position
            Vector2 mousePosition = Input.mousePosition;
            Ray mouseRay = gameCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 100))
            {
                // check if obj has the pikmin script
                MoveCharacter pikmin = hitInfo.transform.GetComponent<MoveCharacter>();
                if (pikmin != null)
                {
                    // if another pikmin is selected while one is active, deselect current pikmin and select new
                    if (activePikmin != null )
                    {
                        activePikmin.SetPikminActive(false);

                        activePikmin = null;
                    }
                    // deselect treasure if previously deselected
                    if(activeTreasure != null)
                    {
                        activeTreasure.SetTreasureActive(false);

                        activeTreasure = null;
                    }

                    pikmin.SetPikminActive(true);
                    activePikmin = pikmin;
                }
                // if pikmin is already selected, set target position
                else if (activePikmin != null)
                {
                    // (this designates the position where the mouse button was clicked as the target)
                    activePikmin.SetPikminTarget(hitInfo.point);
                }

                TreasureScript treasure = hitInfo.transform.GetComponent<TreasureScript>();
                if (treasure != null)
                {
                    // if another treasure is selected, deactivate current and activate new
                    if (activeTreasure != null)
                    {
                        activeTreasure.SetTreasureActive(false);

                        activeTreasure = null;
                    }
                    // deselect pikmin if previously selected
                    //if (activePikmin != null)
                    //{
                    //    activePikmin.SetPikminActive(false);

                    //    activePikmin = null;
                    //}

                    treasure.SetTreasureActive(true);
                    activeTreasure = treasure;
                }
                else if (activeTreasure != null && activeTreasure.GetComponent<TreasureScript>().numberOfPikminRequired == activeTreasure.GetComponent<TreasureScript>().numberOfPikminCurrent)
                {
                    activeTreasure.SetTreasureTarget(hitInfo.point);
                }

                Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100, Color.magenta);
            }
            // deselect pikmin/treasure with right click
            if (Input.GetMouseButtonDown(1))
            {
                if (activePikmin != null)
                {
                    activePikmin.SetPikminActive(false);

                    activePikmin = null;
                }

                if (activeTreasure != null && activeTreasure.GetComponent<TreasureScript>().numberOfPikminRequired == activeTreasure.GetComponent<TreasureScript>().numberOfPikminCurrent)
                {
                    activeTreasure.DismissPikmin();
                    activeTreasure.SetTreasureActive(false);

                    activeTreasure = null;
                }
            }
        }
    }
}
