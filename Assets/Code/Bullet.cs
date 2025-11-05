using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 40;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            SasquatchJr_Movement target = collision.gameObject.GetComponent<SasquatchJr_Movement>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
    void Update()
    {
        
    }
}
