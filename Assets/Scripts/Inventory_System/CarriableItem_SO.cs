using System;
using UnityEngine;

// Talk to Bman about the code

namespace Assets.Scripts.Inventory_System
{
    /// <summary>
    /// The SO of a carriable item that got some effect on the player
    /// </summary>
    [CreateAssetMenu(fileName = "New Carriable Item", menuName = "Inventory System/Carriable Item")]
    public class CarriableItem_SO : ScriptableObject
    {
        [SerializeField] private EItemType _itemType = EItemType.Null;
        [SerializeField] private EItemEffectType _itemEffectType = EItemEffectType.Null;
        [SerializeField] private float _effectValue = 0;
        
        public EItemType GetItemType() => this._itemType;

        public EItemEffectType GetItemEffectType() => this._itemEffectType;

        public float EffectValue
        {
            get => _effectValue;
            set => _effectValue = value;
        }
        
        /// <summary>
        /// Add effect items here
        /// </summary>
        [Serializable]
        public enum EItemType
        {
            Null,
            Ring,
            Crown,
            Wrist,
            Mirror
        }

        [Serializable]
        public enum EItemEffectType
        {
            Null,
            MoveSpeed,
            AttackPower,
            AttackSpeed,
            MaxHealth
        }
    }
}
