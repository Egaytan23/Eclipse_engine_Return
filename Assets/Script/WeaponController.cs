using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject pistolPrefab;
    public GameObject rocketPrefab;
    public GameObject riflePrefab;
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

        string weapon = PlayerPrefs.GetString("CurrentWeapon", "");
        if (weapon != "")
        {
            EquipSavedWeapon(weapon);
        }
    }
    public void EquipSavedWeapon(string weaponName)
    {
        GameObject prefab = null;

        if (weaponName == "Pistol") prefab = pistolPrefab;
        else if (weaponName == "Rocket") prefab = rocketPrefab;
        else if (weaponName == "Rifle") prefab = riflePrefab;


        if (prefab == null)
        {
            Debug.LogError("Missing prefab for " + weaponName);
            return;
        }

        GameObject weapon = Instantiate(prefab);

        //THIS IS THE IMPORTANT LINE
        EquipWeapon(weapon);
    }
    public void EquipWeapon(GameObject weapon)
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
