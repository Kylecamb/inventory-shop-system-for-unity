// InventoryItemUI.cs
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Button useButton;
    private ShopItemSO itemData;

    public void Setup(ShopItemSO item)
    {
        itemData = item;
        itemIcon.sprite = item.icon;
        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);
    }

    private void UseItem()
    {
        if (itemData.itemPrefab == null)
        {
            Debug.LogWarning($"Item prefab is null for {itemData.itemName}.", this);
            return;
        }

        // --- Handle Equipable Items ---
        if (itemData.equipmentSlot != EquipmentSlot.None)
        {
            if (EquipmentManager.Instance != null)
            {
                bool toggled = EquipmentManager.Instance.ToggleEquip(itemData);
                if (toggled)
                {
                    // If it was successfully toggled (equipped or unequipped),
                    // and if it's NOT a consumable, we're done here.
                    if (!itemData.isConsumable)
                    {
                        return; // Prevent further spawning/consumption logic for equipable items
                    }
                }
            }
            else
            {
                Debug.LogError("EquipmentManager.Instance is null! Cannot equip item.", this);
            }
        }

        // --- Handle Consumable Items (and non-equipable items that just spawn) ---
        // This part runs IF the item is NOT an equipable, or if it's an equipable
        // that is ALSO consumable (e.g., a grenade that gets thrown).
        // For a true "toggle" equip, consumable logic wouldn't apply here.
        // Re-introduce default spawn distance and direction here for non-attached items
        float defaultSpawnDistance = 2f; // You can make this configurable or a default
        Vector3 defaultSpawnDirection = Vector3.forward; // You can make this configurable or a default

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPos = player.transform.position + player.transform.TransformDirection(defaultSpawnDirection) * defaultSpawnDistance;
            Instantiate(itemData.itemPrefab, spawnPos, Quaternion.identity);
            Debug.Log($"Spawned {itemData.itemName} at {spawnPos} from Player tag.");
        }
        else
        {
            Debug.LogError("No GameObject with 'Player' tag found! Cannot spawn item at player location.", this);
        }

        // --- Consume Logic (for items marked as consumable) ---
        if (itemData.isConsumable)
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RemoveItem(itemData); // Tell the manager to remove it
            }
            else
            {
                Debug.LogError("InventoryManager.Instance is null! Cannot remove consumable item.", this);
            }
        }
    }
}