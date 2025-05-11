using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private TextMeshProUGUI mileCounterText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverPanel;

    private float miles;
    private float highScore;
    private bool isCounting = true;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            enabled = false;
            return;
        }

        GameManager.Instance.RegisterUIManager(this);
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        miles = 0f;

        UpdateMileText();
        UpdateHighScoreText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        var game = GameManager.Instance;
        if (game == null) return;

        if (isCounting && !game.isGameOver)
        {
            miles += Time.deltaTime * game.GameSpeed;
            UpdateMileText();

            if (miles > highScore)
            {
                highScore = miles;
                UpdateHighScoreText();
            }
        }

        if (game.isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            HandleRestartInput();
        }
    }

    public void ShowGameOver()
    {
        isCounting = false;

        if (finalScoreText != null)
        {
            finalScoreText.text = $"You Ran: {miles:F1} Miles\nBest Run: {highScore:F1} Miles";
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        SaveHighScore();
    }

    private void HandleRestartInput()
    {
        Debug.Log("Restart initiated via keyboard");

        ResetGame();

        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }

    public void ResetGame()
    {
        miles = 0f;
        isCounting = true;
        UpdateMileText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void UpdateMileText()
    {
        if (mileCounterText != null)
            mileCounterText.text = $"{miles:F1} Miles";
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
            highScoreText.text = $"Best: {highScore:F1}";
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
