using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string weaponName;

    public Light pickUplight;

    public bool isEquipped = false;

    private WeaponController player;

    // Prevents instantly re-picking up a dropped gun
    private bool canPickup = true;

    void Start()
    {
        player = FindAnyObjectByType<WeaponController>();

        pickUplight = GetComponentInChildren<Light>();
    }

    void OnTriggerEnter(Collider other) // Called when the player enters the pickup area
    {
        if (!canPickup) // Prevents instantly re-picking up a dropped gun
            return;

        if (other.CompareTag("Player")) // Check if the player is in range
        {
            if (player != null) // Check if the player reference is valid
            {
                player.EquipWeapon(gameObject); // Equip the weapon

                isEquipped = true; // Mark the weapon as equipped

                if (pickUplight != null) // Disable the pickup light when equipped, SUPER IMPORTANT this helped when the light wouldnt turn off. FYI halo and light are different components, the light is the one that actually emits light and the halo is just a visual effect that can be turned on or off. So if you want to turn off the light, you need to disable the Light component, not the Halo component.
                {
                    pickUplight.enabled = false;
                }
                //this part lets you keep the weapon after picking it up. 
                PlayerPrefs.SetString("CurrentWeapon", weaponName); // Save the equipped weapon name to PlayerPrefs

                PlayerPrefs.Save(); // Ensure the data is saved immediately
            }
        }
    }

    // Called by WeaponController after dropping
    public IEnumerator EnablePickupDelay() // This coroutine is called by WeaponController after dropping the weapon to prevent immediate re-pickup
    {
        canPickup = false;

        yield return new WaitForSeconds(1f);

        canPickup = true;
    }
}