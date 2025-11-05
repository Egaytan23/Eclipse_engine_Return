using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int range = 100;
    public int fireRate = 15;
    public int damage = 40;

    public Camera fpsCam;
   

    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float blulletSpeed = 5f;

    private float nextTimeToFire = 0f;
    void Start()
    {
        
    }

    void Shoot()
    {
        

        if(bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = shootPoint.forward * blulletSpeed;
            }

            Destroy(bullet, 2f);
        }

       

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);
            
            SasquatchJr_Movement target = hit.transform.GetComponent<SasquatchJr_Movement>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            
        }
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
}
