using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public Text coinText;

    void Start()
    {
        UpdateCoinDisplay();
    }

    public void UpdateCoinDisplay()
    {
        coinText.text = PlayerPrefs.GetInt("PlayerCoins", 0).ToString();
    }
}