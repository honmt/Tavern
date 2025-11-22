using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // КНОПКА ENTER - запуск игры
    public void StartGame()
    {
        // Загружаем сцену приветствия
        SceneManager.LoadScene("IntroductionScene");
    }

    // КНОПКА EXIT - выход из игры
    public void ExitGame()
    {
        Debug.Log("Выход из игры!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // КНОПКА SETTINGS - открытие настроек
    public void OpenSettings()
    {
        Debug.Log("Открытие настроек!");
        // Здесь будет открытие панели настроек
    }

}