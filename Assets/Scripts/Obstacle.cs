using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 3f;
    public float xKillBoundary = -12f;

    private bool passed = false;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < xKillBoundary)
            Destroy(gameObject);
    }

    public void OnPass()
    {
        if (passed) return;
        passed = true;
        GameManager.Instance.AddScore(1);
    }
}