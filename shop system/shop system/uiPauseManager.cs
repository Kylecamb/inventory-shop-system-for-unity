// UIPauseManager.cs
using UnityEngine;

public class UIPauseManager : MonoBehaviour
{
    public static UIPauseManager Instance { get; private set; } // Singleton

    private bool isGamePaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Useful if you want this manager to persist
        }
    }

    // Call this to open any UI panel that acts as a pause menu
    public void OpenPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            PauseGame();
        }
    }

    // Call this to close any UI panel that acts as a pause menu
    public void ClosePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
            UnpauseGame();
        }
    }

    // Toggles the state (useful for inventory 'I' key)
    public void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            if (panel.activeSelf)
            {
                ClosePanel(panel);
            }
            else
            {
                OpenPanel(panel);
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Stops all time-based operations
        Cursor.visible = true; // Show mouse cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        isGamePaused = true;
        Debug.Log("Game Paused.");
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f; // Resume normal time
        Cursor.visible = false; // Hide mouse cursor (or true if you always want it visible in game)
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center (or None if you always want it free)
        isGamePaused = false;
        Debug.Log("Game Unpaused.");
    }

    // You might want a public getter if other scripts need to know if the game is paused
    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}