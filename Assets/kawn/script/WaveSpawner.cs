using System.Collections;
using UnityEngine;

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

    [Header("Wave Settings")]
    public Wave[] waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool gameEnded = false;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Player Reference")]
    public PlayerHealth playerHealth;

    void Start()
    {
        
        //if (winPanel != null) winPanel.SetActive(false);
        //if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Mulai wave pertama
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        
        if (gameEnded)
            return;

        //Kalau player mati → Game Over
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            GameOver();
            return;
        }

        
        if (isSpawning)
            return;

        
        if (!EnemyMasihHidup())
        {
            
            if (currentWaveIndex >= waves.Length)
            {
                WinGame();
                return;
            }

            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;

        if (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            Debug.Log($"🚨 Memulai Wave {currentWaveIndex + 1}: {wave.name}");

            foreach (EnemyGroup group in wave.enemyGroups)
            {
                StartCoroutine(SpawnEnemyGroup(group));
                yield return new WaitForSeconds(0.5f);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(1f); 
        }

        isSpawning = false;
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
        Debug.Log(" Semua wave selesai — kamu MENANG!");
        if (winPanel != null) winPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        Debug.Log(" Player mati — GAME OVER!");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }
}
