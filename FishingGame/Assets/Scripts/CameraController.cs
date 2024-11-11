using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject fish;
    GameObject activeCamera;
    // Start is called before the first frame update
    void Start()
    {
        activeCamera = GetComponentInChildren<Active>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<FishBehavior>() != null)
        {
            fish = FindObjectOfType<FishBehavior>().gameObject;

            activeCamera.GetComponent<CinemachineVirtualCamera>().LookAt = fish.transform;
        }

    }
}
