using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button continueButton;

    [Header("Reward")]
    public RewardCoinUI rewardCoin;

    [Header("Dialogue Texts")]
    [TextArea] public string firstText;
    [TextArea] public string secondText;
    [TextArea] public string thirdText;

    private int stage = 0;

    void Start()
    {
        Debug.Log("DialogueController START");
        stage = 0;

        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(NextDialogue);

        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1f);

        dialoguePanel.SetActive(true);
        dialogueText.text = firstText;

        yield return new WaitForSeconds(1.5f);
        continueButton.gameObject.SetActive(true);
    }

    void NextDialogue()
    {
        stage++;

        if (stage == 1)
        {
            dialogueText.text = secondText;
        }
        else if (stage == 2)
        {
            dialogueText.text = thirdText;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        Debug.Log("Диалог завершён");

        dialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);

        if (rewardCoin != null)
        {
            rewardCoin.Show();
        }
        else
        {
            Debug.LogError("RewardCoinUI не назначен в Inspector!");
        }
    }
}
