using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // code references this video: https://www.youtube.com/watch?v=f473C43s8nE

    [Header("Movement")]
    public float moveSpeed;
    public float rotationSpeed;

    public float acceleration = 0.1f;
    public float deceleration = 0.5f;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool canJump;

    float horizontalInput;
    float verticalInput;
    Quaternion cameraRot;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    public Vector3 moveDirection;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check (raycast from center of player down)
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // records player input
        MyInput();
        // controls speed
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        // player movement
        MovePlayer();
    }

    // gets player movement input
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // * need to get the input to be relative to the direction the camera is facing

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) 
        {
            readyToJump = false;

            // allows for continuous jumping if jump key is held down
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        
        // calculate movement direction (makes movement follow camera direction)
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // * Vector3.forward converts quaternion (rotation) into a direction

        // actually move the player (while on ground)
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else if(!grounded) 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    // limits player speed
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f,  rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            // calculate what max velocity should be
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            // apply calculated velocity
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity so jump height stays constant
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // (impulse only adds force once)
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        canJump = true;
    }
}
