using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public Slider HealthBar;
    public AudioSource AudioSource;
   

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        HealthBar.maxValue = maxHealth;
        HealthBar.value = currentHealth; 
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        AudioSource.Play();
        HealthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
       
        Destroy(gameObject);
        FindObjectOfType<GameUIController>().LoadLoseScreen();
    }
    
    void Update()
    {
       
    }
}
