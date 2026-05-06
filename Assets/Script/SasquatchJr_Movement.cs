using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SasquatchJr_Movement : MonoBehaviour
{
    Animator animator;
    public int SasjrDamage = 5;

    public float moveSpeed = 5f;
    public float Attackrange = 2f;
    public float Attackcooldown = 1.5f;

    public float circleRadius = 3.5f; // distance at which the enemy starts circling the player instead of directly chasing
    public float circleSpeed = 2f; // how fast the enemy circles around the player when close
    public float repathTime = 0.5f;// how often the enemy recalculates its path when circling (to avoid too much CPU usage)

    private Transform player;
    private NavMeshAgent agent; // NavMeshAgent is a component that allows characters to navigate the game world using Unity's navigation system. It handles pathfinding and movement, making it easier to create AI that can move around obstacles and find the
    //we use navmeshagent to make the enemy move towards the player, it allows it to navigate around obstacles and find the best path to the player
    //this helped with the crowding issue we had before where the enemies stacked on top of each other.
    private float nextAttackTime = 0f;
    private float nextRepathTime = 0f;

    private float circleOffset; // random starting angle for circling to give each enemy a unique movement pattern

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // assumes Animator is on a child object (like the model)    
        player = GameObject.FindGameObjectWithTag("Player").transform; // assumes player has "Player" tag

        agent = GetComponent<NavMeshAgent>(); // assumes NavMeshAgent component is attached

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = Attackrange;
        }

        // gives each enemy a different starting angle around the player
        circleOffset = Random.Range(0f, 360f);
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        //STATE 1: CHASE (far away)
        if (distance > circleRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        //STATE 2: CIRCLE (mid range)
        else if (distance > Attackrange)
        {
            CirclePlayer();
        }

        // STATE 3: ATTACK (close range)
        else
        {
            agent.isStopped = true;

            // Face the player
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0f;

            if (lookPos != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookPos);
            }

            // Attack with cooldown
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + Attackcooldown;
            }
        }

        // Animation update
        if (animator != null)
        {
            float speed = agent.desiredVelocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    void CirclePlayer() // Method to make the enemy circle around the player when within a certain radius
    {
            if (Time.time < nextRepathTime) return; // Only recalculate path at intervals to reduce CPU usage

        nextRepathTime = Time.time + repathTime; // Calculate the angle for circling based on time, speed, and a random offset to give each enemy a unique pattern

        float angle = Time.time * circleSpeed * 50f + circleOffset; // The 50f multiplier is arbitrary to make the circling speed feel good, adjust as needed
        float radians = angle * Mathf.Deg2Rad; // Convert angle to radians for trigonometric functions

        Vector3 circlePosition = new Vector3( // Calculate the position on the circle around the player using cosine and sine functions
                Mathf.Cos(radians) * circleRadius, // X position on the circle
                0f,
                Mathf.Sin(radians) * circleRadius // Z position on the circle
            );

            Vector3 targetPosition = player.position + circlePosition; // The target position for the enemy to move towards is the player's position plus the calculated circle offset

        NavMeshHit hit; // Check if the target position is on the NavMesh (i.e., a valid position for the enemy to move to). If it is, set the agent's destination to that position. This ensures the enemy can navigate around obstacles while circling.
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.5f, NavMesh.AllAreas)) // The 1.5f is the max distance to search for a valid position on the NavMesh around the target position, adjust as needed based on your game's scale and obstacle density
        {
                agent.isStopped = false; // Ensure the agent is not stopped so it can move towards the new destination
            agent.SetDestination(hit.position); // Set the agent's destination to the valid position found on the NavMesh, allowing it to circle around the player while navigating around obstacles
        }
        }

        void AttackPlayer()
        {
            Debug.Log("Enemy attacked");

            PlayerHealth health = player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(SasjrDamage);
            }
        }
    }