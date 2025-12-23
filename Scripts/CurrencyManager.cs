using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public TMP_Text coinText;
    private int coins = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins < amount)
            return false;

        coins -= amount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }

    public int GetCoins()
    {
        return coins;
    }
}
