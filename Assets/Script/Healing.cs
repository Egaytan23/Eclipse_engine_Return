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
            playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth == null) return;

            // CHECK IF PLAYER NEEDS HEALING
            if (playerHealth.currentHealth < playerHealth.maxHealth)
            {
                HealPlayer();

                Debug.Log("Healed by " + healAmount +
                          " health is now " + playerHealth.currentHealth);

                Destroy(gameObject); //ONLY destroy if used
            }
            else
            {
                Debug.Log("Already at full health — cannot heal");
            }
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
