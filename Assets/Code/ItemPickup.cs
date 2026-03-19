using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool pickupRange = false;
    private Light pickUplight;
    


    void Start()
    {
        pickUplight = GetComponentInChildren<Light>(); // Assuming the light is a child of the item
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupRange = false;
            

        }
    }


    void Update()
    {
        if (pickupRange && Input.GetKeyDown(KeyCode.LeftShift))
        {
            WeaponController player = FindAnyObjectByType<WeaponController>();

            if (player != null)
            {
                player.EquipWeapon(gameObject);
                pickUplight.enabled = false; // Turn off the light when the player leaves the pickup range
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            WeaponController player = FindAnyObjectByType<WeaponController>();
            if (player != null)
            {
                if (player.currentWeapon == gameObject)
                {
                    player.DropWeapon();
                    pickUplight.enabled = true; // Turn off the light when the player leaves the pickup range
                }
            }
        }
    }
}
