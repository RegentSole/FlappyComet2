using UnityEngine;

public class Bonus : MonoBehaviour
{
    public float speed = 3f;
    public float xKillBoundary = -12f;

    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private GameObject collectEffectPrefab;

    private float camHeight;
    private bool isAttracted = false;   // для магнита

    private void Start()
    {
        camHeight = Camera.main != null ? Camera.main.orthographicSize : 5f;
    }

    private void Update()
    {
        if (!isAttracted)
            transform.position += Vector3.left * speed * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (transform.position.x < xKillBoundary ||
            Mathf.Abs(transform.position.y) > camHeight + 1.5f)
            Destroy(gameObject);
    }

    public void AttractTo(Vector3 target, float pullSpeed)
    {
        isAttracted = true;
        transform.position = Vector3.MoveTowards(
            transform.position, target, pullSpeed * Time.deltaTime);
    }

    public void Collect()
    {
        if (collectEffectPrefab != null)
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}