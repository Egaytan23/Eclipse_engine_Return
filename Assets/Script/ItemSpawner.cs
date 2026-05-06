using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs to spawn
    public GameObject[] healingPrefabs; // Array of healing item prefabs to spawn

    public float minX = -28f;
    public float maxX = 18f;
    public float minZ = -28f;
    public float maxZ = 18f;
    public float spawnY = 0.25f;

    void Start()
    {
        SpawnAllWeapons();
        SpawnNONWeapons();
    }

    void SpawnNONWeapons() // Method to spawn non-weapon items (healing items)
    {
        for (int i = 0; i < healingPrefabs.Length; i++)
        {
            float randomX = Random.Range(minX, maxX); //Generate a random X position within the specified range
            float randomZ = Random.Range(minZ, maxZ); //Generate a random Z position within the specified range
            Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ); // Create a spawn position using the random X and Z values, and the specified Y value
            Instantiate(healingPrefabs[i], spawnPos, Quaternion.identity);
            Debug.Log("Spawned " + healingPrefabs[i].name + " at " + spawnPos);
        }
    }
    void SpawnAllWeapons() // Method to spawn all weapon items
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);

            GameObject weapon = Instantiate(weaponPrefabs[i], spawnPos, Quaternion.identity); // Instantiate the weapon prefab at the generated spawn position with no rotation

            // FORCE IT VISIBLE
            weapon.SetActive(true);

            // make sure renderer is on
            MeshRenderer mr = weapon.GetComponentInChildren<MeshRenderer>(); // Get the MeshRenderer component from the weapon or its children
            if (mr != null)
            {
                mr.enabled = true;
            }

            Debug.Log("Spawned " + weaponPrefabs[i].name + " at " + spawnPos); // Log the name of the spawned weapon and its spawn position for debugging purposes
        }
    }
}