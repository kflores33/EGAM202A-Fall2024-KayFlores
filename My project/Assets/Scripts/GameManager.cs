using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int enemiesLeft;
    public PlayerHealth playerHealth;

    public GameObject winScreen;
    public GameObject loseScreen;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        enemiesLeft = GameObject.FindObjectsOfType<EnemyHealth>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        //enemiesLeft = GameObject.FindObjectsOfType<EnemyHealth>().Length;

        //if (enemiesLeft == 0)
        //{
        //    // win
        //    winScreen.SetActive(true);
        //}
        if (playerHealth.currentHealth == 0) 
        { 
            // lose
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
