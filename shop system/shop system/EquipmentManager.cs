// EquipmentManager.cs
using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    // Public Transforms for each equipment slot
    public Transform headPoint;
    public Transform rightHandPoint;
    public Transform leftHandPoint;
    public Transform torsoPoint;
    // Add more points as per your EquipmentSlot enum

    // Dictionary to map slot type to its Transform AND currently equipped item
    private Dictionary<EquipmentSlot, Transform> slotTransforms;
    private Dictionary<EquipmentSlot, GameObject> equippedItems; // Stores the currently instantiated item GO

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Useful if your player (and equipment) persists
        }

        // Initialize dictionaries
        slotTransforms = new Dictionary<EquipmentSlot, Transform>()
        {
            { EquipmentSlot.Head, headPoint },
            { EquipmentSlot.RightHand, rightHandPoint },
            { EquipmentSlot.LeftHand, leftHandPoint },
            { EquipmentSlot.Torso, torsoPoint },
            // Add other points here
        };

        equippedItems = new Dictionary<EquipmentSlot, GameObject>();
    }

    /// <summary>
    /// Toggles equipping/unequipping an item.
    /// </summary>
    /// <param name="itemSO">The ScriptableObject of the item to equip/unequip.</param>
    /// <returns>True if the item's state was toggled, false if no slot or prefab.</returns>
    public bool ToggleEquip(ShopItemSO itemSO)
    {
        if (itemSO.equipmentSlot == EquipmentSlot.None || itemSO.itemPrefab == null)
        {
            Debug.LogWarning($"Cannot equip '{itemSO.itemName}': Not an equipable item or missing prefab.", this);
            return false;
        }

        EquipmentSlot targetSlot = itemSO.equipmentSlot;

        // Get the actual Transform for this slot
        if (!slotTransforms.TryGetValue(targetSlot, out Transform attachPointTransform) || attachPointTransform == null)
        {
            Debug.LogError($"Equipment point for slot '{targetSlot}' not assigned in EquipmentManager!", this);
            return false;
        }

        // Check if something is already equipped in this slot
        if (equippedItems.ContainsKey(targetSlot) && equippedItems[targetSlot] != null)
        {
            // Something is equipped. Is it the SAME item? (This is for toggling it off)
            // You might want a more robust check here if multiple itemSOs can use the same prefab
            if (equippedItems[targetSlot].name.Contains(itemSO.itemPrefab.name)) // Simple check based on prefab name
            {
                // It's the same item, so unequip it.
                Unequip(targetSlot);
                return true;
            }
            else
            {
                // Different item is equipped, so unequip the current one and equip the new one.
                Unequip(targetSlot); // Unequip the old item first
                Equip(itemSO, attachPointTransform, targetSlot); // Then equip the new one
                return true;
            }
        }
        else
        {
            // Nothing equipped, so equip the item.
            Equip(itemSO, attachPointTransform, targetSlot);
            return true;
        }
    }

    private void Equip(ShopItemSO itemSO, Transform attachPointTransform, EquipmentSlot targetSlot)
    {
        // Instantiate the item as a child of the attachment point
        GameObject equippedGO = Instantiate(itemSO.itemPrefab, attachPointTransform);

        // Apply local offset and rotation
        equippedGO.transform.localPosition = itemSO.attachedOffset;
        equippedGO.transform.localRotation = Quaternion.Euler(itemSO.attachedRotation);

        // Store the equipped item instance
        equippedItems[targetSlot] = equippedGO;
        Debug.Log($"Equipped '{itemSO.itemName}' to {targetSlot} slot.");
    }

    private void Unequip(EquipmentSlot targetSlot)
    {
        if (equippedItems.ContainsKey(targetSlot) && equippedItems[targetSlot] != null)
        {
            Debug.Log($"Unequipped '{equippedItems[targetSlot].name}' from {targetSlot} slot.");
            Destroy(equippedItems[targetSlot]);
            equippedItems.Remove(targetSlot);
        }
    }

    // You can add a method to get the currently equipped item in a slot if needed
    public GameObject GetEquippedItem(EquipmentSlot slot)
    {
        equippedItems.TryGetValue(slot, out GameObject item);
        return item;
    }
}