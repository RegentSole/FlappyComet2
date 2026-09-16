using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody2D rb;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        // Прыжок по нажатию (пробел или левая кнопка мыши)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // Вращение кометы в зависимости от вертикальной скорости
        float angle = Mathf.Clamp(rb.linearVelocity.y * rotationSpeed, -30f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) // если триггер прохода
        {
            // Проверим, что это зона прохода (можно добавить отдельный тег)
            if (other.name == "PassTrigger")
                GameManager.Instance.AddScore(1);
        }
        else if (other.CompareTag("Bonus"))
        {
            Bonus bonus = other.GetComponent<Bonus>();
            if (bonus != null)
            {
                bonus.Collect();
                GameManager.Instance.AddScore(5); // бонус даёт 5 очков
            }
        }
        else if (other.CompareTag("DeathZone")) // границы
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        GameManager.Instance.GameOver();
    }

    public void ResetPlayer()
    {
        isDead = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.gravityScale = 2f; // если меняли
    }
}