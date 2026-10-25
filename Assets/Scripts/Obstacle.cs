using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 3f;
    public float xKillBoundary = -12f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < xKillBoundary)
            Destroy(gameObject);
    }
}