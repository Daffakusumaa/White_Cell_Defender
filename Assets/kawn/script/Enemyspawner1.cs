using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;
        public int count;
        public float spawnRate = 1f;
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public EnemyGroup[] enemyGroups;
    }

    public Wave[] waves;
    private int currentWaveIndex = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown = 0f;

    private bool isSpawning = false;
    private bool gameEnded = false;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Player Reference")]
    public PlayerHealth playerHealth;  

    void Start()
    {
        waveCountdown = 2f;
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        
        if (gameEnded)
            return;

        
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            GameOver();
            return;
        }

        if (isSpawning)
            return;

        if (EnemyMasihHidup())
            return;

        if (waveCountdown <= 0f)
        {
            if (currentWaveIndex < waves.Length)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
                waveCountdown = timeBetweenWaves;
            }
            else
            {
                
                WinGame();
            }
        }

        waveCountdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        Debug.Log("Memulai Wave: " + wave.name);

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            StartCoroutine(SpawnEnemyGroup(group));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        isSpawning = false;
        currentWaveIndex++;
    }

    IEnumerator SpawnEnemyGroup(EnemyGroup group)
    {
        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab);
            yield return new WaitForSeconds(1f / group.spawnRate);
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    bool EnemyMasihHidup()
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length > 0;
    }

    void WinGame()
    {
        Debug.Log("Semua wave selesai .menang!");
        winPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }

    void GameOver()
    {
        Debug.Log("Player mati, Game Over!");
        gameOverPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f; 
    }
}
