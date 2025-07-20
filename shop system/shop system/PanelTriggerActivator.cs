// PanelTriggerActivator.cs (Modified)
using UnityEngine;

public class PanelTriggerActivator : MonoBehaviour
{
    public GameObject panelToActivate; // Drag your UI Panel here in the Inspector

    // Removed OnTriggerExit logic

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && panelToActivate != null)
        {
            // Use the UIPauseManager to open and pause the game
            UIPauseManager.Instance.OpenPanel(panelToActivate);
            Debug.Log("Player entered trigger. Panel activated and game paused.");
        }
    }
}