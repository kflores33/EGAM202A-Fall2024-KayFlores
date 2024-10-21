using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public enum ActionStates
    {
        Default,
        Parry
    }
    public ActionStates currentState;

    public CooldownUI cooldownUI;

    [Header("Keybinds")]
    public KeyCode parryKey = KeyCode.Mouse0;
    public KeyCode launchKey = KeyCode.Mouse1;

    [Header("Parry Variables")]
    public bool canParry;

    public float parryDuration = 1f;
    public float parryCooldown = 2f;

    public float enemyKnockback = 5f;
    public float knockbackRadius = 10f;

    public Coroutine knockbackAllowance;

    public Coroutine parryCooldownCoroutine;
    public Coroutine parryActiveCoroutine;

    public AuraTriggerDetection parryAura;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ActionStates.Default;
        canParry = true;
        parryAura.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) 
        { 
            case ActionStates.Default:
                UpdateDefault(); break;
            case ActionStates.Parry:
                UpdateParry(); break;
        }

        if (cooldownUI.isCooldown)
        {
            cooldownUI.ApplyCooldown();
        }
    }

    void UpdateDefault()
    {
        // switch to parry state when parry button is pressed (and if cooldown is at 0)
        if (Input.GetKey(parryKey) && canParry) 
        {        
            currentState = ActionStates.Parry;
        }
    }
    void UpdateParry()
    {
        // parry orb shows up for a few seconds (makes player invulnerable to damage)
        StartParryState();

        // if there's a collision with an enemy, knock the enemy back and add a launch charge
        if (parryAura.hitWhileParry) 
        {
            ParrySuccess();
        }
    }

    // starts the parry state & duration timer
    void StartParryState()
    {
        canParry = false;   

        parryAura.gameObject.SetActive(true);

        if (parryActiveCoroutine == null)
        {
            parryActiveCoroutine = StartCoroutine(ParryActiveCoroutine());
        }
    }

    // ends parry state and starts cooldown timer
    void DisableParry()
    {
        Debug.Log("parry disabled");
        parryAura.gameObject.SetActive(false );

        if (parryActiveCoroutine != null)
        {
            StopCoroutine(parryActiveCoroutine);
            parryActiveCoroutine = null;
        }

        // return to default state
        currentState = ActionStates.Default;

        cooldownUI.UseParry();

        // start cooldown (move to after parry is finished executing)
        if (parryCooldownCoroutine == null)
        {
            parryCooldownCoroutine = StartCoroutine(ParryCooldownCoroutine());
        }
    }

    // resets parry and ends cooldown coroutine
    void ResetParry()
    {
        Debug.Log("parry enabled");
        canParry = true;    

        if (parryCooldownCoroutine != null)
        {
            StopCoroutine(parryCooldownCoroutine);
            parryCooldownCoroutine = null;
        }        
    }
    // clears the aura variables and disables parry upon success
    void ParrySuccess()
    {
        parryAura.hitWhileParry = false;
        parryAura.enemies.Clear();
        DisableParry();
    }

    // starts cooldown
    IEnumerator ParryCooldownCoroutine()
    {
        yield return new WaitForSeconds(parryCooldown);

        //Debug.Log("can parry again");
        ResetParry();        

        yield break;
    }

    // waits till end of duration to disable parry state
    IEnumerator ParryActiveCoroutine() 
    {
        yield return new WaitForSeconds(parryDuration);

        DisableParry();        

        yield break;
    }
}
