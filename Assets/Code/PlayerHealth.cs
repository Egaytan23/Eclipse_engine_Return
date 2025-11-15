using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider HealthBar;
    

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.maxValue = maxHealth;
        HealthBar.value = currentHealth; 
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth,0, maxHealth);
        HealthBar.value = currentHealth;
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Lose");
    }
    
    void Update()
    {
       
    }
}
