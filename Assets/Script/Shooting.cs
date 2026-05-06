using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public WeaponController weaponController;
    public int range = 100;
    public int fireRate = 15;
    public int damage = 40;
    public AudioSource audioSource;

    public Camera fpsCam; // Reference to the player's camera for raycasting


    public GameObject projectilePrefab;
    
    public float blulletSpeed = 5f;
    public Transform shootPoint;

    private float nextTimeToFire = 0f;

    public int currentAmmo;
    public int reserveAmmo;
    public bool isReloading;

    public TextMeshProUGUI ammoText;

    private GameObject lastWeapon; // To track the last equipped weapon for ammo setup
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SetupAmmo(true);

        UpdateAmmoUI();
        }

    void Shoot(WeaponStats stats, int dmg, float speed) //shooting logic for projectile weapons
    {

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Ray from the center of the screen
        Vector3 shootDir = ray.direction; // Direction to shoot the projectile

        if (stats != null && stats.projectilePrefab != null && shootPoint != null)
        {
            Quaternion flashRot = shootPoint.rotation * Quaternion.Euler(0f, 180f, 0f); // Rotate the muzzle flash to face the correct direction
            GameObject flash = Instantiate(stats.muzzleFlash, shootPoint.position, flashRot); // Spawn muzzle flash at the shoot point
            Destroy(flash, 0.5f);

            GameObject proj = Instantiate(stats.projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootDir));

            Bullet b = proj.GetComponent<Bullet>(); // Check if the projectile has a Bullet component to set damage
            if (b != null)
            {
                b.damage = dmg;
            }

            Rocket r = proj.GetComponent<Rocket>(); // Check if the projectile has a Rocket component to initialize it with the shooter's root transform
            if (r != null)
            {
                r.Init(transform.root); // Pass the shooter's root transform to the rocket for ownership tracking
            }

            Rigidbody rb = proj.GetComponent<Rigidbody>();

            if (rb != null) // Set the projectile's velocity in the shoot direction
            {
                rb.velocity = shootDir * speed;
            }

            Destroy(proj, 2f);
  


        } 
    }


    void Melee(WeaponStats stats) //melee attack logic using raycasting to detect hits within a certain range
    {

        if (stats == null) return; // Ensure stats is not null before proceeding
        {
            Debug.Log("Melee range = " + stats.RangeAttack);

            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Ray from the center of the screen for melee attack

            if (Physics.Raycast(ray, out RaycastHit hit, stats.RangeAttack)) // Perform raycast to detect hits within melee range
            {
                Debug.Log(hit.transform.name);

                Enemy_health target = hit.transform.GetComponent<Enemy_health>(); 
                if (target != null)
                {
                    target.TakeDamage(stats.damage);

                    // SPAWN HIT EFFECT
                    if (stats.hitEffect != null)
                    {
                        GameObject effect = Instantiate(
                            stats.hitEffect,
                            hit.point, // EXACT impact point
                            Quaternion.identity 
                        );

                        // Face camera (cartoon style)
                        effect.transform.forward = fpsCam.transform.forward;

                        Destroy(effect, 0.5f);
                    }
                }
            }
        }

    }

    IEnumerator ReloadRoutine(WeaponStats stats) //reload logic that plays the reload sound and waits for the reload time before refilling ammo
    {
        isReloading = true;

        if (audioSource != null && stats.reloadSound != null)
        {
            audioSource.PlayOneShot(stats.reloadSound, stats.reloadVolume);
            yield return new WaitForSeconds(stats.reloadTime);

            int needed = stats.magSize - currentAmmo;
            int toLoad = Mathf.Min(needed, reserveAmmo); // Calculate how much ammo to load based on what's needed and what's available in reserve

            currentAmmo += toLoad;
            reserveAmmo -= toLoad;
            UpdateAmmoUI();
            
        }
        isReloading = false;
    }

    void TryReload(WeaponStats stats) //check if the player can reload based on current ammo, reserve ammo, and whether they are already reloading before starting the reload routine
    {
        if (isReloading) return;
        if (stats == null) return;
        if (currentAmmo >= stats.magSize) return;
        if (reserveAmmo <= 0) return;

        StartCoroutine(ReloadRoutine(stats)); // Start the reload process
    }

    void SetupAmmo(bool force) // Set up ammo counts based on the currently equipped weapon, and only update if the weapon has changed or if forced to update
    {
        GameObject w = (weaponController != null) ? weaponController.currentWeapon : null; // Get the currently equipped weapon from the weapon controller, or null if there is no weapon controller

        if (!force && w == lastWeapon) return; // If not forced to update and the weapon hasn't changed, do nothing
        {
            
            lastWeapon = w;
        }

        if (w == null) // If there is no weapon equipped, set ammo counts to zero and update the UI
        {
            currentAmmo = 0;
            reserveAmmo = 0;
            isReloading = false;
            UpdateAmmoUI();
            return;
        }
        WeaponStats stats = w.GetComponent<WeaponStats>();
        if (stats == null) return; // If the equipped weapon doesn't have a WeaponStats component, do nothing
        {
            
            currentAmmo = stats.magSize;
            reserveAmmo = stats.reserveAmmo;
            isReloading = false;
            UpdateAmmoUI();
        }
    }

    void UpdateAmmoUI() // Update the ammo display in the UI to show the current ammo and reserve ammo counts
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + reserveAmmo;
        }
    }

    void Update()
    {
        if (GameUIController.isPaused) return; // Don't allow shooting or reloading when the game is paused
        SetupAmmo(false); //reset ammo when weapon changes

        WeaponStats stats = null; // Get the WeaponStats component from the currently equipped weapon, if there is one
        if (weaponController != null && weaponController.currentWeapon != null) // Check if the weapon controller and current weapon are not null before trying to get the WeaponStats component
        {
            stats = weaponController.currentWeapon.GetComponent<WeaponStats>();
        }


        int dmg = (stats != null) ? stats.damage : damage; // Use the damage value from the WeaponStats if available, otherwise use the default damage value from the Shooting script
        float rate = (stats != null) ? stats.fireRate : fireRate; // Use the fire rate from the WeaponStats if available, otherwise use the default fire rate value from the Shooting script
        float speed = (stats != null) ? stats.bulletSpeed : blulletSpeed; // Use the bullet speed from the WeaponStats if available, otherwise use the default bullet speed value from the Shooting script

        if (Input.GetKeyDown(KeyCode.R))
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


        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire) //player performed an attack input
        {
            if (stats == null) return;

            nextTimeToFire = Time.time + (1f / rate);

            if(stats.isMelee)
            {
                Melee(stats);
                audioSource.PlayOneShot(stats.shootSound, stats.Shootvolume);
                Debug.Log("Melee attack");

            }
            else
            {
                currentAmmo--;
                UpdateAmmoUI();
                Debug.Log("Shoot attack");

                if (stats.shootSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(stats.shootSound, stats.Shootvolume);
                }
                Shoot(stats, dmg, speed);

            }
        }

                
    }
}
