using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab for the explosion effect to instantiate upon impact
    public float speed = 20f; 
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDamage = 80f;
    public float lifetime = 5f;
    public float upwardModifier = 0.3f; // Adjusts the vertical component of the explosion force to create a more realistic blast effect
    bool armed = false;

    Rigidbody rb;
    Collider myCol;
    Transform ownerRoot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

       
        rb.velocity = transform.forward * speed;
        
        Destroy(gameObject, lifetime);
        StartCoroutine(ArmAfterDelay());
    }

    public void Init(Transform owner) // Method to initialize the rocket with the shooter's root transform for ownership tracking
    {
        ownerRoot = owner; // Store the shooter's root transform to ignore collisions with the shooter

        if (myCol != null && ownerRoot != null) // Ensure the rocket has a collider and the owner root is valid before trying to ignore collisions
        {
            foreach (Collider c in ownerRoot.GetComponentsInChildren<Collider>()) // Loop through all colliders in the shooter's hierarchy to ignore collisions with the rocket
            {
                Physics.IgnoreCollision(myCol, c, true); // Ignore collisions between the rocket's collider and each of the shooter's colliders
            }
        }
    }

    IEnumerator ArmAfterDelay() // Coroutine to arm the rocket after a short delay, allowing it to safely ignore collisions with the shooter before it can explode on impact
    {
        yield return new WaitForSeconds(0.05f);
        armed = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!armed)
        {
            return;
        }

        if (collision.collider.isTrigger)
        {
            return;
        }

        Explode(collision.contacts[0].point);
    }

    void Explode(Vector3 center) // Method to handle the explosion logic, including damage and physics effects on nearby objects
    {
        Debug.Log("ROCKET exploding at " + transform.position);
        // Instantiate explosion effect at the point of impact
        if (explosionPrefab != null)
        {
           GameObject explosion = Instantiate(explosionPrefab, center, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * 2.5f;
            Destroy(explosion, 2f);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius); // Get all colliders within the explosion radius

        HashSet<Enemy_health> damagedEnemies = new HashSet<Enemy_health>(); // check moved OUTSIDE loop to track which enemies have already been damaged to prevent multiple damage applications
        HashSet<Rigidbody> pushed = new HashSet<Rigidbody>(); // Check moved OUTSIDE loop

        foreach (Collider hit in hits) // Loop through each collider hit by the explosion to apply damage and physics effects
        {
            // Damage
            Enemy_health enemy = hit.GetComponentInParent<Enemy_health>();
            if (enemy != null && damagedEnemies.Add(enemy))
            {
                Debug.Log("ROCKET damaging: " + enemy.gameObject.name);
                enemy.TakeDamage((int)explosionDamage);
            }

            // Push
            Rigidbody hitRb = hit.attachedRigidbody;
            if (hitRb != null && pushed.Add(hitRb))
            {
                hitRb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    upwardModifier,
                    ForceMode.Impulse
                );
            }
        }

        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
