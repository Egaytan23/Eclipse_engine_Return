using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammoAmount = 10;

    private Shooting shooting; // Reference to the Shooting script

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shooting = collision.GetComponent<Shooting>(); // Get the Shooting component from the player object
            AddAmmoToWeapon();
            Destroy(gameObject); // Destroy the ammo pickup object after adding ammo to the player's weapon
        }
    }
    public void AddAmmoToWeapon() // Method to add ammo to the player's weapon
    {
       shooting.reserveAmmo += ammoAmount; // Increase the player's current ammo by the ammo amount

        if (shooting.ammoText != null)
        {
            shooting.ammoText.text = shooting.currentAmmo + " / " + shooting.reserveAmmo; // Update the ammo UI text
        }
    }
    void Update()
    {
        
    }
}
