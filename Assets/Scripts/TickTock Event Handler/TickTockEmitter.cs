using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTockEmitter : MonoBehaviour
{
    // Singleton
    public static TickTockEmitter Instance;
    
    // Settings variables
    public float frequency = 1f; // This is the frequency in between the tick and tock
    
    // Event area
    public delegate void TickTockEventHandler(object source, EventArgs args);

    public event TickTockEventHandler Ticked;        // 1000
    public event TickTockEventHandler Tocked;        // 0010
    public event TickTockEventHandler TickTocked;    // 1010

    private bool _lastTicked = false;
    private float _timeUntilNextTickTockedEvent;

    public void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        
        _timeUntilNextTickTockedEvent = 1 / frequency;
        
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (_timeUntilNextTickTockedEvent < 0)
        {
            if(_lastTicked) OnTocked(this);
            else OnTicked(this);

            _lastTicked = !_lastTicked;
            
            _timeUntilNextTickTockedEvent += 1 / frequency;
        }
        _timeUntilNextTickTockedEvent -= Time.deltaTime;
    }
    
    // Only called on Ticks
    protected virtual void OnTicked(object source)
    {
        Ticked?.Invoke(source, EventArgs.Empty);
        // TODO: Play Tick Sound
        
        OnTickTocked(this);
    }

    // Only called on Tocks
    protected virtual void OnTocked(object source)
    {
        Tocked?.Invoke(source, EventArgs.Empty);
        // TODO: Play Tock Sound
        
        OnTickTocked(this);
    }

    // Called on both Ticks and Tocks
    protected virtual void OnTickTocked(object source)
    {
        TickTocked?.Invoke(source, EventArgs.Empty);
    }
}
