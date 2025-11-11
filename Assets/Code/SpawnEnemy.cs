using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Prefabs of the enemies to spawn (e.g. your 3 prefabs)
    public Transform[] spawnPoints; // Array of possible spawn points for enemies
    public float timeBetweenWaves = 5f; // Time between waves of enemies
    public int enemiesPerWave = 5; // Number of enemies to spawn per wave
    public float timeBetweenSpawns = 0.25f; // Small delay between individual spawns in a wave (optional)
    private int currentWave = 1; // Current wave number
    private int enemimesAlive = 0; // Track number of alive enemies

    private Transform player;

    void Start()
    {
        Debug.Log("Enemy prefabs count: " + enemyPrefabs.Length + ", spawn points count: " + spawnPoints.Length);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Spawner script started!");
        StartCoroutine(SpawnWaves());
    }


    IEnumerator SpawnWave(int enemyCount)
    {
        Debug.Log("Spawning wave " + currentWave + " with " + enemyCount + " enemies.");

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
  
            GameObject enemy = Instantiate(prefab, spawnPoint.position + Vector3.up * 1.2f,spawnPoint.rotation);

            enemimesAlive++;

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        }


    IEnumerator SpawnWaves() //IEnumerator allows for pausing execution and resuming later, important for timed events like spawning enemies
    {
       
        while (true)
        {
            Debug.Log("Preparing to spawn wave " + currentWave);
            
            int toSpawn = enemiesPerWave + (currentWave - 1) * 2; // Increase enemies per wave

            Debug.Log("Wave " + currentWave + " will spawn " + timeBetweenWaves + " seconds");
            yield return new WaitForSeconds(timeBetweenWaves);

            Debug.Log("Spawning wave " + currentWave);

            yield return StartCoroutine(SpawnWave(toSpawn));
            
            while (enemimesAlive > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            Debug.Log("Wave " + currentWave + " completed!");
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }


    public void EnemyKilled()
    {
        enemimesAlive = Mathf.Max(0, enemimesAlive - 1);
    }

    void Update()
    {
        
    }
}

