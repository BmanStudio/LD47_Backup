using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Inventory_System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem),
    typeof(PlayerController))]
public class PassiveStatsManager : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem = null;
    [SerializeField] private PlayerController _movementSystem  = null;
    [SerializeField] public PlayerWeapon _playerWeapon = null;
    public void UpdatePlayerStats(Dictionary<CarriableItem_SO.EItemEffectType, float> passiveEffectDict)
    {
        if (passiveEffectDict.ContainsKey(CarriableItem_SO.EItemEffectType.MoveSpeed))
        {
            passiveEffectDict.TryGetValue(CarriableItem_SO.EItemEffectType.MoveSpeed, out float value);
            _movementSystem.UpdateSpeedPassiveBonus(value);
        }

        if (passiveEffectDict.ContainsKey(CarriableItem_SO.EItemEffectType.MaxHealth))
        {
            passiveEffectDict.TryGetValue(CarriableItem_SO.EItemEffectType.MaxHealth, out float value);
            _healthSystem.UpdateHealthPassiveBonus(value);
        }
        
        if (passiveEffectDict.ContainsKey(CarriableItem_SO.EItemEffectType.AttackPower))
        {
            passiveEffectDict.TryGetValue(CarriableItem_SO.EItemEffectType.AttackPower, out float value);
            _playerWeapon.UpdateDamagePassiveBonus(value);
        }
    }
}
