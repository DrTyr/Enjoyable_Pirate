using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFloatingSliderBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform target;

    //Be set in UNITY Script to move the bar away from the center of the object
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        //Rotate the bar following the camera set in the script in UNITY
        transform.rotation = camera.transform.rotation;
        //To place the bar always on top even if object in rotating
        transform.position = target.position + offset;
    }

    public void UpdateSlideBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;

    }
}
