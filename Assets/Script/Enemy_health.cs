using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject Numbers;
    private Slider healthSlider;
    private Transform healthBarTransform;
    public AudioSource hitSound;

    //quick note: Quaternion is system used to represent rotations in 3D space, stops it from glitching or flipping (like we had before)
    void Start()
    {
        hitSound = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        //healthSlider.maxValue = maxHealth;
        //healthSlider.value = currentHealth;
    }

    public void DropItem()
    {
        Debug.Log("Dropping item");
        GameObject item = Instantiate(Numbers, transform.position, Quaternion.identity);    
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        hitSound.Play();
       
        Debug.Log("Enemy took" + damageAmount + " damage. Health now: " + currentHealth);

        if(healthSlider != null)
        {
           
            healthSlider.value = currentHealth;
            
        }

        if (currentHealth <= 0)
        {
            Die();
            DropItem();
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
        
    }

    
}
