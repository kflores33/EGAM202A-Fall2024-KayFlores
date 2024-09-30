using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public GameObject gameManager;
    public Camera mainCamera;

    // Start is called before the first frame update
    public void restartGame()
    {
        SceneManager.LoadScene("Pikmin");
        gameManager.GetComponent<ClickManager>().gameCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.GetComponent<DontDestroyOnLoad>().ResetDDOL();
            restartGame();
        }
    }
}
