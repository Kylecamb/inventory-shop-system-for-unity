// ShopItemSO.cs
using UnityEngine;
using UnityEngine.UI;

// Define an enum for different equipment slot types
public enum EquipmentSlot
{
    None, // For items that don't equip
    Head,
    RightHand, // Using Hand for held items, could be Wrist too
    LeftHand,
    Torso, // For chest armor/clothing
    // Add more as needed (e.g., Feet, Legs, Back, etc.)
}

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Item")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public GameObject itemPrefab; // The 3D model prefab to instantiate

    public bool isConsumable; // If true, item is removed after use (e.g., potion)

    // NEW: Equipment slot type for this item
    public EquipmentSlot equipmentSlot = EquipmentSlot.None;
    public Vector3 attachedOffset = Vector3.zero; // Local position offset when attached
    public Vector3 attachedRotation = Vector3.zero; // Local Euler angles rotation when attached
}