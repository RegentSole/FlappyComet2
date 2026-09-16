using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed = 1f;
    private float spriteWidth;

    private void Start()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * parallaxSpeed * Time.deltaTime);

        // Зацикливание
        if (transform.position.x < -spriteWidth)
        {
            transform.position += new Vector3(spriteWidth * 2f, 0, 0);
        }
    }
}