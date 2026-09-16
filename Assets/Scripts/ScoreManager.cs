using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int currentScore = 0;

    public void AddScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "0";
    }
}