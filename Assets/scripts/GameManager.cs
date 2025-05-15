using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float gameSpeed = 0.8f;
    [SerializeField] private float maxGameSpeed = 3.0f;
    [SerializeField] private float speedIncreaseAmount = 0.2f;
    [SerializeField] private float difficultyIncreaseInterval = 10f;
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    [Header("Game State")]
    public bool isGameOver { get; private set; }

    private Player player;
    private ThornSpawner thornSpawner;
    private UIManager uiManager;

    private float difficultyTimer = 0f;

    public float GameSpeed => gameSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindFirstObjectByType<Player>();
        thornSpawner = FindFirstObjectByType<ThornSpawner>();
        uiManager = FindFirstObjectByType<UIManager>();


        if (scene.name == "GameScene")
        {
            isGameOver = false;
            Time.timeScale = 1f;
            player?.ResetToInitialPosition();
            thornSpawner?.HandleGameRestart();
            uiManager?.ResetGame();

            gameSpeed = 0.8f;
            difficultyTimer = 0f;
        }
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(restartKey))
        {
            RestartGame();
        }

        if (!isGameOver)
        {
            difficultyTimer += Time.deltaTime;
            if (difficultyTimer >= difficultyIncreaseInterval)
            {
                difficultyTimer = 0f;
                IncreaseDifficulty();
            }
        }
    }

    private void IncreaseDifficulty()
    {
        if (gameSpeed < maxGameSpeed)
        {
            gameSpeed += speedIncreaseAmount;
            if (gameSpeed > maxGameSpeed)
                gameSpeed = maxGameSpeed;

            if (thornSpawner != null)
                thornSpawner.DecreaseSpawnInterval(speedIncreaseAmount * 0.5f);
        }
    }

    public void RegisterThornSpawner(ThornSpawner spawner)
    {
        thornSpawner = spawner;
    }

    public void RegisterUIManager(UIManager ui)
    {
        uiManager = ui;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        uiManager?.ShowGameOver();
        StartCoroutine(DelayBeforePause());
    }

    private IEnumerator DelayBeforePause()
    {
        yield return new WaitForSeconds(5f);
    }

    public void RestartGame()
    {
        if (!isGameOver) return;
        isGameOver = false;

        player?.ResetToInitialPosition();
        thornSpawner?.HandleGameRestart();
        uiManager?.ResetGame();

        gameSpeed = 0.8f;
        difficultyTimer = 0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
