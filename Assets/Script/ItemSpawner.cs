using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    public GameObject[] healingPrefabs;

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

    void SpawnNONWeapons()
    {
        for (int i = 0; i < healingPrefabs.Length; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);
            Instantiate(healingPrefabs[i], spawnPos, Quaternion.identity);
            Debug.Log("Spawned " + healingPrefabs[i].name + " at " + spawnPos);
        }
    }
    void SpawnAllWeapons()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);

            GameObject weapon = Instantiate(weaponPrefabs[i], spawnPos, Quaternion.identity);

            // FORCE IT VISIBLE
            weapon.SetActive(true);

            // make sure renderer is on
            MeshRenderer mr = weapon.GetComponentInChildren<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = true;
            }

            Debug.Log("Spawned " + weaponPrefabs[i].name + " at " + spawnPos);
        }
    }
}