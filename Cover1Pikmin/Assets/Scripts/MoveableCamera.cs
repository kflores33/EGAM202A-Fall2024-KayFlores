using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // get mouse cursor position

    // if cursor raycast comes in contact with the edge sections of the screen, move the camera towards that direction
    // (change x and z, not y)
    // i think like.... maybe moving the cursor changes the position of the camera's look at point? prolly wouldnt work though..
    // wait no im stupid just have the reference transform in an empty game object that moves with the cursor. wait. but then how would...the fuckin..
    // how would the boundary thing work? wait. have it in an if statement? like. only have the transform follow the cursor when its out of bounds. or sumn.

    // to create the boundary, i could prolly put actual gameobjects the cursor can collide with and have them parented to the camera (and have
    // them be invisible ofc
}
