using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttribute : MonoBehaviour
{
    public GameObject gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MoveCharacter>() == null)
        {
            if (collision.gameObject.GetComponent<FireAttribute>() || collision.gameObject.GetComponent<WaterAttribute>())
            {
                gameManager.GetComponent<RestartScene>().restartGame();
            }
        }
    }
}
