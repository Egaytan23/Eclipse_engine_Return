using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SasquatchJr_Movement : MonoBehaviour
{
    public int SasjrMAXHealth = 100;
    public int SasjrCURRhealth;
    public int SasjrDamage = 5;

    public float moveSpeed = 5f;
    public float Attackrange = 2f;
    public float Attackcooldown = 1.5f;

    private Transform player;
    private bool canAttack = true;

    void Start()
    {
        SasjrCURRhealth = SasjrMAXHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
        
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= Attackrange)
        {
            StartCoroutine(AttackPlayer());
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    

    void MoveTowardsPlayer()
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed *Time.deltaTime);
            transform.LookAt(player);
        }

    IEnumerator AttackPlayer()
        {
            if (canAttack)
            {
                canAttack = false;
                Debug.Log("Enemey attacked");
                player.GetComponent<PlayerHealth>().TakeDamage(SasjrDamage);
                yield return new WaitForSeconds(Attackcooldown);
                canAttack = true;
            }
        }

    
}
