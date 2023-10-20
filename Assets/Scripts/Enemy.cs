using UnityEngine;
using UnityEngine.AI;

public class Enemy : PlayableEntity
{
    private GameObject player;
    private float attackCooldown;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }
    
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    protected override void FirePrimary()
    {
        Debug.Log("Primary fire fired");
    }
    
    protected override void FireSecondary()
    {
        Debug.Log("Secondary fire fired");
    }
    
    private Vector3 FindBestValidPoint()
    {
        int numberOfDirections = 8;
        Vector3 bestPoint = Vector3.zero;
        float maxDistance = 0;

        for (int i = 0; i < numberOfDirections; i++)
        {
            float angle = i * (360f / numberOfDirections);
            Vector3 position = transform.position;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * (position - GameManager.Instance.player.transform.position);
            Vector3 possiblePoint = position + dir.normalized * GameManager.Instance.player.fireRadius;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(possiblePoint, out navHit, agent.speed, NavMesh.AllAreas))
            {
                float distanceFromPlayer = Vector3.Distance(navHit.position, GameManager.Instance.player.transform.position);
                if (distanceFromPlayer > maxDistance)
                {
                    maxDistance = distanceFromPlayer;
                    bestPoint = navHit.position;
                }
            }
        }
        return bestPoint;
    }

    protected override void Update()
    {
        base.Update();
        attackCooldown -= Time.deltaTime;
        
        // If there are PowerUp in the map, follow him 
        if (GameManager.Instance.PowerUpCount > 0)
        {
            
        }
        // If there are no PowerUp, but ability are ready, focus on attack
        else if ((primaryActualCooldown <= 0 || secondaryActualCooldown <= 0) && attackCooldown <= 0)
        {
            Debug.Log("Enemy attacking");
            agent.destination = GameManager.Instance.player.transform.position;
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 3)
            {
                if (primaryActualCooldown <= 0)
                {
                    FirePrimary();
                    Debug.Log("Enemy fired");
                    attackCooldown = 3;
                }
                else if (secondaryActualCooldown <= 0)
                {
                    FireSecondary();
                    attackCooldown = 3;
                }
            }
        }
        // Escape in other case
        else
        {
            Vector3 position = transform.position;
            Vector3 directionAwayFromPlayer = position - GameManager.Instance.player.transform.position;
            Vector3 destination = position + directionAwayFromPlayer.normalized * GameManager.Instance.player.fireRadius;

            // Verifying that final destination is valid
            if (NavMesh.SamplePosition(destination, out var navHit, GameManager.Instance.player.fireRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
            else
            {
                // If point not valid, find best escape route
                Vector3 bestValidPoint = FindBestValidPoint();
                if (bestValidPoint != Vector3.zero)
                {
                    agent.SetDestination(bestValidPoint);
                }
            }
            Debug.Log("Enemy escaping");
        }
    }
}
