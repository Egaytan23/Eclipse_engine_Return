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

    public void Init(Transform owner)
    {
        ownerRoot = owner;

        if (myCol != null && ownerRoot != null)
        {
            foreach (Collider c in ownerRoot.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(myCol, c, true);
            }
        }
    }

    IEnumerator ArmAfterDelay()
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

    void Explode(Vector3 center)
    {
        Debug.Log("ROCKET exploding at " + transform.position);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        HashSet<Enemy_health> damagedEnemies = new HashSet<Enemy_health>();
        HashSet<Rigidbody> pushed = new HashSet<Rigidbody>(); // ✅ moved OUTSIDE loop

        foreach (Collider hit in hits)
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
