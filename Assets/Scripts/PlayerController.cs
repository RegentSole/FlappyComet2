using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxUpwardVelocity = 4.5f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Control")]
    [SerializeField] private bool useMagnetControl = true;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("References")]
    [SerializeField] private TrailRenderer trail;

    [Inject] private GameManager gameManager;

    private Rigidbody2D rb;
    private bool isDead;
    private float camHeight;
    private readonly HashSet<Collider2D> counted = new HashSet<Collider2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        camHeight = Camera.main != null ? Camera.main.orthographicSize : 5f;

        // Гарантированно скрываем панель при старте
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isDead || gameManager.IsGameOver) return;

        // Управление
        if (useMagnetControl)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
                rb.velocity = new Vector2(rb.velocity.x, maxUpwardVelocity);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Смерть по границам экрана
        if (Mathf.Abs(transform.position.y) > camHeight + 0.5f)
        {
            Die();
            return;
        }

        // Вращение
        float angle = Mathf.Clamp(rb.velocity.y * rotationSpeed, -30f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PassTrigger"))
        {
            if (counted.Contains(other)) return;
            counted.Add(other);
            gameManager.AddScore(1);
        }
        else if (other.CompareTag("Bonus"))
        {
            Bonus bonus = other.GetComponent<Bonus>();
            if (bonus != null)
            {
                bonus.Collect();
                gameManager.AddScore(5);
            }
        }
        else if (other.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        gameManager.GameOver();

        // ✅ Панель появляется здесь — прямо в одном скрипте
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogWarning("[PlayerController] gameOverPanel не назначен в инспекторе!");
    }

    public void ResetPlayer()
    {
        isDead = false;
        counted.Clear();
        rb.velocity = Vector2.zero;
        rb.gravityScale = gravityScale;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        if (trail != null) trail.Clear();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}