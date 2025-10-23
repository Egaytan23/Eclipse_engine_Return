using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefab; // Prefabs of the enemies to spawn (e.g. your 3 prefabs)
    public Transform[] spawnPoints; // Array of possible spawn points for enemies
    public float timeBetweenWaves = 5f; // Time between waves of enemies
    public int enemiesPerWave = 5; // Number of enemies to spawn per wave
    public float timeBetweenSpawns = 0.25f; // Small delay between individual spawns in a wave (optional)
    private int currentWave = 1; // Current wave number

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    
   
    IEnumerator SpawnWaves() //IEnumerator allows for pausing execution and resuming later, important for timed events like spawning enemies
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave(enemiesPerWave));
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }


        }

    IEnumerator SpawnWave(int enemyCount)
    {
        for(int i = 0; i < enemyCount; i++)
        {
            GameObject prefab = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            if(timeBetweenSpawns > 0f)
            
                yield return new WaitForSeconds(timeBetweenSpawns);
            else
        
            yield return null; //wait until next frame
        }
        }
    

    void Update()
    {
        
    }
}

