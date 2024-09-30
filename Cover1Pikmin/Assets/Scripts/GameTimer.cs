using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning;
    public Slider sliderTimer;
    public TMP_Text timeText;

    public GameObject loseText;
    public GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 30f;
        timerIsRunning = true;

        Time.timeScale = 1;

        // slider stuff
        sliderTimer.maxValue = timeRemaining;
        sliderTimer.value = timeRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            DisplayTime(timeRemaining);

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                sliderTimer.value = timeRemaining;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                loseText.SetActive(true);
                //Time.timeScale=0;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
