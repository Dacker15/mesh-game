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
            Debug.Log("Enemy escaping");
        }
    }
}
