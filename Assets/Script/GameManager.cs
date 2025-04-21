using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Time Setting")]
    public float totalTime = 100f;
    private float timeRemaining;

    [Header("Target Prefab")]
    public GameObject staticTargetPrefab;
    public GameObject movingTargetPrefab;
    public GameObject irregularTargetPrefab;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("Spawn Settings")]
    public float minDistance = 5f;
    public float maxDistance = 100f;
    private float spawnTimer = 0f;

    private int score = 0;
    public int totalShots = 0;
    public int hits = 0;

    public static int FinalScore;
    public static int FinalShots;
    public static int FinalHits;

    void Start()
    {
        timeRemaining = totalTime;
        score = hits = totalShots = 0;
        scoreText.text = "Score: 0";
        timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";
    }

    void Update()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0f) timeRemaining = 0f;
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1f)
            {
                spawnTimer -= 1f;
                int spawnCount = Random.Range(1, 4);
                for (int i = 0; i < spawnCount; i++)
                    SpawnTarget();
            }

            if (timeRemaining == 0f)
                EndGame();
        }
    }

    void SpawnTarget()
    {
        float z = Random.Range(minDistance, maxDistance);
        Vector3 vp = new Vector3(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            z
        );
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(vp);

        int type = Random.Range(0, 3);
        GameObject prefab = type switch
        {
            1 => movingTargetPrefab,
            2 => irregularTargetPrefab,
            _ => staticTargetPrefab
        };

        Instantiate(prefab, worldPos, Quaternion.identity);
    }

    public void AddScore(int amount)
    {
        score += amount;
        hits += 1;
        scoreText.text = $"Score: {score}";
    }

    void EndGame()
    {
        enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FinalScore = score;
        FinalShots = totalShots;
        FinalHits = hits;

        SceneManager.LoadScene("EndScene");
    }
}
