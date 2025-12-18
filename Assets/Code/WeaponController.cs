using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject currentWeapon; // The weapon currently held by the player
   
    public Transform shootPoint;
    public Shooting shooting;
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
       
        Transform hold = weapon.transform.Find("HoldPoint");
        if (hold != null)
        {
            weapon.transform.localPosition = -hold.localPosition;
            weapon.transform.localRotation = Quaternion.Inverse(hold.localRotation);
        }
        else
        {
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

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

    public void DropWeapon()
    {
       if(currentWeapon == null)
        {
            return;
        }
        // Re-enable physics and collider
        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        Collider col = currentWeapon.GetComponent<Collider>();
        currentWeapon.transform.SetParent(null);

        if(rb != null)
        {
            rb.isKinematic = false; // Enable physics
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }
        if(col != null)
        {
            col.enabled = true; 
            col.isTrigger = false;
        }
        Shooting shooting = FindAnyObjectByType<Shooting>();
        if(shooting != null)
        {
            shooting.shootPoint = null;
            currentWeapon = null;
        }
    }

        void Update()
    {
        
    }
}
