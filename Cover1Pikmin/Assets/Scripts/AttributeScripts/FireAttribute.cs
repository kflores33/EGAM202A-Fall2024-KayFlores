using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour
{
    public GameObject gameManager;
    public bool isPlayerObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MoveCharacter>() == null)
        {
            if (collision.gameObject.GetComponent<ElectricAttribute>() || collision.gameObject.GetComponent<WaterAttribute>())
            {
                gameManager.GetComponent<RestartScene>().restartGame();
            }
        }
    }
}
