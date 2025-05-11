using UnityEngine;
using UnityEngine.SceneManagement; 


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float gameSpeed = 5f;
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    [Header("Game State")]
    public bool isGameOver { get; private set; }

    private ThornSpawner thornSpawner;
    private UIManager uiManager;

    public float GameSpeed => gameSpeed;


    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager initialized");
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(restartKey))
        {
            Debug.Log($"Restart key '{restartKey}' pressed.");
            RestartGame();
        }
    }

    public void RegisterThornSpawner(ThornSpawner spawner)
    {
        thornSpawner = spawner;
        Debug.Log("ThornSpawner registered.");
    }

    public void RegisterUIManager(UIManager ui)
    {
        uiManager = ui;
        Debug.Log("UIManager registered.");
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("--- GAME OVER ---");

        uiManager?.ShowGameOver();
    }

    public void RestartGame()
    {
        if (!isGameOver) return;

        Debug.Log("=== GAME RESTART ===");
        isGameOver = false;

        thornSpawner?.HandleGameRestart();
        uiManager?.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
