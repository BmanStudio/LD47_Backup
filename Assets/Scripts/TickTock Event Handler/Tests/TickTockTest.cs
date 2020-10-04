using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an example on how the Tick Tock works
/// </summary>
public class TickTockTest : MonoBehaviour
{
    
    public bool isTick;
    public string messageTick;
    
    public bool isTock;
    public string messageTock;
    
    public bool isTickTock;
    public string messageTickTock;
    
    // Start is called before the first frame update
    void Start()
    {
        if(isTick) TickTockEmitter.Instance.Ticked += OnTick;
        if(isTock) TickTockEmitter.Instance.Tocked += OnTock;
        if(isTickTock) TickTockEmitter.Instance.TickTocked += OnTickTock;
    }

    public void OnTick(object obj, EventArgs args)
    {
        Debug.Log(messageTick);
    }

    public void OnTock(object obj, EventArgs args)
    {
        Debug.Log(messageTock);
    }

    public void OnTickTock(object obj, EventArgs args)
    {
        Debug.Log(messageTickTock);
    }
}
