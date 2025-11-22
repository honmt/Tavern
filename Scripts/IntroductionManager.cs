using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroductionManager : MonoBehaviour
{
    [Header("UI элементы")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    public Image characterImage; // Изображение Торина
    public Image backgroundImage; // Фон сцены

    [Header("Диалоги")]
    public string[] dialogues;
    public string characterName = "ТОРИН";

    [Header("Настройки анимации")]
    public float characterFadeTime = 3.0f;
    public float backgroundFadeTime = 1.5f;
    public float panelFadeTime = 1.0f;
    public float textFadeTime = 0.5f;
    public float delayAfterCharacter = 1.0f;

    private int currentDialogueIndex = 0;
    private bool isAnimationPlaying = true;

    void Start()
    {
        // Сначала скрываем все элементы
        SetInitialTransparency();

        // Запускаем анимацию появления
        StartCoroutine(StartSceneAnimation());

        // Привязываем кнопку к методу NextDialogue
        continueButton.onClick.AddListener(NextDialogue);
    }

    void SetInitialTransparency()
    {
        // Фон - полностью прозрачный
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            bgColor.a = 0;
            backgroundImage.color = bgColor;
        }

        // Персонаж - полностью прозрачный
        if (characterImage != null)
        {
            Color charColor = characterImage.color;
            charColor.a = 0;
            characterImage.color = charColor;
        }

        // Панель диалога - полностью прозрачная
        if (dialoguePanel != null)
        {
            CanvasGroup panelGroup = dialoguePanel.GetComponent<CanvasGroup>();
            if (panelGroup == null) panelGroup = dialoguePanel.AddComponent<CanvasGroup>();
            panelGroup.alpha = 0;
        }

        // Тексты - полностью прозрачные
        if (nameText != null)
        {
            Color nameColor = nameText.color;
            nameColor.a = 0;
            nameText.color = nameColor;
        }

        if (dialogueText != null)
        {
            Color dialogueColor = dialogueText.color;
            dialogueColor.a = 0;
            dialogueText.color = dialogueColor;
        }

        // Кнопка - неактивна и прозрачна
        if (continueButton != null)
        {
            continueButton.interactable = false;
            CanvasGroup buttonGroup = continueButton.GetComponent<CanvasGroup>();
            if (buttonGroup == null) buttonGroup = continueButton.gameObject.AddComponent<CanvasGroup>();
            buttonGroup.alpha = 0;
        }
    }

    IEnumerator StartSceneAnimation()
    {
        // 1. Сначала ТОЛЬКО Торин появляется медленно
        if (characterImage != null)
        {
            yield return StartCoroutine(FadeInImage(characterImage, characterFadeTime));
        }

        // 2. Пауза после появления Торина
        yield return new WaitForSeconds(delayAfterCharacter);

        // 3. Появление фона
        if (backgroundImage != null)
        {
            yield return StartCoroutine(FadeInImage(backgroundImage, backgroundFadeTime));
        }

        // 4. Появление диалоговой панели
        if (dialoguePanel != null)
        {
            yield return StartCoroutine(FadeInCanvasGroup(dialoguePanel, panelFadeTime));
        }

        // 5. Появление текста имени
        if (nameText != null)
        {
            yield return StartCoroutine(FadeInText(nameText, textFadeTime));
        }

        // 6. Появление текста диалога
        if (dialogueText != null)
        {
            yield return StartCoroutine(FadeInText(dialogueText, textFadeTime));
        }

        // 7. Появление кнопки
        if (continueButton != null)
        {
            yield return StartCoroutine(FadeInCanvasGroup(continueButton.gameObject, textFadeTime));
            continueButton.interactable = true;
        }

        // Запускаем диалоги
        if (dialogues != null && dialogues.Length > 0)
        {
            ShowCurrentDialogue();
        }

        isAnimationPlaying = false;
    }

    IEnumerator FadeInImage(Image image, float fadeTime)
    {
        float elapsedTime = 0;
        Color color = image.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = color;
            yield return null;
        }
    }

    IEnumerator FadeInCanvasGroup(GameObject obj, float fadeTime)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        if (group == null) yield break;

        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            yield return null;
        }

        group.alpha = 1;
    }

    IEnumerator FadeInText(TextMeshProUGUI text, float fadeTime)
    {
        float elapsedTime = 0;
        Color color = text.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeTime);
            text.color = color;
            yield return null;
        }
    }

    public void NextDialogue()
    {
        if (isAnimationPlaying) return;

        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            ShowCurrentDialogue();
        }
        else
        {
            // Загружаем следующую сцену
            SceneManager.LoadScene("TutorialScene");
        }
    }

    void ShowCurrentDialogue()
    {
        if (currentDialogueIndex < 0 || currentDialogueIndex >= dialogues.Length)
        {
            Debug.LogError("Индекс диалога вне диапазона: " + currentDialogueIndex);
            return;
        }

        nameText.text = characterName;
        dialogueText.text = dialogues[currentDialogueIndex];
    }
}