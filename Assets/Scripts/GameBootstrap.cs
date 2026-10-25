using VContainer.Unity;

public class GameBootstrap : IStartable
{
    private readonly GameManager gameManager;

    public GameBootstrap(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Start()
    {
        gameManager.StartGame();
    }
}