using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject trail;
    public GameObject struggleParticles;

    public FishBehavior fish;
    public PlayerStates player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        // grab fish reference
        if (GameObject.FindObjectOfType<FishBehavior>() != null)
        {
            fish = FindObjectOfType<FishBehavior>();
        }

        // if there is a fish in scene, play the struggle particles
        if (fish != null) 
        {
            // follow position of fish
            struggleParticles.transform.position = fish.transform.position;

            SetParticlesActive(struggleParticles);
        }
        else
        {
            struggleParticles.SetActive(false);
        }

        if (player.currentState == PlayerStates.PlayerStateMachine.FishingIdle)
        {
            trail.transform.position = player.lastClickLocation;

            SetParticlesActive(trail);
        }
        else if (player.currentState == PlayerStates.PlayerStateMachine.FishingActive)
        {
            if (fish != null)
            {
                struggleParticles.transform.position = fish.transform.position;
            }

            SetParticlesActive(trail);
        }
        else
        {
            trail.SetActive(false);
        }
    }

    private void SetParticlesActive(GameObject particleSystem)
    {
        if (!particleSystem.activeInHierarchy) 
        { 
            particleSystem.SetActive(true);
        }
    }
}
