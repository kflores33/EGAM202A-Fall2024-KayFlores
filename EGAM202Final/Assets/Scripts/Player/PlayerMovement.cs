using Cinemachine.Examples;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Quaternion lastRotation;

    [HideInInspector]public Vector3 camPos;

    PlayerMovement character;

    public float groundCheckRadius;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        character = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // records player input
        MyInput();
        // controls speed
        SpeedControl();

        if (CheckIfGrounded(transform.position, groundCheckRadius, whatIsGround))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

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

        camPos = camForward + camRight;
        camPos.Normalize();
        camPos.y = 0f;

        moveDirection = camForward * verticalInput;
        moveDirection += camRight * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
    }

    private void RotateCharacterLockOn()
    {
        if (!canRotate) { return; }
        else

        targetRotateDirection = moveDirection;
        targetRotateDirection.Normalize();

        if (targetRotateDirection.x != -1)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetRotateDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            // if there is any vertical/horizontal input, log the rotation of the character
            if (verticalInput != 0 || horizontalInput != 0)
            {
                //lastRotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                lastRotation = targetRotation;

                // only change rotation when character moves forward
                if (verticalInput == 1)
                {
                    transform.rotation = targetRotation;
                }
                // change rotation when character moves left and right

                // need to make it so that the camera dictates the forward direction of the character to a degree
            }
            // no input, then rotate the player towards the last rotation
            else
            {
                // have player face the direction of last input
                transform.rotation = lastRotation;
            }
        }
    }

    private void RotateCharacter()
    {
        if (!canRotate) { return; }
        else

        targetRotateDirection = moveDirection;
        targetRotateDirection.Normalize();

        if (targetRotateDirection.x != -1)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetRotateDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            // if there is any vertical/horizontal input, log the rotation of the character
            if (verticalInput != 0 || horizontalInput != 0)
            {
                lastRotation = targetRotation;
                transform.rotation = targetRotation;

                TiltWhileRun();
            }
            // no input, then rotate the player towards the last rotation
            else
            {
                // have player face the direction of last input
                transform.rotation = lastRotation;
            }
        }
    }

    private void TiltWhileRun()
    {

    }

    private void MovePlayer()
    {
        if (!canMove) { return; }
        else

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

    public void DisableMovement()
    {
        canMove = false;
        canRotate = false;
    }
    public void EnableMovement()
    {
        canMove = true;
        canRotate = true;
    }

    static bool CheckIfGrounded(Vector3 playerPos, float rad, LayerMask groundLayer)
    {
        return (Physics.CheckSphere(playerPos, rad, groundLayer));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, groundCheckRadius);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Ground>() != null)
    //    {
    //        grounded = true;
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Ground>() != null)
    //    {
    //        grounded = false;
    //    }
    //}
}
