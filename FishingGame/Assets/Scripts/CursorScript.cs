using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    PlayerStates player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetMousePositionActive()
    {
        // references this: https://discussions.unity.com/t/solved-moving-gameobject-along-x-and-z-axis-by-drag-and-drop-using-x-and-y-from-screenspace/674363

        float planeY = 0;
        Plane plane = new Plane(Vector3.up, Vector3.up * planeY);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }

    public void CheckCursorAngle()
    {
        // moves this object depending on cursor location
        GetMousePositionActive();

        // variables for ray
        Vector3 pos = transform.position;
        Vector3 dir = (player.transform.position - pos).normalized;

        // raycast
        Ray ray = new Ray(pos, dir * 20);
        Debug.DrawRay(pos, dir * 20, Color.red);
        // check if fish is hit
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            FishBehavior fish = hit.collider.GetComponent<FishBehavior>();

            if (fish != null)
            {
                //Debug.Log("fish hit!!!!!!!!");
                player.fishHit = true;
            }
            else { player.fishHit = false; }
        }
    }
}
