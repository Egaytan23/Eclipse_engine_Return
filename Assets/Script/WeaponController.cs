using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject pistolPrefab; // Prefabs for each weapon type
    public GameObject rocketPrefab; // Prefabs for each weapon type
    public GameObject riflePrefab;  // Prefabs for each weapon type
    public Transform weaponHolder; //   The transform where the weapon will be held (e.g., player's hand)
    public GameObject currentWeapon; // The weapon currently held by the player

    public Transform shootPoint; // The point from which projectiles will be fired, set by the equipped weapon
    public Shooting shooting; // Reference to the Shooting script to update the shoot point when equipping weapons

  


    void Start()
    {
        shooting = GetComponent<Shooting>();

        string weapon = PlayerPrefs.GetString("CurrentWeapon", "");
        if (weapon != "")
        {
            EquipSavedWeapon(weapon);
        }
    }
    public void EquipSavedWeapon(string weaponName) // This method is called on Start to equip the weapon saved in PlayerPrefs
    {
        GameObject prefab = null;

        if (weaponName == "Pistol") prefab = pistolPrefab; // Determine which prefab to use based on the weapon name
        else if (weaponName == "Rocket") prefab = rocketPrefab; // Determine which prefab to use based on the weapon name
        else if (weaponName == "Rifle") prefab = riflePrefab; // Determine which prefab to use based on the weapon name


        if (prefab == null)
        {
            Debug.LogError("Missing prefab for " + weaponName);
            return;
        }

        GameObject weapon = Instantiate(prefab); 

        //THIS IS THE IMPORTANT LINE
        EquipWeapon(weapon);
    }
    public void EquipWeapon(GameObject weapon) // This method is called by ItemPickup when the player picks up a weapon, and also by EquipSavedWeapon to equip the saved weapon on Start
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        Collider col = weapon.GetComponent<Collider>();

        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
        {
            col.enabled = false;
            col.isTrigger = false;
        }

        weapon.transform.SetParent(weaponHolder);

        // TURN OFF HALO (ROOT  CHILD SAFE)
        Behaviour halo = (Behaviour)weapon.GetComponent("Halo");

        if (halo == null)
        {
            Component[] comps = weapon.GetComponentsInChildren<Component>();
            foreach (Component c in comps)
            {
                if (c.GetType().Name == "Halo")
                {
                    halo = (Behaviour)c;
                    break;
                }
            }
        }

        if (halo != null)
        {
            halo.enabled = false;
        }

        WeaponStats stats = weapon.GetComponent<WeaponStats>();

        if (stats != null)
        {
            weapon.transform.localPosition = stats.holdLocalPosition; // Set the local position and rotation based on the weapon's stats
            weapon.transform.localRotation = Quaternion.Euler(stats.holdlocalEuler); // Set the local position and rotation based on the weapon's stats
        }
        else
        {
            weapon.transform.localPosition = Vector3.zero; // Default to zero if no stats are found
            weapon.transform.localRotation = Quaternion.identity; // Default to identity if no stats are found
        }

        currentWeapon = weapon;

        Shooting shooting = FindAnyObjectByType<Shooting>();

        if (shooting != null)
        {
            Transform sp = weapon.transform.Find("Barrel/ShootPoint");

            if (sp != null)
                shooting.shootPoint = sp;
            else
                Debug.LogWarning("ShootPoint not found on weapon");
        }
    }
    public void DropWeapon()
    {
        if (currentWeapon == null)
            return;

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        Collider col = currentWeapon.GetComponent<Collider>();

        currentWeapon.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }

        if (col != null)
        {
            col.enabled = true;
            col.isTrigger = true;
        }

        //TURN HALO BACK ON (ROOT + CHILD SAFE)
        Behaviour halo = (Behaviour)currentWeapon.GetComponent("Halo");

        if (halo == null)
        {
            Component[] comps = currentWeapon.GetComponentsInChildren<Component>();
            foreach (Component c in comps)
            {
                if (c.GetType().Name == "Halo")
                {
                    halo = (Behaviour)c;
                    break;
                }
            }
        }

        if (halo != null)
        {
            halo.enabled = true;
        }

        Shooting shooting = FindAnyObjectByType<Shooting>();
        if (shooting != null)
        {
            shooting.shootPoint = null;
        }

        currentWeapon = null;
    }
    void Update()
    {

    }
}
