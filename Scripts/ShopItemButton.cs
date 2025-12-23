using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    public int price;
    public string itemId;

    public Button buyButton;
    public TMP_Text priceText;

    void Start()
    {
        priceText.text = price.ToString();
        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        // 🔥 ПЫТАЕМСЯ СПИСАТЬ МОНЕТЫ
        if (CurrencyManager.Instance.SpendCoins(price))
        {
            InventoryManager.Instance.AddItem(itemId, 1);
            Debug.Log("Куплено: " + itemId);
        }
        else
        {
            Debug.Log("Недостаточно монет!");
        }
    }
}
