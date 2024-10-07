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
            IncreasePoints();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        MoveCharacter pikmin = col.GetComponent<MoveCharacter>();
        if (pikmin != null)
        {
            DecreasePoints();
        }
    }

    void IncreasePoints()
    {
        score += 1;
        scoreText.SetText(score.ToString());
    }
    void DecreasePoints()
    {
        score -= 1;
        scoreText.SetText(score.ToString());
    }
}
