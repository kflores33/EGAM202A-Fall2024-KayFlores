using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // references this forum post : https://discussions.unity.com/t/keep-audio-playing-even-though-i-reset-the-scene/115650/4

    public GameObject[] timer;

    void Start()
    {
        timer = GameObject.FindGameObjectsWithTag("timer");
        if (timer.Length > 1)
        {
            Destroy(timer[1]);
        }
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ResetDDOL()
    {
        Destroy(timer[0]);
    }
}
