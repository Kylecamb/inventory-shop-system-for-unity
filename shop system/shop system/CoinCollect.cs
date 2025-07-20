using UnityEngine;
using UnityEngine.UI; // Although not strictly needed for this modified script, it's good practice to keep if you plan to display UI elements later.

public class CoinCollect : MonoBehaviour
{
    // Public variable to set the value of this specific coin
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the current number of coins
            int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);

            // Add the value of this coin to the total
            currentCoins += coinValue;

            // Save the updated coin count
            PlayerPrefs.SetInt("PlayerCoins", currentCoins);

            // Destroy the coin GameObject after collection
            Destroy(gameObject);
        }
    }
}