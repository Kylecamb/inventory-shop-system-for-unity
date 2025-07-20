// InventoryManager.cs
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json; // Make sure this is still here and Newtonsoft.Json is installed

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<ShopItemSO> inventoryItems = new List<ShopItemSO>();
    public Transform inventoryContainer;
    public GameObject inventoryItemUIPrefab;

    public List<ShopItemSO> allAvailableShopItems; // Assign all your ShopItemSO assets here

    private const string InventorySaveKey = "PlayerInventory";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ShopItemSO item)
    {
        inventoryItems.Add(item);
        UpdateInventoryUI();
        SaveInventory();
    }

    // NEW: Method to remove a specific item from inventory
    public void RemoveItem(ShopItemSO itemToRemove)
    {
        // Find and remove the first occurrence of the item
        if (inventoryItems.Contains(itemToRemove))
        {
            inventoryItems.Remove(itemToRemove);
            Debug.Log($"Removed {itemToRemove.itemName} from inventory.");
            UpdateInventoryUI(); // Update UI after removal
            SaveInventory();    // Save inventory after removal
        }
        else
        {
            Debug.LogWarning($"Attempted to remove {itemToRemove.itemName} but it was not found in inventory.");
        }
    }

    void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItemSO item in inventoryItems)
        {
            GameObject itemUI = Instantiate(inventoryItemUIPrefab, inventoryContainer);
            InventoryItemUI uiComponent = itemUI.GetComponent<InventoryItemUI>();
            uiComponent.Setup(item);
        }
    }

    private void SaveInventory()
    {
        List<string> itemNamesToSave = new List<string>();
        foreach (ShopItemSO item in inventoryItems)
        {
            itemNamesToSave.Add(item.name);
        }

        string json = JsonConvert.SerializeObject(itemNamesToSave);
        PlayerPrefs.SetString(InventorySaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Inventory Saved: " + json);
    }

    private void LoadInventory()
    {
        if (PlayerPrefs.HasKey(InventorySaveKey))
        {
            string json = PlayerPrefs.GetString(InventorySaveKey);
            List<string> loadedItemNames = JsonConvert.DeserializeObject<List<string>>(json);

            inventoryItems.Clear();
            foreach (string itemName in loadedItemNames)
            {
                ShopItemSO item = allAvailableShopItems.Find(so => so.name == itemName);
                if (item != null)
                {
                    inventoryItems.Add(item);
                }
                else
                {
                    Debug.LogWarning($"Could not find ShopItemSO with name '{itemName}' during inventory load.");
                }
            }
            UpdateInventoryUI();
            Debug.Log("Inventory Loaded: " + json);
        }
        else
        {
            Debug.Log("No saved inventory found.");
        }
    }
}