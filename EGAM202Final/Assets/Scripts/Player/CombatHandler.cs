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

    [SerializeField] KeyCode lightAttack = KeyCode.Mouse0;

    Animator anim;
    int attackLayer;

    [SerializeField] AttackHandler attackHandler;

    public UnityEvent OnAttack, OnAttackEnd;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        attackLayer = anim.GetLayerIndex("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(lightAttack))
        {
            Attack();
        }

        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > 0.5f && comboCounter < combo1.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= 0.4f)
            {
                // takes attack from combo list (corresponding to current attack)
                anim.runtimeAnimatorController = combo1[comboCounter].animatorOV;
                anim.Play("Attack", attackLayer, 0);
                //attackHandler.damage = combo1[comboCounter].damage;

                comboCounter++;
                lastClickedTime = Time.time;

                if(comboCounter >= combo1.Count)
                {
                    comboCounter = 0;
                }
            }
        }

        OnAttack?.Invoke();
    }

    void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(attackLayer).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(attackLayer).IsTag("Attack"))
        {
            OnAttackEnd?.Invoke();

            Invoke("EndCombo", 1);

            Debug.Log("end combo invoked");
        }
    }

    void EndCombo()
    {
        OnAttackEnd?.Invoke();

        Debug.Log("combo ended");

        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
