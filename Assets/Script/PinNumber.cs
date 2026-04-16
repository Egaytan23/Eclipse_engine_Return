using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinNumber : MonoBehaviour
{
    private SpawnEnemy spawnEnemy; // Reference to the SpawnEnemy script to access the ItemsCollected variable
    void Start()
    {
        spawnEnemy = FindObjectOfType<SpawnEnemy>(); // Find the SpawnEnemy script in the scene
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the pin number trigger object after the player enters it
            spawnEnemy.ItemsCollected++; // Increment the ItemsCollected counter to indicate that the pin number has been collected
            spawnEnemy.ItemCheck(); // Call the ItemCheck method to check if the win condition has been met
            Debug.Log("pin collected");
        }
    }

    
    void Update()
    {
        
    }
}
