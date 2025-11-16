using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool pickupRange = false;


  
    void Start()
    {
        
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
            }
        }
    }
}
