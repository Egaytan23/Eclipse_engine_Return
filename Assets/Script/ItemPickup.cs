using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string weaponName;
    public bool pickupRange = false;
    public Light pickUplight;
    public bool isEquipped = false;

    private WeaponController player;
    void Start()
    {
        player = FindAnyObjectByType<WeaponController>();
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


            if (player != null)
            {

                player.EquipWeapon(gameObject);
                isEquipped = true;
                if (pickUplight != null)
                    //pickUplight.enabled = false;

                    PlayerPrefs.SetString("CurrentWeapon", weaponName);
                PlayerPrefs.Save();

            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {

            if (player != null)
            {
                if (player.currentWeapon == gameObject)
                {
                    player.DropWeapon();
                    isEquipped = false;
                    if (pickUplight != null)
                    {
                        //pickUplight.enabled = true; // Turn on the light when the player drops the weapon
                    }

                }
            }
        }
    }
}
