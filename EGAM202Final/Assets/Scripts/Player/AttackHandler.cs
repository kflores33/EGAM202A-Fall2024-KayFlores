using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// referenced this video: https://youtu.be/gHaJUNiItmQ?si=P6a5F2lp6mYEyior
public class AttackHandler : MonoBehaviour
{
    public Animator _animator;
    public float _cd = 0.8f;
    private float _nextFireTime = 0f;
    public static int _noOfClicks = 0;
    float _lastClickedTime = 0;
    float _maxComboDelay = 1;

    [Header("Combo String Stuff")]
    public int _basicLightComboString = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _animator.GetCurrentAnimatorStateInfo(0).IsName("lightAttack1"))
        {
            _animator.SetBool("lightAttack1", false);
        }

        // reset combo if player takes too long
        if (Time.time - _lastClickedTime > _maxComboDelay)
        {
            _noOfClicks = 0;
        }
        if (Time.time > _nextFireTime)
        {
            // perform attack on click
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        _lastClickedTime = Time.deltaTime;
        _noOfClicks++;

        if (_noOfClicks == 1)
        {
            _animator.SetBool("lightAttack1", true);
        }
        _noOfClicks = Mathf.Clamp(_noOfClicks, 0, _basicLightComboString);

        if (_noOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _animator.GetCurrentAnimatorStateInfo(0).IsName("lightAttack1"))
        {
            //_animator.SetBool("lightAttack1", false);
            // next attack is true...etc
            SwitchAttackAnim("lightAttack1", "lightAttack2");
        }
    }

    void SwitchAttackAnim(string prevAnim, string nextAnim)
    {
            _animator.SetBool(prevAnim, false);
            _animator.SetBool(nextAnim, true);
    }

    // write separate functions for different attack strings, basically check if the previous attacks make a certain move possible to execute ig
}
