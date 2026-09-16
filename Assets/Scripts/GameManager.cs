using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel; // панель с кнопкой рестарта

    private int score = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        isGameOver = false;
        scoreText.text = "0";
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        player.ResetPlayer();
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        // Здесь можно сохранить рекорд (PlayerPrefs)
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        scoreText.text = score.ToString();
    }

    // Вызывается кнопкой рестарта
    public void RestartGame()
    {
        // Перезагружаем сцену или просто сбрасываем состояние
        // Проще: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Либо вызываем StartGame() и сбрасываем все объекты вручную.
        StartGame();
        // Обнуляем препятствия (можно через событие)
        ObstacleSpawner.Instance.ResetSpawner();
    }
}