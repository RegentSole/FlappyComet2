using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [Inject] private GameManager gameManager;
    [SerializeField] private PlayerController player;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            gameManager.StartGame();
            player.ResetPlayer();
        });
    }
}