using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterDot : MonoBehaviour
{
    // Singleton
    public static CenterDot Instance;

    // Static variables
    public static Color
        NormalColor = Color.white,
        InteractColor = Color.green,
        EnemyColor = Color.red,
        DisabledColor = Color.grey,
        SpeakColor = Color.blue;
    
    // Component references
    [SerializeField] private Image centerDotImage;

    private void Awake()
    {
        if (Instance) Destroy(Instance.gameObject);

        Instance = this;

        if (!centerDotImage) centerDotImage = GetComponent<Image>();
    }

    public static void ChangeColor(Color c)
    {
        Instance.centerDotImage.color = c;
    }
}
