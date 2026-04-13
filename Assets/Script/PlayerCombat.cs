using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 5;
    public float attackRate = 1f;
   

    public Camera playerCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameUIController.isPaused) return;
        
    }

    
}
