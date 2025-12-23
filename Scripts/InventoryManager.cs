using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Храним предметы и их количество
    private Dictionary<string, int> items = new Dictionary<string, int>();

    [Header("UI")]
    public TMP_Text inventoryDebugText; // временно, для проверки

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 🔥 ДОБАВЛЕНИЕ ПРЕДМЕТА
    public void AddItem(string itemId, int amount)
    {
        if (items.ContainsKey(itemId))
            items[itemId] += amount;
        else
            items[itemId] = amount;

        Debug.Log($"В инвентарь добавлено: {itemId} x{amount}");

        UpdateUI();
    }

    // 🔄 Обновление интерфейса
    void UpdateUI()
    {
        if (inventoryDebugText == null) return;

        inventoryDebugText.text = "";
        foreach (var item in items)
        {
            inventoryDebugText.text += $"{item.Key} x{item.Value}\n";
        }
    }
}
