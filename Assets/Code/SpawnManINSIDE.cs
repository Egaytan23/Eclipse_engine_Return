using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManINSIDE : MonoBehaviour
{
    [SerializeField] private string defaultSpawnID = "InteriorSpawn";


    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Debug.LogWarning("SpawnManINSIDE: no Player found");
            return; // No Player found, can't spawn
        }

        var points = FindObjectsOfType<SpawnINSIDE>();
        foreach (var p in points)
        {
            if (p.id == defaultSpawnID)
            {
                player.transform.SetPositionAndRotation(p.transform.position, p.transform.rotation);
                return; // Spawned successfully, exit the method
            }
            Debug.LogWarning($"SpawnManINSIDE: Spawn point with ID '{defaultSpawnID}' not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
