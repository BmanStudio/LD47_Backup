using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TickTockAnimatorTrigger : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator = null;
    
    [Space(10)]
    
    [SerializeField, Tooltip("will the trigger called when the ticktock emitter emits the Tick event?")]
    private bool triggerOnTick = false;
    [SerializeField, Tooltip("What is the tick trigger name?")]
    private string tickTriggerName = "Tick";
    
    [Space(10)]
    
    [SerializeField, Tooltip("will the trigger called when the ticktock emitter emits the Tock event?")]
    private bool triggerOnTock = false;
    [SerializeField, Tooltip("What is the tock trigger name?")]
    private string tockTriggerName = "Tock";
    
    [Space(10)]
    
    [SerializeField, Tooltip("will the trigger called when the ticktock emitter emits the TickTock event?")]
    private bool triggerOnTicktock = false;
    [SerializeField, Tooltip("What is the tickTock trigger name?")]
    private string ticktockTriggerName = "TickTock";


    [SerializeField] private bool ignoreAmbiguousNextState = false;

    private void Start()
    {
        if (!ignoreAmbiguousNextState && (triggerOnTick || triggerOnTock) && triggerOnTicktock)
            Debug.LogError("Ticktock should not be used when Tick or Tock are active (Ambiguous next state)");

        if(triggerOnTick) TickTockEmitter.Instance.Ticked += OnTick;
        if(triggerOnTock) TickTockEmitter.Instance.Tocked += OnTock;
        if (triggerOnTicktock) TickTockEmitter.Instance.TickTocked += OnTickTock;
    }

    private void TriggerAnimation(string trigger)
    {
        targetAnimator.SetTrigger(trigger);
    }
    
    public void OnTick(object obj, EventArgs args)
    {
        TriggerAnimation(tickTriggerName);
    }

    public void OnTock(object obj, EventArgs args)
    {
        TriggerAnimation(tockTriggerName);
    }

    public void OnTickTock(object obj, EventArgs args)
    {
        TriggerAnimation(ticktockTriggerName);
    }

    private void OnDestroy()
    {
        TickTockEmitter.Instance.Ticked -= OnTick;
        TickTockEmitter.Instance.Tocked -= OnTock;
        TickTockEmitter.Instance.TickTocked -= OnTickTock;
    }
}
