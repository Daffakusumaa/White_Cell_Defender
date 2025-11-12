using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class script : MonoBehaviour
{
    public enum SpawnState { Spawning, waiting, counting };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timebetweenWave = 5f;
    private float WaveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.counting;

    private void Start()
    {
        if(spawnPoints.Length ==0)
        {
            Debug.LogError("no spawn points referenced.");
        }
        WaveCountdown = timebetweenWave;
    }

    private void Update()
    {
        if (state == SpawnState.waiting)
        {
            if (!enemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }


        if(WaveCountdown <= 0)
        {
            if (state == SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            WaveCountdown -= Time.deltaTime; 
        }
    }

    bool enemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave:" + _wave.name);
        state = SpawnState.Spawning;
        
       for (int i = 0; i <_wave.count; i++)
        {
            Spawnenemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.counting;
        WaveCountdown = timebetweenWave;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("semua wave selesai! looping... ");
        }
        else
        {
            nextWave++;
        }

        
    }
    void Spawnenemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy:" + _enemy.name);

        

        Transform _sp = spawnPoints[Random.Range(0,spawnPoints.Length)];
        Instantiate(_enemy, transform.position, transform.rotation);
    }
        


}
