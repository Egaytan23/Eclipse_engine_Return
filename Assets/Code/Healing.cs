using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public int healAmount = 10; // Amount of health to restore
    

    private PlayerHealth playerHealth; // Reference to the player's health script
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<PlayerHealth>(); // Get the PlayerHealth component from the player object
            HealPlayer();
            Debug.Log("healed by " + healAmount + " health is now " + playerHealth.currentHealth);
            Destroy(gameObject); // Destroy the healing object after use
        }
    }

   
    public void HealPlayer() // Method to heal the player
    {
        playerHealth.currentHealth += healAmount; // Increase the player's current health by the heal amount
        playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0, playerHealth.maxHealth); // Ensure health does not exceed max
        if (playerHealth.HealthBar != null)
        {
            playerHealth.HealthBar.value = playerHealth.currentHealth; // Update the health bar value
        }
    }

    void Update()
    {
        
    }
}
