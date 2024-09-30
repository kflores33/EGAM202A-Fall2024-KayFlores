using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalPoint : MonoBehaviour
{
    public int score = 0;

    bool fireHasScored;
    bool waterHasScored;
    bool electricHasScored;

    public TMP_Text scoreText;
    public GameObject winScreen;
    public GameObject gameTimer;

    // Update is called once per frame
    void Update()
    {
        if (score == 3)
        {
            // win stuff
            winScreen.SetActive(true);
            gameTimer.GetComponent<GameTimer>().timerIsRunning = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        MoveCharacter pikmin = col.GetComponent<MoveCharacter>();
        if (pikmin != null)
        {
            // check which pikmin it is
            if (pikmin.GetComponent<FireAttribute>() != null && !fireHasScored)
            {
                IncreasePoints();
                fireHasScored = true;
            }
            else if (pikmin.GetComponent<WaterAttribute>() != null && !waterHasScored)
            {
                IncreasePoints();
                waterHasScored = true;
            }
            else if(pikmin.GetComponent<ElectricAttribute>() != null && !electricHasScored)
            {
                IncreasePoints();
                electricHasScored = true;
            }
        }
    }

    void IncreasePoints()
    {
        score += 1;
        scoreText.SetText(score.ToString());
    }
}
