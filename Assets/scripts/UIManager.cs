using UnityEngine;
using TMPro;
using ArabicSupport;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mileCounterText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverPanel;

    private float miles;
    private float highScore;
    private bool isCounting = true;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
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

        if (mileCounterText != null)
            mileCounterText.gameObject.SetActive(true);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(true);
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

        UpdateMileText();
        UpdateHighScoreText();

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
            finalScoreText.text = ArabicFixer.Fix($"لقد ركضت: {miles:F1} ميل\nأفضل ركضة: {highScore:F1} ميل");
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (mileCounterText != null)
            mileCounterText.gameObject.SetActive(true);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(true);

        SaveHighScore();
    }

    private void HandleRestartInput()
    {
        ResetGame();
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }

    public void OnRestartButton()
    {
        HandleRestartInput();
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ResetGame()
    {
        miles = 0f;
        isCounting = true;
        UpdateMileText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (mileCounterText != null)
            mileCounterText.gameObject.SetActive(true);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(true);
    }

    private void UpdateMileText()
    {
        if (mileCounterText != null)
            mileCounterText.text = ArabicFixer.Fix($" ميل: {miles:F1}");
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
            highScoreText.text = ArabicFixer.Fix($"الأفضل: {highScore:F1}");
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
