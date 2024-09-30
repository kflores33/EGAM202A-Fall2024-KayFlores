using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttribute : MonoBehaviour
{
    public GameObject gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MoveCharacter>() == null)
        {
            if (collision.gameObject.GetComponent<ElectricAttribute>() || collision.gameObject.GetComponent<FireAttribute>())
            {
                gameManager.GetComponent<RestartScene>().restartGame();
            }
        }
    }
}
