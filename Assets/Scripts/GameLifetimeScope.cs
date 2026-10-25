using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Scene Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private RestartButton restartButton;
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private GameOverUI gameOverUI;

    protected override void Configure(IContainerBuilder builder)
    {
        // ✅ GameManager — как Singleton-сервис
        builder.Register<GameManager>(Lifetime.Singleton);

        // ✅ GameBootstrap — entry point, зависит от GameManager
        builder.RegisterEntryPoint<GameBootstrap>();

        // ✅ Компоненты сцены — VContainer внедрит в них [Inject]
        builder.RegisterComponent(playerController);
        builder.RegisterComponent(obstacleSpawner);
        builder.RegisterComponent(restartButton);
        builder.RegisterComponent(scoreUI);
        builder.RegisterComponent(gameOverUI);
    }
}