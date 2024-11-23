using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensControl : MonoBehaviour
{
    public Slider slider;
    public PlayerCamera playerCamera;

    public float minSens;
    public float maxSens;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxSens;
        slider.minValue = minSens;

        slider.value = (maxSens + minSens)/2;

        playerCamera = FindObjectOfType<PlayerCamera>();

        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        playerCamera.sensX = slider.value;
        playerCamera.sensY = slider.value;
    }
}
