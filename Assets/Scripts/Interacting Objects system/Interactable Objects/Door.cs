using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    // Abstract properties
    public override string HintText => open ? "Close door" : "Open door";
    public override bool Interactable => true; // TODO: Key logic in here

    // Local variables
    public bool open;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private float closedRotation = 0f;
    [SerializeField] private float openRotation = 90f;
    [SerializeField] private float actionSpeed = 2f;

    private void Awake()
    {
        if(!rootTransform) rootTransform = transform;
    }

    public override void Interact()
    {
        open = !open;
    }

    private void FixedUpdate()
    {
        RotateDoor();
    }

    private void RotateDoor()
    {
        if (Math.Abs(rootTransform.localRotation.eulerAngles.y - (open ? openRotation : closedRotation)) > 1f)
        {
            var rotationalDistanceToObjective =
                ((open ? openRotation : closedRotation) - rootTransform.localRotation.eulerAngles.y);
            var rotationalDirection =
                Mathf.Sign(Mathf.Abs(rotationalDistanceToObjective) > 180
                    ? -Mathf.Sign(rotationalDistanceToObjective)
                    : Mathf.Sign(rotationalDistanceToObjective));
            var newRot = rootTransform.localRotation.eulerAngles.y +
                         rotationalDirection * actionSpeed * Time.fixedDeltaTime;

            
            rootTransform.localRotation = Quaternion.Euler(new Vector3(0, newRot, 0));
        }
    }
}
