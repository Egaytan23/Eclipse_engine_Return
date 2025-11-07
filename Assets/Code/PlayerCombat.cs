using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 5;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    public Camera playerCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))  // 0 = left click 1 = right click 2 = middle click  
        {
            Attack();
        }


        if(Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)  //mousebuttondown is the isntant its pressed (getmousebutton is true while you hold it)
        {                                                               // >= enough time has passed <= not enough time has passed
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy_health>().TakeDamage(attackDamage); //Find the SasquatchJr_Movement script on the object we hit, and call its TakeDamage() method
                Debug.Log("Hit");
            }
        }
    }
}
