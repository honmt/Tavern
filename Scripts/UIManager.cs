using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject inventoryPanel;

    void Start()
    {
        shopPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void OpenInventory()
    {
        Debug.Log("OpenInventory вызван");

        inventoryPanel.SetActive(true);
        shopPanel.SetActive(false);
    }


    public void CloseAll()
    {
        shopPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }
}
