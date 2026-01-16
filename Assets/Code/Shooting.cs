using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public WeaponController weaponController;
    public int range = 100;
    public int fireRate = 15;
    public int damage = 40;
    public AudioSource audioSource;

    public Camera fpsCam;


    public GameObject projectilePrefab;
    
    public float blulletSpeed = 5f;
    public Transform shootPoint;

    private float nextTimeToFire = 0f;

    public int currentAmmo;
    public int reserveAmmo;
    public bool isReloading;

    private GameObject lastWeapon;
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SetupAmmo(true);
        }

    void Shoot(WeaponStats stats, int dmg, float speed)
    {

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 shootDir = ray.direction;

        if (stats != null && stats.projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(stats.projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootDir));

            Rigidbody rb = proj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = shootDir * speed;
            }

            Destroy(proj, 5f);
        }



        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemy_health target = hit.transform.GetComponent<Enemy_health>();

            if (target != null)
            {
                target.TakeDamage(dmg);
            }


        }
    }

    IEnumerator ReloadRoutine(WeaponStats stats)
    {
        isReloading = true;

        if (audioSource != null && stats.reloadSound != null)
        {
            audioSource.PlayOneShot(stats.reloadSound, stats.reloadVolume);
            yield return new WaitForSeconds(stats.reloadTime);

            int needed = stats.magSize - currentAmmo;
            int toLoad = Mathf.Min(needed, reserveAmmo);

            currentAmmo += toLoad;
            reserveAmmo -= toLoad;
            isReloading = false;
        }
    }

    void TryReload(WeaponStats stats)
    {
        if (isReloading) return;
        if (stats == null) return;
        if (currentAmmo >= stats.magSize) return;
        if (reserveAmmo <= 0) return;

        StartCoroutine(ReloadRoutine(stats));
    }

    void SetupAmmo(bool force)
    {
        GameObject w = (weaponController != null) ? weaponController.currentWeapon : null;

        if (!force && w == lastWeapon) return;
        {
            
            lastWeapon = w;
        }

        if (w == null)
        {
            currentAmmo = 0;
            reserveAmmo = 0;
            isReloading = false;
            return;
        }
        WeaponStats stats = w.GetComponent<WeaponStats>();
        if (stats == null) return;
        {
            
            currentAmmo = stats.magSize;
            reserveAmmo = stats.reserveAmmo;
            isReloading = false;
        }
    }

    void Update()
    {
        if (PuaseScritp.isPaused) return;
        SetupAmmo(false); //reset ammo when weapon changes

        WeaponStats stats = null;
        if(weaponController != null && weaponController.currentWeapon != null)
        {
            stats = weaponController.currentWeapon.GetComponent<WeaponStats>();
        }


        int dmg = (stats != null) ? stats.damage : damage;
        float rate = (stats != null) ? stats.fireRate : fireRate;
        float speed = (stats != null) ? stats.bulletSpeed : blulletSpeed;

        if(Input.GetKeyDown(KeyCode.R))
        {
            TryReload(stats);
        }

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            TryReload(stats);
            return;
        }


        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / rate);
            currentAmmo--;

            if (stats != null && stats.shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(stats.shootSound, stats.Shootvolume);
            }

            Shoot(stats, dmg, speed);
        }
    }
}
