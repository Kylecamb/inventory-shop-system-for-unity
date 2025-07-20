// InventoryPanelActivator.cs (Modified)
using UnityEngine;

public class InventoryPanelActivator : MonoBehaviour
{
    public GameObject inventoryPanel; // Drag your Inventory UI Panel here

    void Start()
    {
        // Ensure the inventory panel is initially hidden
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        // Initialize cursor state when the game starts (assuming game starts unpaused)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel != null)
            {
                // Use the UIPauseManager to toggle and pause/unpause the game
                UIPauseManager.Instance.TogglePanel(inventoryPanel);
                Debug.Log($"Inventory panel toggled. Current state: {inventoryPanel.activeSelf}");
            }
        }
    }
}