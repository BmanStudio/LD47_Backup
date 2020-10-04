using System;
using Assets.Scripts.Inventory_System;
using UnityEngine;

// Talk to Bman about that code:
public class PickupWeapon : InteractableObject
{
    [SerializeField] public Weapon_SO WeaponSo = null;
    
    public override bool Interactable => true;

    public override string HintText
    {
        get
        {
            String str = "Press F to ";
            if (WeaponSo != null)
            {
                if (PlayerInventory.Instance.GetCurrentWeapon() == Weapon_SO.EWeaponType.Null)
                {
                    str += "pickup " + WeaponSo.GetWeaponType();
                }
                else
                {
                    str += "swap your" + PlayerInventory.Instance.GetCurrentWeapon() +
                           " With " + WeaponSo.GetWeaponType();
                }
            }
            return str;
        }
    }

    // User Inputs for floating object
    [SerializeField] float degreesPerSecond = 15.0f;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float frequency = 1f;
        
    // Position Storage Variables
    private Vector3 _posOffset = new Vector3 ();
    private Vector3 _tempPos = new Vector3 ();

    void Start()
    {
        _posOffset = transform.position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        _tempPos = _posOffset;
        _tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = _tempPos;
    }


    public override void Interact()
    {
        PlayerInventory.Instance.AddWeaponToPlayer(WeaponSo);
        Destroy(gameObject, 0.2f);
    }
}
