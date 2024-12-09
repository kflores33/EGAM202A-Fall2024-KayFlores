using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // references this video : https://www.youtube.com/watch?v=_J8RPIaO2Lc

    public PlayerMovement moveScript;
    public Animator animator;

    public Transform playerTransform;

    float blendZ = 0.0f;
    float blendX = 0.0f;

    public float acceleration = 0.5f;
    public float deceleration = 0.5f;

    public KeyCode _attackKey = KeyCode.Mouse0;
    public KeyCode _jumpKey = KeyCode.Space;

    public List<KeyCode> _moveKeysGeneric = new List<KeyCode>() { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    // increase performance
    int BlendZHash;
    int BlendXHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // increase performance
        BlendZHash = Animator.StringToHash("VelocityZ");
        BlendXHash = Animator.StringToHash("VelocityX");
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

    void ChangeVelocity1D(bool moveButtonPressed)
    {
        if (moveButtonPressed && blendZ < 1f) blendZ += Time.deltaTime * acceleration;

        if (!moveButtonPressed && blendZ > 0.0f) blendZ -= Time.deltaTime * deceleration;
    }

    public static bool GetAnyMoveKeyDown(List<KeyCode> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKey(keys[i]))
            {
                Debug.Log("key has been pressed");
                return true;
            }
        }
        return false;
    }

    void DefaultMovement()
    {
        bool moveKeyPressed = GetAnyMoveKeyDown(_moveKeysGeneric);

        ChangeVelocity1D(moveKeyPressed);

        animator.SetFloat(BlendZHash, blendZ);
    }

    void LockOnMovement()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed);

        animator.SetFloat(BlendXHash, blendX);
        animator.SetFloat(BlendZHash, blendZ);
    }

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();

        if (Input.GetKeyDown(_jumpKey) && moveScript.grounded == true)
        {
            animator.SetTrigger("jump");
        }
    }
}
