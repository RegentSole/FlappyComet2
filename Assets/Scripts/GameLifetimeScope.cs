using UnityEngine;
using TMPro;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Scene Components")]
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PlayerController playerController;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;

    protected override void Configure(IContainerBuilder builder)
    {
        // Регистрируем UI-зависимости как готовые экземпляры
        builder.RegisterInstance(scoreText);
        builder.RegisterInstance(gameOverPanel);

        // GameManager — чистый C# класс, Singleton + EntryPoint (получает Tick)
        builder.Register<GameManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<GameManager>();

        // Компоненты сцены — регистрируем как MonoBehaviour
        builder.RegisterComponent(obstacleSpawner);
        builder.RegisterComponent(playerController);
    }
}