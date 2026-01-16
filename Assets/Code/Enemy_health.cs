using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject healthBarPrefab;
    private Slider healthSlider;
    private Transform healthBarTransform;

    //quick note: Quaternion is system used to represent rotations in 3D space, stops it from glitching or flipping (like we had before)
    void Start()
    {
        currentHealth = maxHealth;
        
        Debug.Log("Spawned enemy health bar for " + gameObject.name);
        GameObject EnemyHB = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
       
        EnemyHB.transform.localScale = Vector3.one * 0.02f; // fix scale
        EnemyHB.layer = LayerMask.NameToLayer("Default"); // ensure visible layer
        EnemyHB.transform.rotation = Quaternion.identity; // reset rotation so it doesn't inherit enemy rotation quaternion.identity means no rotation

        healthBarTransform = EnemyHB.transform;
        healthSlider = EnemyHB.GetComponentInChildren<Slider>();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Enemy took" + damageAmount + " damage. Health now: " + currentHealth);

        if(healthSlider != null)
        {
           
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        if(healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }
        Destroy(gameObject);
    }   
    void Update()
    {
        if(healthBarTransform != null) // makes sure the health bar still exists
        {
            healthBarTransform.position = transform.position + Vector3.up * 2.5f; //follow enemy position

            healthBarTransform.forward = -Camera.main.transform.forward; //face camera

        }
    }

    
}
