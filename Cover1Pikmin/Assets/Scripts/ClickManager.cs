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
                TreasureScript treasure = hitInfo.transform.GetComponent<TreasureScript>();
                MoveCharacter pikmin = hitInfo.transform.GetComponent<MoveCharacter>();

                // if a pikmin is selected
                if (pikmin != null)
                {
                    // if another pikmin is selected while one is active, deselect current pikmin and select new
                    if (activePikmin != null)
                    {
                        activePikmin.currentState = MoveCharacter.PikminStates.Idle;

                        activePikmin = null;
                    }

                    // set this selected pikmin as the active one
                    pikmin.currentState = MoveCharacter.PikminStates.Selected;
                    activePikmin = pikmin;
                }
                // if there is a pikmin active and something other than the pikmin is selected, set target position
                else if (activePikmin != null)
                {
                    // (this designates the position where the mouse button was clicked as the target)
                    activePikmin.SetPikminTarget(hitInfo.point);
                }

                // select previously unselected treasure
                else if (treasure != null && activePikmin != null)
                {
                    activePikmin.SetPikminTargetTreasure(treasure.transform.position);
                }
            }
        }
    }
}
