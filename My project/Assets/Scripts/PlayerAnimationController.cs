using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // references this video : https://www.youtube.com/watch?v=_J8RPIaO2Lc

    public PlayerMovement moveScript;
    public Animator animator;

    public Transform playerTransform;

    float blendZ = 0.0f;
    float blendX = 0.0f;
    float posY = 0.0f;

    public float acceleration = 0.5f;
    public float deceleration = 0.5f;

    public float jumpAcceleration = 0.5f;
    public float jumpDeceleration = 0.5f;

    // increase performance
    int BlendZHash;
    int BlendXHash;
    int BlendYHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // increase performance
        BlendZHash = Animator.StringToHash("VelocityZ");
        BlendXHash = Animator.StringToHash("VelocityX");
        BlendYHash = Animator.StringToHash("PosY");
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

    //void changeVelocityY(bool jumpPressed, bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed)
    //{
    //    if (jumpPressed && posY < 1f)
    //    {
    //        posY = 1f;

    //        if (!forwardPressed && blendZ > 0.0f)
    //            blendZ -= Time.deltaTime * deceleration;
    //        if (!backPressed && blendZ < 0.0f)
    //            blendZ += Time.deltaTime * deceleration;

    //        if (!forwardPressed && !backPressed && blendZ != 0.0f && (blendZ > -0.05f && blendZ < 0.05f))
    //            blendZ = 0.0f;

    //        if (!leftPressed && blendX < 0.0f)
    //            blendX += Time.deltaTime * deceleration;
    //        if (!rightPressed && blendX > 0.0f)
    //            blendX -= Time.deltaTime * deceleration;

    //        if (!leftPressed && !rightPressed && blendX != 0.0f && (blendX > -0.05f && blendX < 0.05f))
    //            blendX = 0.0f;
    //    }
    //    if (!jumpPressed && posY > 0.0f)
    //    {
    //        posY -= Time.deltaTime * jumpDeceleration;

    //        if (forwardPressed && blendZ < 1f)
    //            blendZ += Time.deltaTime * acceleration;
    //        if (backPressed && blendZ > -1f)
    //            blendZ -= Time.deltaTime * acceleration;
    //        if (leftPressed && blendX > -1f)
    //            blendX -= Time.deltaTime * acceleration;
    //        if (rightPressed && blendX < 1f)
    //            blendX += Time.deltaTime * acceleration;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool spacePressed = Input.GetKey(KeyCode.Space);

        bool jumpPressed = Input.GetKey(KeyCode.Space);

        changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed);

        //changeVelocityY(jumpPressed, forwardPressed, backPressed, leftPressed, rightPressed);

        animator.SetFloat(BlendXHash, blendX);
        animator.SetFloat(BlendZHash, blendZ);

        // jump stuff
        //JumpAnimationCheck();
        animator.SetFloat(BlendYHash, posY);
    }
}
