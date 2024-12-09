using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatHandler : MonoBehaviour
{
    public List<AttackData> combo1; // get rid of this once you implement the uhhh expanded combo system

    public List<ComboList> comboList;

    float lastClickedTime;
    float lastComboEnd;
    public int comboCounter;

    bool canEndCombo;

    public float endComboTime;

    [SerializeField] KeyCode lightAttack = KeyCode.Mouse0;

    Animator anim;
    int attackLayer;

    PlayerMovement playerMovement;

    [SerializeField] AttackHandler attackHandler;

    public UnityEvent OnAttack, OnAttackEnd;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        attackLayer = anim.GetLayerIndex("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(lightAttack))
        {
            //if ((anim.GetCurrentAnimatorStateInfo(attackLayer).IsTag("Attack")))
            //{
            //    Debug.Log("reset collider");
            //    OnAttackEnd?.Invoke();
            //}
            Attack();
            OnAttack?.Invoke();
        }

        //ExitAttack();
    }

    void Attack()
    {
        //Debug.Log("attack clicked");
        if (Time.time - lastComboEnd > 0.1f && comboCounter < combo1.Count)
        {
            //Debug.Log("ATTACK EXECUTED");
            CancelInvoke("EndCombo");
            //OnAttack?.Invoke();
            

            if (Time.time - lastClickedTime >= 0.2f)
            {
                Debug.Log("ANIMATION EXECUTED");
                // takes attack from combo list (corresponding to current attack)
                anim.runtimeAnimatorController = combo1[comboCounter].animatorOV;
                anim.Play("Attack", attackLayer, 0);
                //attackHandler.damage = combo1[comboCounter].damage;

                comboCounter++;
                lastClickedTime = Time.time;

                rb.AddForce(playerMovement.targetRotateDirection * 10, ForceMode.Impulse);

                if(comboCounter >= combo1.Count)
                {
                    comboCounter = 0;
                }
            }
        }

        canEndCombo = true;
    }

    void ExitAttack()
    {
        //if(anim.GetCurrentAnimatorStateInfo(attackLayer).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(attackLayer).IsTag("Attack"))
        //{
            OnAttackEnd?.Invoke();

            if (canEndCombo)
            {
                Invoke("EndCombo", endComboTime);
                canEndCombo = false;

                Debug.Log("end combo invoked");
            }
        //}
    }

    void EndCombo()
    {
        //OnAttackEnd?.Invoke();

        Debug.Log("combo ended");

        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
