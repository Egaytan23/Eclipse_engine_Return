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

    void Start()
    {
        currentHealth = maxHealth;
        
        Debug.Log("Spawned enemy health bar for " + gameObject.name);
        GameObject EnemyHB = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
        EnemyHB.transform.localScale = Vector3.one * 0.02f; // fix scale
        EnemyHB.layer = LayerMask.NameToLayer("Default"); // ensure visible layer

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
        if(healthBarTransform != null)
        {
            healthBarTransform.position = transform.position + Vector3.up * 2.5f;
            healthBarTransform.LookAt(Camera.main.transform);

        }
    }

    
}
