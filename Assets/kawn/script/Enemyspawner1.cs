using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemyPrefab;
        public int count;         
        public float spawnRate;     
    }

    public Wave[] waves;           
    private int currentWaveIndex = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private bool isSpawning = false;

    void Update()
    {
        if (isSpawning)
            return;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        Debug.Log("Mulai Wave: " + wave.name);

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        Debug.Log("Wave Selesai!");

        isSpawning = false;

       
        if (currentWaveIndex + 1 < waves.Length)
            currentWaveIndex++;
        else
            Debug.Log("Semua wave selesai!");
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
