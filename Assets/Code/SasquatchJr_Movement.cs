using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SasquatchJr_Movement : MonoBehaviour
{
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

        float distance = Vector3.Distance(transform.position, player.position); // calculates distance between enemy and player

        // If enemy is far, chase player normally
        if (distance > circleRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // when close, move around the player instead of directly into them
            CirclePlayer();
        }

        // Attack only when actually in attack range
        if (distance <= Attackrange)
        {
            agent.isStopped = true;

            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0f;

            if (lookPos != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookPos);
            }

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + Attackcooldown;
            }
        }
    }

    void CirclePlayer()
    {
        if (Time.time < nextRepathTime) return;

        nextRepathTime = Time.time + repathTime;

        float angle = Time.time * circleSpeed * 50f + circleOffset;
        float radians = angle * Mathf.Deg2Rad;

        Vector3 circlePosition = new Vector3(
            Mathf.Cos(radians) * circleRadius,
            0f,
            Mathf.Sin(radians) * circleRadius
        );

        Vector3 targetPosition = player.position + circlePosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
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