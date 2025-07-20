using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<ShopItemSO> shopItems;
    public GameObject shopItemUIPrefab;
    public Transform shopItemContainer;
    public CoinManager coinManager;

    void Start()
    {
        PopulateShop();
    }

    void PopulateShop()
    {
        foreach (ShopItemSO item in shopItems)
        {
            GameObject itemUI = Instantiate(shopItemUIPrefab, shopItemContainer);
            ShopItemUI shopItemUI = itemUI.GetComponent<ShopItemUI>();
            shopItemUI.Setup(item, this);
        }
    }

    public void AttemptPurchase(ShopItemSO item)
    {
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        if (currentCoins >= item.price)
        {
            currentCoins -= item.price;
            PlayerPrefs.SetInt("PlayerCoins", currentCoins);
            InventoryManager.Instance.AddItem(item);
            coinManager.UpdateCoinDisplay();
        }
    }
}