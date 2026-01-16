using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDamage = 50f;
    public float lifetime = 5f;
    public float upwardModifier = 0.3f;

    Rigidbody rb;
    Collider myCol;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null && myCol != null)
        {
            foreach (Collider c in player.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(myCol, c, true);
            }
        }
        rb.useGravity = false;
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.isTrigger)
        {
            return;
        }

        Explode();
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        HashSet<Enemy_health> damagedEnemies = new HashSet<Enemy_health>(); // To avoid double damage

        foreach (Collider hit in hits)
        {
            Enemy_health enemy = hit.GetComponentInParent<Enemy_health>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                damagedEnemies.Add(enemy);
                enemy.TakeDamage((int)explosionDamage);
            }
            HashSet<Rigidbody> pushed = new HashSet<Rigidbody>();
            Rigidbody hitRb = hit.attachedRigidbody; // Get the Rigidbody attached to the collider
            if (hitRb != null && !pushed.Contains(hitRb))
            {
                pushed.Add(hitRb);
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
