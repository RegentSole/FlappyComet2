using System;
using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    public static event Action ResetRequested;

    [Header("Physics")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("References")]
    [SerializeField] private TrailRenderer trail; // опционально

    [Inject] private GameManager gameManager;

    private Rigidbody2D rb;
    private bool isDead = false;
    private float camHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        ResetRequested += ResetPlayer;
    }

    private void OnDisable()
    {
        ResetRequested -= ResetPlayer;
    }

    private void Start()
    {
        camHeight = Camera.main != null ? Camera.main.orthographicSize : 5f;
        ResetPlayer();
    }

    private void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            Jump();

        if (Mathf.Abs(transform.position.y) > camHeight + 0.5f)
            Die();

        float angle = Mathf.Clamp(rb.velocity.y * rotationSpeed, -30f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        gameManager.GameOver();
    }

    public void ResetPlayer()
    {
        isDead = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 2f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        if (trail != null) trail.Clear();
    }
}