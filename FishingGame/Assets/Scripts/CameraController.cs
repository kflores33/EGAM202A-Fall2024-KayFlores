using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject fish;
    public GameObject activeCamera;
    public CinemachineVirtualCamera activeCameraShake;
    PlayerStates player;

    public float camShake;

    // Start is called before the first frame update
    void Start()
    {
        //activeCamera = GetComponentInChildren<Active>().gameObject;
        player = FindObjectOfType<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<FishBehavior>() != null)
        {
            fish = FindObjectOfType<FishBehavior>().gameObject;

            activeCamera.GetComponent<CinemachineVirtualCamera>().LookAt = fish.transform;
        }

        if (player.currentState == PlayerStates.PlayerStateMachine.FishingActive)
        {
            // adding camera noise?
            if (player.fishHit)
            {
                ShakeCamera(camShake);
            }
            else
            {
                ShakeCamera(0);
            }
        }
    }

    public void ShakeCamera(float intensity)
    {
        activeCameraShake.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
    }
}
