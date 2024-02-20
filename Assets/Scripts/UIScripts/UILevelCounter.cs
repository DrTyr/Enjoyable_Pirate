using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UILevelCounter : MonoBehaviour
{

    public static UILevelCounter instance { get; private set; }
    public TextMeshProUGUI text;
    public int level = 1;

    void Start()
    {
        instance = this;
        text.text = level.ToString();
    }

    public void SetValue(int level)
    {
        text.text = level.ToString();
    }
}
