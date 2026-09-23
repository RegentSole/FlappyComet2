using UnityEngine;
using TMPro;
using VContainer;
using VContainer.Unity;

public class GameManager : ITickable, IStartable
{
    private readonly TMP_Text scoreText;
    private readonly GameObject gameOverPanel;

    private int score = 0;
    private bool isGameOver = false;

    [Inject]
    public GameManager(TMP_Text scoreText, GameObject gameOverPanel)
    {
        this.scoreText = scoreText;
        this.gameOverPanel = gameOverPanel;
    }

    // Аналог Start()
    public void Start()
    {
        StartGame();
    }

    // Аналог Update()
    public void Tick()
    {
        if (scoreText != null && scoreText.text != score.ToString())
            scoreText.text = score.ToString();
    }

    public void StartGame()
    {
        score = 0;
        isGameOver = false;
        if (scoreText != null) scoreText.text = "0";
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        StartGame();
        if (ObstacleSpawner.Instance != null)
            ObstacleSpawner.Instance.ResetSpawner();

        // Возвращаем игрока в исходное состояние через событие
        PlayerController.ResetRequested?.Invoke();
    }

    public bool IsGameOver => isGameOver;
}