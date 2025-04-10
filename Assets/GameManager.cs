using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Global game speed, can be modified at runtime
    public float gameSpeed = 1f;

    // Optional: Game state flags
    public bool isGameOver = false;

    void Awake()
    {
        // Set up singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it alive across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Optional: You can add functions to control game flow
    public void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        Debug.Log("Game Restarted");
    }
}
