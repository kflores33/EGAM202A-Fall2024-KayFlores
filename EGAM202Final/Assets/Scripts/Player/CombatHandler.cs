using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    public List<AttackData> combo1;

    public List<ComboList> comboList;

    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;

    [SerializeField] KeyCode lightAttack = KeyCode.Mouse0;

    Animator anim;
    [SerializeField] AttackHandler attackHandler;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
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

            if (Time.time - lastClickedTime >= 0.2f)
            {
                // takes attack from combo list (corresponding to current attack)
                anim.runtimeAnimatorController = combo1[comboCounter].animatorOV;
                anim.Play("Attack", anim.GetLayerIndex("Attack"), 0);
                //attackHandler.damage = combo1[comboCounter].damage;

                comboCounter++;
                lastClickedTime = Time.time;

                if(comboCounter >= combo1.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
