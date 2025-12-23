using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCoinUI : MonoBehaviour
{
    public int rewardAmount = 10;
    public Button collectButton;

    void Start()
    {
        gameObject.SetActive(false); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Collect()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance == null");
            return;
        }

        CurrencyManager.Instance.AddCoins(rewardAmount);
        gameObject.SetActive(false);
    }
}
