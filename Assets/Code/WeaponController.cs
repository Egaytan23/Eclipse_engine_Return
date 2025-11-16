using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject currentWeapon; // The weapon currently held by the player
   
    public Transform shootPoint;
    private Shooting shooting;
    void Start()
    {
        shooting = GetComponent<Shooting>();
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null) {

            DropWeapon();
        
        }

        // disables physics and collider
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        Collider col = weapon.GetComponent<Collider>();
        
        rb.isKinematic = true; // Disable physics
        
        col.enabled = true; 
        col.isTrigger = true; 

        //move weapon to weapon holder
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        currentWeapon = weapon;
        
        Shooting shooting = FindAnyObjectByType<Shooting>();

        if (shooting != null)
        {
            Transform sp = weapon.transform.transform.Find("Barrel/ShootPoint");


            if (sp != null)
            {

                shooting.shootPoint = sp;
            }

            else
            {
                Debug.LogWarning("ShootPoint not found on weapon"); 
            }

                shooting.shootPoint = sp;

        }

    }

    void DropWeapon()
    {
        

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        Collider col = currentWeapon.GetComponent<Collider>();

        rb.isKinematic = false; // Enable physics

        col.enabled = true;
        col.isTrigger = false; // Enable collider

        currentWeapon.transform.SetParent(null); // Detach from weapon holder

        rb.AddForce(transform.forward * 2f, ForceMode.Impulse);

        currentWeapon = null;
    }

        void Update()
    {
        
    }
}
