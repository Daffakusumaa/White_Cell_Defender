using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;
        public int count = 1;
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
    private bool isSpawning = false;
    private bool gameEnded = false;

    public Transform[] spawnPoints;

    public GameObject winPanel;
    public GameObject gameOverPanel;

    public PlayerHealth playerHealth;

    [Header("UI Wave")]
    public TMP_Text waveText;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (waveText != null)
            waveText.text = "Wave 1";

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

            
            if (waveText != null)
            {
                if (currentWaveIndex == waves.Length - 1)
                    waveText.text = "FINAL";
                else
                    waveText.text = $"Wave {currentWaveIndex + 1}";
            }

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
        if (winPanel != null) winPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        gameEnded = true;
        Time.timeScale = 0f;
    }
}
