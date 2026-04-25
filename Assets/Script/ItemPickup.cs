using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string weaponName;
    public bool pickupRange = false;
    private Light pickUplight;
    


    void Start()
    {
        pickUplight = GetComponentInChildren<Light>(); // Assuming the light is a child of the item
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered weapon trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER ENTERED PICKUP RANGE");
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

                PlayerPrefs.SetString("CurrentWeapon", weaponName);
                PlayerPrefs.Save();

                if (pickUplight != null)
                    pickUplight.enabled = false;
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
