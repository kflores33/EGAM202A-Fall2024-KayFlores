using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public Camera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // mouse into world position
        Vector2 mousePosition = Input.mousePosition;
        Ray mouseOriginAndDirection = gameCamera.ScreenPointToRay(mousePosition);

        // get list of all hits
        RaycastHit[] raycastHits = Physics.RaycastAll(mouseOriginAndDirection, 100);
        foreach(RaycastHit hitInfo in raycastHits)
        {
            // if object hit has clickable object script...
            ClickableObject clickableObject = hitInfo.transform.GetComponent<ClickableObject>();
            if (clickableObject != null) { 
            
            }
        }

        Debug.DrawRay(mouseOriginAndDirection.origin, mouseOriginAndDirection.direction * 100, Color.magenta);
    }

    // update this script to have characters be selectable
}
