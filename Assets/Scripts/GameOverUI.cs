using UnityEngine;
using VContainer;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Inject] private GameManager gameManager;

    private void Start()
    {
        if (panel == null) panel = gameObject;
        Debug.Log($"[GameOverUI] Start. panel={panel.name}, gameManager={(gameManager != null ? "OK" : "NULL")}");

        gameManager.GameStarted += OnGameStarted;
        gameManager.GameEnded   += OnGameEnded;

        OnGameStarted();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.GameStarted -= OnGameStarted;
            gameManager.GameEnded   -= OnGameEnded;
        }
    }

    private void OnGameStarted()
    {
        Debug.Log("[GameOverUI] Hide");
        panel.SetActive(false);
    }

    private void OnGameEnded()
    {
        Debug.Log("[GameOverUI] Show");
        panel.SetActive(true);
    }
}