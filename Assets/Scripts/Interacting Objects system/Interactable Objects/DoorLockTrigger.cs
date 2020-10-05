using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DoorLockTrigger : MonoBehaviour
{
    public delegate void DoorLock();

    public event DoorLock OnPlayerTrigger;

    private Vector3 startPos;

    void OnEnable()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = startPos;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hey 1");
        if (other.transform.root.gameObject == PlayerInventory.Instance.gameObject)
        {
            Debug.Log("Hey 2");
            OnPlayerTrigger?.Invoke();
        }
    }

}
