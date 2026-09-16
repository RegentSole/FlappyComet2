using UnityEngine;

public class Bonus : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float xKillBoundary = -12f;

    [Header("Visual")]
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private GameObject collectEffectPrefab;

    [Header("Debug")]
    [SerializeField] private bool showDebug = false;

    private float camHeight;

    private void Start()
    {
        camHeight = Camera.main != null ? Camera.main.orthographicSize : 5f;

        if (showDebug)
            Debug.Log($"[Bonus] Start. speed = {speed}, pos = {transform.position}");
    }

    private void Update()
    {
        // Движение влево по X — на игрока
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Вращение для красоты
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Удаление за границами
        if (transform.position.x < xKillBoundary ||
            Mathf.Abs(transform.position.y) > camHeight + 1.5f)
        {
            if (showDebug)
                Debug.Log($"[Bonus] Удалён на pos = {transform.position}");
            Destroy(gameObject);
        }
    }

    public void Collect()
    {
        if (collectEffectPrefab != null)
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}