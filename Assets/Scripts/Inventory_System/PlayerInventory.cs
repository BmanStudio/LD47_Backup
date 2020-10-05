using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Inventory_System;
using Unity.Mathematics;
using UnityEngine;

// Talk to Bman about the code

[RequireComponent(typeof(PlayerController))]
public class PlayerInventory : MonoBehaviour
{
    // Singleton:
    public static PlayerInventory Instance = null;
    
    [SerializeField] private Weapon_SO _startingWeapon = null;
    [SerializeField] private CarriableItem_SO[] _startingItems = null;
    [SerializeField] private int _itemSlots = 2;


    [SerializeField] private PassiveStatsManager _passiveStatsManager = null;
    [SerializeField] private Transform weaponSlotGameObject = null;

    // The current equipped:
    private Weapon_SO _currentWeapon = null;
    private GameObject _currentWeaponGameObject = null;
    
    private LinkedList<CarriableItem_SO> _currentItems;

    private PlayerController _playerController;
    
    // For the passive items effects
    // Dict <Effect Type,Current Value>:
    private Dictionary<CarriableItem_SO.EItemEffectType, float> _passiveEffectDict;
    
    // Init for the linked list by the starting items array
    // and assigning the starting weapon to the current one
    void Awake()
    {
        // Singleton:
        
        if (Instance != null && Instance != this) 
        {
            Destroy(this.gameObject);
        }
        
        Instance = this;
        
    }

    void OnEnable()
    {

        if (_startingItems.Length > 0 && _startingItems.Length <= _itemSlots)
        {
            _currentItems = new LinkedList<CarriableItem_SO>(_startingItems);
            _passiveEffectDict = new Dictionary<CarriableItem_SO.EItemEffectType, float>();
            UpdatePassiveStatsManager();
        }
        else
        {
            Debug.LogWarning("Player doesnt have any starting items, or there are more items than slots");
        }
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        
        if (_startingWeapon != null)
        {
            AddWeaponToPlayer(_startingWeapon);
        }
        else
        {
            Debug.LogWarning("Player doesnt have any starting weapon");
        }
    }
    
