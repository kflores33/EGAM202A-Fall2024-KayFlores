using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // references this video : https://www.youtube.com/watch?v=_J8RPIaO2Lc

    public Animator animator;
    float blendZ = 0.0f;
    float blendX = 0.0f;
    public float acceleration = 0.5f;
    public float deceleration = 0.5f;

    public bool hasJumped;

    // increase performance
    int BlendZHash;
    int BlendXHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("canJump", true);

        // increase performance
        BlendZHash = Animator.StringToHash("DirectionZ");
        BlendXHash = Animator.StringToHash("DirectionX");
    }

    // handles acceleration and deceleration
    void changeVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed)
    {
        // increase blend
        if (forwardPressed && blendZ < 1f)
            blendZ += Time.deltaTime * acceleration;
        if (backPressed && blendZ > -1f)
            blendZ -= Time.deltaTime * acceleration;
        if (leftPressed && blendX > -1f)
            blendX -= Time.deltaTime * acceleration;
        if (rightPressed && blendX < 1f)
            blendX += Time.deltaTime * acceleration;

        // decrease velocity/blend
        if (!forwardPressed && blendZ > 0.0f)
            blendZ -= Time.deltaTime * deceleration;
        if (!backPressed && blendZ < 0.0f)
            blendZ += Time.deltaTime * deceleration;

        if (!forwardPressed && !backPressed && blendZ != 0.0f && (blendZ > -0.05f && blendZ < 0.05f))
            blendZ = 0.0f;

        if (!leftPressed && blendX < 0.0f)
            blendX += Time.deltaTime * deceleration;
        if (!rightPressed && blendX > 0.0f)
            blendX -= Time.deltaTime * deceleration;

        if (!leftPressed && !rightPressed && blendX != 0.0f && (blendX > -0.05f && blendX < 0.05f))
            blendX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerMovement>().canJump == true)
        {
            animator.SetBool("canJump", true);
        }

        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool spacePressed = Input.GetKey(KeyCode.Space);

        // play jump animation
        if (spacePressed && animator.GetBool("canJump") == true) { 
            animator.SetTrigger("JumpTrigger");

            animator.SetBool("canJump", false);
            
            GetComponent<PlayerMovement>().canJump = false;
        }
        else changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed);

        animator.SetFloat(BlendXHash, blendX);
        animator.SetFloat(BlendZHash, blendZ);
    }
}
