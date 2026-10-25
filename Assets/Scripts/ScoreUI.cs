using UnityEngine;
using TMPro;
using VContainer;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [Inject] private GameManager gameManager;

    private void Start()
    {
        if (scoreText == null) scoreText = GetComponent<TMP_Text>();

        gameManager.ScoreChanged += OnScoreChanged;
        OnScoreChanged(gameManager.Score);   // синхронизация при загрузке

        Debug.Log($"[ScoreUI] Подписан, scoreText={scoreText.name}");
    }

    private void OnDestroy()
    {
        if (gameManager != null)
            gameManager.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
            Debug.Log($"[ScoreUI] UI = {score}");
        }
    }
}