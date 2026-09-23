using System.Collections;
using UnityEngine;
using VContainer;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject bonusPrefab;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 1.8f;
    [SerializeField] private float horizontalSpawnPos = 10f;

    [Header("Obstacle Vertical Spread")]
    [SerializeField] private float obstacleMinY = -3.5f;
    [SerializeField] private float obstacleMaxY = 3.5f;

    [Header("Bonus Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float bonusSpawnChance = 0.6f;
    [SerializeField] private float minVerticalGap = 2.5f;
    [SerializeField] private float bonusEdgePadding = 1f;

    [Header("Movement Speed")]
    [SerializeField] private float obstacleSpeed = 3f;
    [SerializeField] private float bonusSpeed = 3f;

    [Header("Cleanup")]
    [SerializeField] private float killMargin = 2f;

    [Inject] private GameManager gameManager;

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (!gameManager.IsGameOver)
                SpawnObstacleWithBonus();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObstacleWithBonus()
    {
        float obstacleY = Random.Range(obstacleMinY, obstacleMaxY);
        Vector3 obstaclePos = new Vector3(horizontalSpawnPos, obstacleY, 0);

        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
        Obstacle obsScript = obstacle.GetComponent<Obstacle>();
        if (obsScript != null)
        {
            obsScript.speed = obstacleSpeed;
            obsScript.xKillBoundary = GetLeftKillBoundary();
        }

        if (Random.value < bonusSpawnChance && bonusPrefab != null)
            SpawnBonus(obstacleY);
    }

    private void SpawnBonus(float obstacleY)
    {
        float bonusX = horizontalSpawnPos;
        float camHeight = Camera.main != null ? Camera.main.orthographicSize : 5f;

        float minY = -camHeight + bonusEdgePadding;
        float maxY = camHeight - bonusEdgePadding;

        float bonusY = 0f;
        bool found = false;

        for (int i = 0; i < 20; i++)
        {
            bonusY = Random.Range(minY, maxY);
            if (Mathf.Abs(bonusY - obstacleY) >= minVerticalGap)
            {
                found = true;
                break;
            }
        }

        if (!found)
            bonusY = obstacleY > 0 ? minY : maxY;

        Vector3 bonusPos = new Vector3(bonusX, bonusY, 0);
        GameObject bonus = Instantiate(bonusPrefab, bonusPos, Quaternion.identity);
        Bonus bonusScript = bonus.GetComponent<Bonus>();
        if (bonusScript != null)
        {
            bonusScript.speed = bonusSpeed;
            bonusScript.xKillBoundary = GetLeftKillBoundary();
        }
    }

    private float GetLeftKillBoundary()
    {
        if (Camera.main == null) return -12f;
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        return Camera.main.transform.position.x - halfWidth - killMargin;
    }

    public void ResetSpawner()
    {
        foreach (var obs in FindObjectsOfType<Obstacle>())
            Destroy(obs.gameObject);

        foreach (var b in FindObjectsOfType<Bonus>())
            Destroy(b.gameObject);

        StartSpawning();
    }

    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float h = cam.orthographicSize;
        float w = h * cam.aspect;
        Vector3 camPos = cam.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(camPos.x, camPos.y, 0), new Vector3(w * 2, h * 2, 0));

        Gizmos.color = Color.yellow;
        Vector3 spawnLine = new Vector3(horizontalSpawnPos, camPos.y, 0);
        Gizmos.DrawLine(spawnLine + Vector3.up * h, spawnLine + Vector3.down * h);
    }
}