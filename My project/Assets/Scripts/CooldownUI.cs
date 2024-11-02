using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownUI : MonoBehaviour
{
    // references this video:https://www.youtube.com/watch?v=1fBKVWie8ew

    [SerializeField]
    private Image imageCooldown;

    [SerializeField]
    private TMP_Text textCooldown;

    public PlayerActions playerActions;

    public bool isCooldown = false;
    private float cooldownTime;
    private float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;

        cooldownTime = playerActions.parryCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0) 
        { 
            isCooldown = false ;
            textCooldown.gameObject .SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else 
        {
            // sets the text of the countdown and rounds the value to a whole number
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();

            imageCooldown.fillAmount = cooldownTimer/cooldownTime;
        }
    }

    public void UseParry()
    {
        if (isCooldown)
        {
            // user has clicked spell while cooldown is still active
            Debug.Log("wait bro");
        }
        else
        {
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);

            cooldownTimer = cooldownTime;
        }
    }
}
