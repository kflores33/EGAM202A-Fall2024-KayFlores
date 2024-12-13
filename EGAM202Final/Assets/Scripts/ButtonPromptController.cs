using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPromptController : MonoBehaviour
{
    List<KeyCode> keys = new List<KeyCode> { KeyCode.E, KeyCode.Q};
    public List<Sprite> sprites;

    public Canvas canvas;
    public Image image;

    public KeyCode currentKey;
    public Sprite currentSprite;

    public EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        GetButton();
        canvas.enabled = false;
        //enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetButton()
    {
        int N = keys.Count;
        int key = Random.Range(0, N);

        currentKey = keys[key]; 
        currentSprite = sprites[key];

        image.sprite = currentSprite;
    }

    public void ShowButtonPrompt()
    {
        canvas.enabled = true;
    }
    public void HideButtonPrompt()
    {
        canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == gameObject.GetComponent<PlayerMovement>())
        {
            if (enemyHealth.currentHealthState == EnemyHealth.HealthState.AtRisk)
            {
                ShowButtonPrompt();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == gameObject.GetComponent<PlayerMovement>())
        {
            HideButtonPrompt();
        }
    }
}