    /// <summary>
    /// Call this method to add Carriable Item (for weapon call AddWeaponToPlayer)
    /// If the inventory full - first removes the last item in the list
    /// (the oldest item)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToPlayer(CarriableItem_SO item)
    {
        if (item != null)
        {
            // For safety, if the count is bigger than slots
            if (_currentItems.Count > _itemSlots)
            {
                Debug.LogError("More items than slots!");
            }
            
            // If the item slots are full:
            // TODO add player selection to decide if to remove item, which item or to not remove at all
            else if (_currentItems.Count == _itemSlots)
            {
                Debug.Log("Items slots are full! have to remove item " + _currentItems.Last.Value + " of type " +
                          _currentItems.Last.Value.GetItemType());
                _currentItems.RemoveLast();
            }
            
            // If got free slot, adding the item:
            if (_currentItems.Count < _itemSlots)
            {
                _currentItems.AddFirst(item);
                Debug.Log("Added item " + item.name + " of type " + item.GetItemType() +
                          ". the current items length is " + _currentItems.Count);
                
                UpdatePassiveStatsManager();
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Call this method to add Weapon (for item call AddItemToPlayer)
    /// for swapping weapon call swap
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public bool AddWeaponToPlayer(Weapon_SO weapon)
    {
        // TODO ask the player if he wants to switch
        if (weapon != null)
        {
            Debug.Log("Switching weapon from " + _currentWeapon + " to " + weapon);
            SwapCurrentWeapon(weapon);
            if (_currentWeaponGameObject.GetComponent<PlayerWeapon>())
            {
                var playerWeapon = _currentWeaponGameObject.GetComponent<PlayerWeapon>();
                playerWeapon.fireClips = weapon.fireSounds;
                _passiveStatsManager._playerWeapon = playerWeapon;
                _playerController.Weapon = playerWeapon;
            }
            else
            {
                Debug.LogError("Something wrong here with PlayerWeapon");
            }
            Debug.Log("Current weapon now is " + _currentWeapon);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Call this method to find if there is an item
    /// of type EItemType (could be multiple)
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public bool DoesHaveItem(CarriableItem_SO.EItemType itemType)
    {
        var cur = _currentItems.First;
        for (int i = 0; i < _currentItems.Count; i++)
        {
            if (cur != null && cur.Value.GetItemType() == itemType)
            {
                return true;
            }

            if (cur != null) cur = cur.Next;
        }

        return false;
    }

    public Weapon_SO.EWeaponType GetCurrentWeapon()
    {
        if (_currentWeapon == null)
        {
            return Weapon_SO.EWeaponType.Null;
        }
        return _currentWeapon.GetWeaponType();
    }

    /// <summary>
    /// Call this method to remove item of EItemType.
    /// if not specified - will delete the oldest item in the inventory.
    /// if there is more than one item of the same type in the inventory,
    /// will remove the "new" one.
    /// </summary>
    /// <param name="itemType"></param>
    public void RemoveItem(CarriableItem_SO.EItemType itemType = CarriableItem_SO.EItemType.Null)
    {
        // If not specified - will delete the oldest item in the list.
        if (itemType == CarriableItem_SO.EItemType.Null)
        {
            _currentItems.RemoveLast();
            Debug.Log("Deleted the last item because nothing else was specified");
            UpdatePassiveStatsManager();
            return;
        }
        var cur = _currentItems.First;
        for (int i = 0; i < _currentItems.Count; i++)
        {
            if (cur != null && cur.Value.GetItemType() == itemType)
            {
                Debug.Log("Removing the item " + cur.Value.GetItemType());
                _currentItems.Remove(cur);
                UpdatePassiveStatsManager();
                return;
            }

            if (cur != null) cur = cur.Next;
        }
    }
    
    /// <summary>
    /// Call this method to remove the current equipped weapon
    /// Basically set the currentWeapon to null
    /// </summary>
    public void RemoveWeapon()
    {
        _currentWeapon = null;
    }

    public int GetCurrentItemsCount() => _currentItems.Count;

    public bool CanAddMoreItems() => _currentItems.Count < _itemSlots;

    public CarriableItem_SO.EItemType GetLastItemType() => _currentItems.Last.Value.GetItemType();
    
    private void UpdatePassiveItemsBonusDict()
    {
        _passiveEffectDict.Clear();
        
        var cur = _currentItems.First;
        for (int i = 0; i < _currentItems.Count; i++)
        {
            if (cur != null)
            {
                // If there is already an item with that effect in the dict
                // it will make a sum of them, then delete the old one
                // and create a new one - representing both of them
                // so we dont have to keep duplicates
                var currType = cur.Value.GetItemEffectType();
                if (_passiveEffectDict.ContainsKey(currType))
                {
                    _passiveEffectDict.TryGetValue(currType, out var currentDictValue);
                    var sum = cur.Value.EffectValue + currentDictValue;
                    _passiveEffectDict.Remove(currType);
                    _passiveEffectDict.Add(currType, sum);
                }
                else
                {
                    _passiveEffectDict.Add(currType, cur.Value.EffectValue);
                }
            }

            if (cur != null) cur = cur.Next;
        }
    }

    private void UpdatePassiveStatsManager()
    {
        UpdatePassiveItemsBonusDict();
        if (_passiveEffectDict.Count <= 0)
        {
            Debug.LogError("Something wrong with the passiveEffectDict" + _passiveEffectDict + this);
            return;
        }
        _passiveStatsManager.UpdatePlayerStats(_passiveEffectDict);
    }

    private void SwapCurrentWeapon(Weapon_SO weaponSo)
    {
        if (_currentWeaponGameObject != null)
        {
            if (_currentWeapon == null)
            {
                Debug.LogError("Something wrong with the weapon SO here");
            }
            //TODO change if want to drop current weapon on the floor
            Destroy(_currentWeaponGameObject);
            _currentWeaponGameObject = null;
            _currentWeapon = null;
        }

        if (_currentWeapon == null && _currentWeaponGameObject == null)
        {
            if (weaponSo.weaponPrefab != null)
            {
                _currentWeaponGameObject =
                    Instantiate(original:weaponSo.weaponPrefab, position:weaponSlotGameObject.position,
                        Quaternion.identity, parent:weaponSlotGameObject);
                _currentWeaponGameObject.transform.localRotation = Quaternion.Euler(0,0,0);
                _currentWeapon = weaponSo;
                _currentWeaponGameObject.transform.parent= weaponSlotGameObject;
            }
        }
    }

    public Transform GetPlayerTransform()
    {
        return GetComponent<Transform>();
    }

}