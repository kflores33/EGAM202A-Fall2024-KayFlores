using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public Coroutine jumpCDCoroutine;

    public float airMultiplier;
    public bool readyToJump;
    public bool canJump;

    float horizontalInput;
    float verticalInput;

    public Transform orientation;

    public Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    bool canRotate = true;
    bool canMove = true;

    Vector3 targetRotateDirection;

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
        // records player input
        MyInput();
        // controls speed
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    // for physics/force related things
    void FixedUpdate()
    {
        // player movement
        MovePlayer();

        if (canJump)
        {
            if (jumpCDCoroutine == null)
            {
                Debug.Log("start coroutine");

                Jump();
                canJump = false;

                jumpCDCoroutine = StartCoroutine(JumpCD());
            }
        }
    }

    // gets player movement input
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            canJump = true;
        }
    }

    private void CameraInfluence()
    {
        // relative to world space
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        moveDirection = camForward * verticalInput;
        moveDirection += camRight * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
    }

    private void RotateCharacter()
    {
        if (!canRotate) { return; }

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    private void MovePlayer()
    {
        if (!canMove) { return; }

        // gets move direction
        CameraInfluence();

        // calculate movement direction (makes movement follow camera direction)
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // actually move the player (while on ground)
        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        RotateCharacter();
    }

    // limits player speed
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            // calculate what max velocity should be
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            // apply calculated velocity
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    // actually does the physics part of the jump
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
        StopJumpCoroutine();
    }
    // stops coroutine
    void StopJumpCoroutine()
    {
        if (jumpCDCoroutine != null)
        {
            StopCoroutine(jumpCDCoroutine);
            jumpCDCoroutine = null;
        }
    }
    IEnumerator JumpCD()
    {
        Debug.Log("start cooldown");

        yield return new WaitForSeconds(jumpCooldown);

        ResetJump();

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            grounded = false;
        }
    }
}
