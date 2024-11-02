using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // references this forum post : https://discussions.unity.com/t/keep-audio-playing-even-though-i-reset-the-scene/115650/4

    public GameObject[] music;

    void Start()
    {
        music = GameObject.FindGameObjectsWithTag("gameMusic");
        Destroy(music[1]);
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
