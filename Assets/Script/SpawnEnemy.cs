using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;// For UI text updates

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Prefabs of the enemies to spawn (e.g. your 3 prefabs)
    public Transform[] spawnPoints; // Array of possible spawn points for enemies
    public float timeBetweenWaves = 5f; // Time between waves of enemies
    public int enemiesPerWave = 5; // Number of enemies to spawn per wave
    public float timeBetweenSpawns = 0.25f; // Small delay between individual spawns in a wave (optional)
    public int currentWave = 1; // Current wave number
    private int enemimesAlive = 0; // Track number of alive enemies
    public int totalWaves = 3; // Total number of waves to spawn
    public TextMeshProUGUI waveText; // UI text element to display wave information

    public GameObject[] possibleDrops; // Array of possible drop items after wave completion
    public Transform[] dropPoints; // Array of possible drop points for items
    public int dropsPerWave = 1; // Number of drops to spawn per wave
    private Transform player; // Reference to the player location

    public int MaxItems = 4; // max number of items that can be collected, used for win condition in DoorSceneLoader
    public int ItemsCollected = 0; // Counter for the number of items collected

    private DoorSceneLoader door;

    void Start()
    {
        // Only reset once per game session
        if (!PlayerPrefs.HasKey("SessionStarted"))
        {
            PlayerPrefs.DeleteKey("ItemsCollected");
            PlayerPrefs.DeleteKey("CurrentWave");

            PlayerPrefs.SetInt("SessionStarted", 1);
        }


        if (PlayerPrefs.HasKey("ItemsCollected"))
        {
            ItemsCollected = PlayerPrefs.GetInt("ItemsCollected");
        }

        if (PlayerPrefs.HasKey("CurrentWave"))
        {
            currentWave = PlayerPrefs.GetInt("CurrentWave");
        }
        ItemCheck();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        door = FindObjectOfType<DoorSceneLoader>(); // Find the DoorSceneLoader script in the scene

        UpdateWaveUI(); // Update the UI with the initial wave number
        StartCoroutine(SpawnWaves()); // Start spawning waves
    }


    void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave + " / " + totalWaves;
        }
    }

    IEnumerator SpawnWave(int enemyCount) // Coroutine to spawn a single wave of enemies
    {
        Debug.Log("Spawning wave " + currentWave + " with " + enemyCount + " enemies."); //enemyCount is passed from SpawnWaves Ie enemyCount = tospawn

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
  
            GameObject enemy = Instantiate(prefab, spawnPoint.position + Vector3.up * 1.2f,spawnPoint.rotation);
            SasquatchJr_Movement enemyStats = enemy.GetComponent<SasquatchJr_Movement>();
           
           
            enemyStats.SasjrDamage += (currentWave - 1) * 2; // baseDamage + (currentWave - 1) * increasePerWave += adds to
            Debug.Log("Spawned enemy with damage: " + enemyStats.SasjrDamage);
            
            enemimesAlive++;

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        }

    public void ItemCheck()
    {
        if (ItemsCollected >= MaxItems)
        {
            door.IsDoorOpen = true;

        }
    } 

    public void SpawnDrops() // Method to spawn drops after wave completion
    {
        List<int> usedIndexes = new List<int>(); // To track used drop points

        for (int i =0; i < dropsPerWave; i++)
        {
            if (possibleDrops.Length == 0 || dropPoints.Length == 0)
            {
                Debug.LogWarning("No possible drops or drop points defined.");
                return;
            }
            GameObject dropPrefab = possibleDrops[Random.Range(0, possibleDrops.Length)];
            int dropPointIndex;
            do
            {
                dropPointIndex = Random.Range(0, dropPoints.Length);
            } while (usedIndexes.Contains(dropPointIndex) && usedIndexes.Count < dropPoints.Length);

            usedIndexes.Add(dropPointIndex);
            Transform dropPoint = dropPoints[dropPointIndex];
            Instantiate(dropPrefab, dropPoint.position + Vector3.up * 1.2f, dropPoint.rotation);
        }
    }

    IEnumerator SpawnWaves() // Coroutine to spawn waves of enemies
    {
        //IEnumerator allows for pausing execution and resuming later, important for timed events like spawning enemies
        while (true)
        {
            Debug.Log("Preparing to spawn wave " + currentWave);


            if(currentWave > totalWaves)
            {
                Debug.Log("Game over! All waves completed.");
                SceneManager.LoadScene("Win");
                yield break; //stops the spawner


            }
            
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
            SpawnDrops(); // Spawn drops after wave completion
            currentWave++;
            UpdateWaveUI(); // Update the UI with the new wave number
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

