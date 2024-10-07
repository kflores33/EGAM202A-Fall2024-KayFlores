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
                if (pikmin != null && pikmin.currentState != MoveCharacter.PikminStates.TryingToCarry && activeTreasure == null)
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
                else if (activePikmin != null && treasure == null)
                {
                    // (this designates the position where the mouse button was clicked as the target)
                    activePikmin.SetPikminTarget(hitInfo.point);
                }

                else if (activePikmin != null && treasure != null)
                {
                    activePikmin.SetPikminTarget(treasure.gameObject.transform.position);
                }

                // select previously unselected treasure
                else if (treasure != null && activePikmin == null)
                {
                    // if another treasure is selected, deactivate current and activate new
                    if (activeTreasure != null)
                    {
                        activeTreasure.currentState = TreasureScript.TreasureStates.Idle;

                        activeTreasure = null;
                    }
                    // set this treasure as active treasure
                    if (treasure.numberOfPikminCurrent != treasure.numberOfPikminRequired)
                    {
                        treasure.currentState = TreasureScript.TreasureStates.TryingToCarry;

                    }
                    else if (treasure.numberOfPikminCurrent == treasure.numberOfPikminRequired)
                    {
                        treasure.currentState = TreasureScript.TreasureStates.BeingCarried;
                    }
                    activeTreasure = treasure;
                }

                // if treasure is active and some random shit is selected move the treasure
                else if (activeTreasure != null && activeTreasure.currentState == TreasureScript.TreasureStates.BeingCarried && pikmin == null)
                {
                    activeTreasure.SetTreasureTarget(hitInfo.point);
                }

                // deselect if not enough pikmin
                else if (activeTreasure != null && activeTreasure.currentState == TreasureScript.TreasureStates.TryingToCarry && pikmin == null)
                {
                    activeTreasure = null;
                }

                Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100, Color.magenta);
            }
        }
    }
}
