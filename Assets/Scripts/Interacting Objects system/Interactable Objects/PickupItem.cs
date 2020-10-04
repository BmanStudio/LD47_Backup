using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Inventory_System;
using UnityEngine;

// Talk to Bman about the code

public class PickupItem : InteractableObject
{
    [SerializeField] public CarriableItem_SO ItemSO = null;
    
    public override bool Interactable => true;

    public override string HintText
    {
        get
        {
            String str = "Press F to ";
            if (ItemSO != null)
            {
                if (PlayerInventory.Instance.CanAddMoreItems())
                {
                    str += "pickup " + ItemSO.GetItemType();
                }
                else
                {
                    str += "swap your" + PlayerInventory.Instance.GetLastItemType() +
                           " With " + ItemSO.GetItemType();
                }
            }

            return str;
        }
    }

    // User Inputs for floating object
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
        
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }


    public override void Interact()
    {
        PlayerInventory.Instance.AddItemToPlayer(ItemSO);
    }
}
