using System;
using UnityEngine;

// Talk to Bman about the code

namespace Assets.Scripts.Inventory_System
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Weapon")]
    public class Weapon_SO : ScriptableObject
    {
        [SerializeField] private EWeaponType _weaponType = EWeaponType.Null;
        [SerializeField] private float _weaponPower = 0;
        [SerializeField] private float _weaponSpeed = 0;
        [SerializeField] public GameObject weaponPrefab = null;
        
        public EWeaponType GetWeaponType() => this._weaponType;

        public float WeaponPower
        {
            get => _weaponPower;
            set => _weaponPower = value;
        }

        public float WeaponSpeed
        {
            get => _weaponSpeed;
            set => _weaponSpeed = value;
        }
        
        /// <summary>
        /// Add Weapon types here
        /// </summary>
        [Serializable]
        public enum EWeaponType
        {
            Null,
            Shotgun,
            Pistol,
            Axe,
            Sword,
            Shield
        }
    }
}