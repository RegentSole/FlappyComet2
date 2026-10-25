using System;
using UnityEngine;

public class GameManager
{
    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<int> ScoreChanged;
    public event Action GameStarted;
    public event Action GameEnded;

    public void StartGame()
    {
        Score = 0;
        IsGameOver = false;
        IsRunning = true;
        Time.timeScale = 1f;

        Debug.Log("[GameManager] StartGame");
        ScoreChanged?.Invoke(Score);
        GameStarted?.Invoke();
    }

    public void AddScore(int amount)
    {
        if (!IsRunning || IsGameOver) return;

        Score += amount;
        Debug.Log($"[GameManager] Score = {Score}");
        ScoreChanged?.Invoke(Score);
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        IsRunning = false;
        Time.timeScale = 0f;

        Debug.Log("[GameManager] GameOver");
        GameEnded?.Invoke();
    }

    public void RestartGame()
    {
        if (!IsGameOver) return;
        Debug.Log("[GameManager] RestartGame");
        StartGame();
    }
}