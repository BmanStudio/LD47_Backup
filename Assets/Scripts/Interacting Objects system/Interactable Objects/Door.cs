using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    // Abstract properties
    //public override string HintText => open ? "Close door" : "Open door";
    public override string HintText
    {
        get
        {
            String str = "";
            if (_isLocked)
            {
                str += "Door is locked forever";
            }
            else
            {
                if (open)
                {
                    str += "Press F to close door";
                }
                else
                {
                    str += "Press F to open door";
                }
            }

            return str;
        }
    }

    public override bool Interactable => true; // TODO: Key logic in here

    // Local variables
    public bool open;
    [SerializeField] private Transform _rootTransform;
    [SerializeField] private float closedRotation = 0f;
    [SerializeField] private float openRotation = 90f;
    [SerializeField] private float actionSpeed = 2f;
    [SerializeField] private bool _isStartingLocked = false;
    [SerializeField] private DoorLockTrigger _doorLockTrigger = null;
    //[SerializeField] private Transform _doorMesh = null;
    
    private bool _isLocked = false;

    void OnEnable()
    {
        if (_isStartingLocked)
        {
            _isLocked = true;
            open = false;
        }
        
        if (_doorLockTrigger != null)
        {
            _doorLockTrigger.OnPlayerTrigger += LockDoor;
        }
    }
    
    void OnDisable()
    {
        if (_doorLockTrigger != null)
        {
            _doorLockTrigger.OnPlayerTrigger -= LockDoor;
        }
    }

    private void Awake()
    {
        if(!_rootTransform) _rootTransform = transform;
    }

    public override void Interact()
    {
        if (!_isLocked)
        {
            open = !open;
        }
    }

    private void LockDoor()
    {
        Debug.Log("Hey");
        _isLocked = true;
        open = false;
        //transform.Rotate(0, 180,0, Space.World);
    }

    private void FixedUpdate()
    {
        RotateDoor();
    }

    private void RotateDoor()
    {
        if (Math.Abs(_rootTransform.localRotation.eulerAngles.y - (open ? openRotation : closedRotation)) > 1f)
        {
            var rotationalDistanceToObjective =
                ((open ? openRotation : closedRotation) - _rootTransform.localRotation.eulerAngles.y);
            var rotationalDirection =
                Mathf.Sign(Mathf.Abs(rotationalDistanceToObjective) > 180
                    ? -Mathf.Sign(rotationalDistanceToObjective)
                    : Mathf.Sign(rotationalDistanceToObjective));
            var newRot = _rootTransform.localRotation.eulerAngles.y +
                         rotationalDirection * actionSpeed * Time.fixedDeltaTime;

            
            _rootTransform.localRotation = Quaternion.Euler(new Vector3(0, newRot, 0));
        }
    }
}
