using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        [Header("Prefab musuh yang akan muncul di wave ini")]
        public GameObject enemyPrefab;
        [Header("Jumlah musuh dari prefab ini")]
        public int count = 1;
        [Header("Kecepatan spawn")]
        public float spawnRate = 1f;
    }

    [System.Serializable]
    public class Wave
    {
        [Header("Nama wave")]
        public string name;
        [Header("Grup musuh di wave ini")]
        public EnemyGroup[] enemyGroups;
    }

    [Header("Wave yang Akan Dimainkan")]
    public Wave[] waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool gameEnded = false;

    [Header("spawn musuh")]
    public Transform[] spawnPoints;

    [Header("Panel UI")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Referensi Player")]
    public PlayerHealth playerHealth;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        
        StartCoroutine(StartNextWave());
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
            Debug.Log($"Memulai Wave {currentWaveIndex + 1}: {wave.name}");

            
            foreach (EnemyGroup group in wave.enemyGroups)
            {
                yield return StartCoroutine(SpawnEnemyGroup(group));
            }

            currentWaveIndex++;
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
        Debug.Log("Semua wave selesai — kamu MENANG!");
        if (winPanel != null) winPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        Debug.Log("Player mati — GAME OVER!");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }
}
