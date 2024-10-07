using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ClickManager : MonoBehaviour
{
    public Camera gameCamera;

    public GameObject selectIndicator;

    public MoveCharacter activePikmin = null;
    public TreasureScript activeTreasure = null;

    public TreasureScript treasureTarget = null;

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
                    // you can pass shit into functions like this!!! cool!!!
                    PikminClicked(pikmin);
                }
                // if there is a pikmin active and something other than the pikmin is selected, set target position
                else if (activePikmin != null)
                {
                    ActivePikminClickedNonPikmin(activePikmin, hitInfo);

                    // this is the problem lol
                    if (treasure != null)
                    {
                        treasureTarget = treasure;
                        activeTreasure = treasure;

                        activePikmin.SetPikminTargetTreasure(treasureTarget.transform);

                        activePikmin.currentState = MoveCharacter.PikminStates.MoveToTarget;

                        activePikmin = null;
                    }
                    

                    // (this designates the position where the mouse button was clicked as the target)
                }


                // if treasure is clicked on and that treasure has enough pikmin carrying it, set it active
                else if (activeTreasure != null)
                {
                    if (activeTreasure.currentState == TreasureScript.TreasureStates.BeingCarried)
                    activeTreasure.SetTreasureTarget(hitInfo.point);
                }
            }
        }
    }

    void PikminClicked(MoveCharacter pikmin)
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
    void ActivePikminClickedNonPikmin(MoveCharacter pikmin, RaycastHit hitInfo)
    {
        // deselect
        activePikmin.SetPikminTarget(hitInfo.point);
    }
}
