using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveableCamera : MonoBehaviour
{
    // reference: https://www.reddit.com/r/Unity3D/comments/7i057l/how_to_check_if_mouse_position_is_within_a/
    // https://discussions.unity.com/t/detect-cursor-on-edge-of-screen/70650

    public Camera gameCamera;

    public Transform cameraTransform;

    public float moveSpeed;

    public Rigidbody rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray mouseOriginAndDirection = gameCamera.ScreenPointToRay(mousePosition);

        // transform movement
        CheckIfNearScreenEdge();

        // add force movement
        //AddForceCameraMove();
    }

    // move camera with transform
    void CheckIfNearScreenEdge()
    {
        // up
        if (Input.mousePosition.y >= Screen.height * 0.9f )
        {
            //cameraTransform.transform.Translate(Vector3.up.normalized * Time.deltaTime * moveSpeed, Space.World);
            cameraTransform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
        }
        // right
        if (Input.mousePosition.x >= Screen.width * 0.9f)
        {
            //cameraTransform.transform.Translate(Vector3.right.normalized * Time.deltaTime * moveSpeed, Space.World);
            cameraTransform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
        }
        // down
        if (Input.mousePosition.y <= Screen.height * 0.1f)
        {
            //cameraTransform.transform.Translate(Vector3.down.normalized * Time.deltaTime * moveSpeed, Space.World);
            cameraTransform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
        }
        // left
        if (Input.mousePosition.x <= Screen.width * 0.1f)
        {
            //cameraTransform.transform.Translate(Vector3.left.normalized * Time.deltaTime * moveSpeed, Space.World);
            cameraTransform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    // move camera with add force
    void AddForceCameraMove()
    {
        Vector3 moveDirection = Input.mousePosition;
        rb.AddForce( moveDirection.normalized * Time.deltaTime * moveSpeed, ForceMode.Force);
    }

    // if cursor raycast comes in contact with the edge sections of the screen, move the camera towards that direction
    // (change x and z, not y)
    // i think like.... maybe moving the cursor changes the position of the camera's look at point? prolly wouldnt work though..
    // wait no im stupid just have the reference transform in an empty game object that moves with the cursor. wait. but then how would...the fuckin..
    // how would the boundary thing work? wait. have it in an if statement? like. only have the transform follow the cursor when its out of bounds. or sumn.

    // to create the boundary, i could prolly put actual gameobjects the cursor can collide with and have them parented to the camera (and have
    // them be invisible ofc

    // actually wait jk. just see if the cursor is at a position _ distance away from the border and move the camera towards it if so.
}
