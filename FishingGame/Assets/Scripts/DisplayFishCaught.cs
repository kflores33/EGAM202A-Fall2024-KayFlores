using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayFishCaught : MonoBehaviour
{
    public GameObject fishCaughtPanel;
    public TMP_Text textToEdit;

    public FishBehavior fish;

    // Start is called before the first frame update
    void Start()
    {
        fishCaughtPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<FishBehavior>() != null)
        {
            fish = FindObjectOfType<FishBehavior>();
        }

        if (fish != null)
        {
            textToEdit.text = "You landed a " + fish.data.fishName + "!";
        }
    }

    public void FishCaught(bool fishCaught)
    {
        if (!fishCaught) { fishCaughtPanel.SetActive(false); }
        else
        {
            fishCaughtPanel.SetActive(true);
        }
    }
}
