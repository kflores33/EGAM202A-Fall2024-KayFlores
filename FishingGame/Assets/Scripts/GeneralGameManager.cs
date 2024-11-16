using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneralGameManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;

    public TMP_Text fishCount;

    public int fishCaught;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFishCaught()
    {
        fishCaught++;

        fishCount.text = "Fish Caught: " + fishCaught.ToString();

        if (fishCaught == 5) 
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void TriggerLoseScreen()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
