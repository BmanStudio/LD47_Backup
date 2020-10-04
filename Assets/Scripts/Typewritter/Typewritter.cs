using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.UIElements;
using UnityEngine;

public class Typewritter : MonoBehaviour
{
    // Singleton
    public static Typewritter Instance;
    
    // Component references
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private Animator animator;

    private string _text;    // The text that will be shown on the message
    private float _speed;
    private float AcceleratedSpeed => 10 * _speed;

    private float _currentTime;
    
    // Animation string hashes
    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Hide = Animator.StringToHash("Hide");

    private void Awake()
    {
        if (Instance) Destroy(gameObject);

        if (!animator) animator = GetComponent<Animator>();
        if (!textMesh) textMesh = GetComponent<TextMesh>();
        
        Instance = this;
    }

    public void ShowMessageHud(bool clear = true)
    {
        if (clear) textMesh.text = "";
        
        animator.SetTrigger(Show);
    }

    public void HideMessageHud()
    {
        animator.SetTrigger(Hide);
    }

    public void NewMessage(string text, float speed)
    {
        _speed = speed;
        _text = text;
    }
}
