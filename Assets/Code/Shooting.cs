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
   

    public GameObject bulletPrefab;
    
    public float blulletSpeed = 5f;
    public Transform shootPoint;

    private float nextTimeToFire = 0f;
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        }

    void Shoot(int dmg, float speed)
    {
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 shootDir = ray.direction;

        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(shootDir));

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            if(rb != null)
            {
                rb.velocity = shootDir * speed;
            }

            Destroy(bullet, 1f);
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
    void Update()
    {
        WeaponStats stats = null;
        if(weaponController != null && weaponController.currentWeapon != null)
        {
            stats = weaponController.currentWeapon.GetComponent<WeaponStats>();
        }


        int dmg = (stats != null) ? stats.damage : damage;
        float rate = (stats != null) ? stats.fireRate : fireRate;
        float speed = (stats != null) ? stats.bulletSpeed : blulletSpeed;

        if (PuaseScritp.isPaused) return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / rate);
            if (stats != null && stats.shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(stats.shootSound, stats.volume);
            }

            Shoot(dmg, speed);
        }
    }
}
