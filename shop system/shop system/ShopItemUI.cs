using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image itemIcon;
    public Text itemNameText;
    public Text priceText;
    public Button buyButton;
    
    private ShopItemSO itemData;
    private ShopManager shopManager;

    public void Setup(ShopItemSO item, ShopManager manager)
    {
        itemData = item;
        shopManager = manager;
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        priceText.text = item.price.ToString();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        shopManager.AttemptPurchase(itemData);
    }
}