using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForestGameManager : MonoBehaviour
{
    public enum GameState
    {
        Instructions,
        Playing,
        GameOver
    }

    public static ForestGameManager Instance;

    [Header("UI Elements")]
    public GameObject gameUI;
    public TextMeshProUGUI instructionsText;
    public GameObject resultPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")]
    public float gameTime = 300f;
    public float instructionsTime = 3f;

    [Header("Plant Spawning")]
    public GameObject[] plantPrefabs;
    public Transform herbsParent;
    public float spawnInterval = 1.5f;
    public int maxPlantsOnScreen = 10;
    public Vector2 spawnAreaMin = new Vector2(1163, 538); // ИЗМЕНИЛ ОБЛАСТЬ СПАВНА
    public Vector2 spawnAreaMax = new Vector2(1173, 548); // ИЗМЕНИЛ ОБЛАСТЬ СПАВНА
    public float plantLifetime = 1f;

    private GameState currentState;
    private float timeLeft;
    private int herbsCollected = 0;
    private List<GameObject> currentPlants = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private int currentPlantIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        FindUIElements();
        CheckCameraSettings();
        StartGameSequence();
    }

    void CheckCameraSettings()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log($"Камера: Pos={mainCamera.transform.position}, Size={mainCamera.orthographicSize}, Near={mainCamera.nearClipPlane}, Far={mainCamera.farClipPlane}");
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void FindUIElements()
    {
        gameUI = GameObject.Find("GameUI");
        instructionsText = GameObject.Find("InstructionsText")?.GetComponent<TextMeshProUGUI>();
        resultPanel = GameObject.Find("ResultPanel");

        if (gameUI != null)
        {
            Transform timerTransform = gameUI.transform.Find("TimerText");
            if (timerTransform != null)
                timerText = timerTransform.GetComponent<TextMeshProUGUI>();

            Transform scoreTransform = gameUI.transform.Find("ScoreText");
            if (scoreTransform != null)
                scoreText = scoreTransform.GetComponent<TextMeshProUGUI>();
        }

        if (resultPanel != null)
        {
            resultText = resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void StartGameSequence()
    {
        herbsCollected = 0;
        currentPlantIndex = 0;
        SetState(GameState.Instructions);

        if (instructionsText != null)
            instructionsText.text = "Собери как можно больше трав!\nНажимай на них быстро!";

        Invoke("StartGame", instructionsTime);
    }

    void SetState(GameState newState)
    {
        currentState = newState;

        if (instructionsText != null)
            instructionsText.gameObject.SetActive(currentState == GameState.Instructions);
        if (gameUI != null)
            gameUI.SetActive(currentState == GameState.Playing);
        if (resultPanel != null)
            resultPanel.SetActive(currentState == GameState.GameOver);

        switch (currentState)
        {
            case GameState.Instructions:
                break;

            case GameState.Playing:
                timeLeft = gameTime;
                UpdateUI();
                StartSpawning();
                break;

            case GameState.GameOver:
                StopSpawning();
                ShowResults();
                break;
        }
    }

    void StartGame()
    {
        SetState(GameState.Playing);
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            timeLeft -= Time.deltaTime;
            UpdateUI();

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                SetState(GameState.GameOver);
            }
        }
    }

    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Время: {(int)timeLeft}";
        if (scoreText != null)
            scoreText.text = $"Трав: {herbsCollected}";
    }

    void ShowResults()
    {
        if (resultText != null)
            resultText.text = $"Игра окончена!\nСобрано трав: {herbsCollected}";
    }

    void StartSpawning()
    {
        if (plantPrefabs.Length > 0)
        {
            spawnCoroutine = StartCoroutine(SpawnPlants());
        }
        else
        {
            Debug.LogError("Нет префабов растений для спавна!");
        }
    }

    void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        foreach (GameObject plant in currentPlants)
        {
            if (plant != null)
                Destroy(plant);
        }
        currentPlants.Clear();
    }

    IEnumerator SpawnPlants()
    {
        while (currentState == GameState.Playing)
        {
            if (currentPlants.Count < maxPlantsOnScreen && plantPrefabs.Length > 0)
            {
                SpawnNextPlant();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnNextPlant()
    {
        if (plantPrefabs.Length == 0)
        {
            Debug.LogError("Массив plantPrefabs пустой!");
            return;
        }

        GameObject plantToSpawn = plantPrefabs[currentPlantIndex];
        currentPlantIndex = (currentPlantIndex + 1) % plantPrefabs.Length;

        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0f
        );

        GameObject newPlant = Instantiate(plantToSpawn, spawnPosition, Quaternion.identity, herbsParent);

        newPlant.transform.localScale = Vector3.one;

        SpriteRenderer renderer = newPlant.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError($"У растения {plantToSpawn.name} нет SpriteRenderer!");
        }
        else if (renderer.sprite == null)
        {
            Debug.LogError($"У растения {plantToSpawn.name} не назначен спрайт!");
        }
        else
        {
            Debug.Log($"Растение {plantToSpawn.name} создано. Спрайт: {renderer.sprite.name}, Масштаб: {newPlant.transform.localScale}, Позиция: {spawnPosition}");

            renderer.sortingOrder = 10;
            if (renderer.color.a < 1f)
            {
                renderer.color = Color.white;
            }
        }

        currentPlants.Add(newPlant);

        PlantClickHandler clickHandler = newPlant.GetComponent<PlantClickHandler>();
        if (clickHandler == null)
        {
            clickHandler = newPlant.AddComponent<PlantClickHandler>();
        }

        clickHandler.SetLifetime(plantLifetime);

        Collider2D collider = newPlant.GetComponent<Collider2D>();
        if (collider == null)
        {
            newPlant.AddComponent<BoxCollider2D>();
        }
    }

    public void CollectPlant(GameObject plant)
    {
        if (currentState != GameState.Playing) return;

        herbsCollected++;
        UpdateUI();

        if (currentPlants.Contains(plant))
        {
            currentPlants.Remove(plant);
            Destroy(plant);
        }
    }

    public void AddScore(int amount)
    {
        herbsCollected += amount;
        UpdateUI();
    }

    public void RemovePlant(GameObject plant)
    {
        if (currentPlants.Contains(plant))
        {
            currentPlants.Remove(plant);
        }
    }

    public void RestartGame()
    {
        CancelInvoke();
        StartGameSequence();
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}