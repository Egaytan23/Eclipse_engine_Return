using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject currentWeapon; // The weapon currently held by the player
   
    public Transform shootPoint;
    public Shooting shooting;

    public GameObject pistol;
    public GameObject rocket;
    public GameObject rifle;
    public GameObject chicken;
    public GameObject pan;

    void Start()
    {
        shooting = GetComponent<Shooting>();
        if (PlayerPrefs.HasKey("CurrentWeapon"))
        {
            string weaponName = PlayerPrefs.GetString("CurrentWeapon");
            EquipSavedWeapon(weaponName);
        }
    }

    public void EquipSavedWeapon(string weaponName)
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (GameObject w in weapons)
        {
            ItemPickup pickup = w.GetComponent<ItemPickup>();

            if (pickup != null && pickup.weaponName == weaponName)
            {
                EquipWeapon(w);
                return;
            }
        }

        Debug.Log("Saved weapon not found in scene");
    }
    public void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null) {

            DropWeapon();
        
        }

        // disables physics and collider
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        Collider col = weapon.GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics
        }

        if (col != null)
        {
            col.isTrigger = false; //keeps it a normal collider 
            col.enabled = false; // Disable collider
        }

        //move weapon to weapon holder
        weapon.transform.SetParent(weaponHolder);

       
        WeaponStats stats = weapon.GetComponent<WeaponStats>();

        if (stats != null)
        {
            weapon.transform.localPosition = stats.holdLocalPosition;
            weapon.transform.localRotation = Quaternion.Euler(stats.holdlocalEuler);
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
            col.isTrigger = true;
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
