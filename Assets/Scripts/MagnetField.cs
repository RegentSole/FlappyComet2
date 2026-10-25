using UnityEngine;
using VContainer;

public class MagnetField : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private float pullSpeed = 8f;
    [SerializeField] private LayerMask bonusLayer;

    [Inject] private GameManager gameManager;

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, radius, bonusLayer);

        foreach (var col in hits)
        {
            if (col.TryGetComponent<Bonus>(out var bonus))
                bonus.AttractTo(transform.position, pullSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}