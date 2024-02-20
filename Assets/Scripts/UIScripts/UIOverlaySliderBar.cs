using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlaySliderBar : MonoBehaviour
{
    public static UIOverlaySliderBar instance { get; private set; }

    [SerializeField] private Slider slider;


    void Start()
    {

        instance = this;
        slider.value = 0;

    }

    public void UpdateSlideBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;

    }
}
