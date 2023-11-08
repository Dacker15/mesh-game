using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PlayableEntity
{
    private float attackCooldown;
    private NavMeshAgent agent;
    private PowerUp nearestPowerUp;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        GameEvents.onPowerUpSpawn += HandlePowerUpSpawn;
        GameEvents.onPowerUpPick += HandlePowerUpPick;
    }

    private void OnDestroy()
    {
        GameEvents.onPowerUpSpawn -= HandlePowerUpSpawn;
        GameEvents.onPowerUpPick -= HandlePowerUpPick;
    }


    protected override void OnFirePrimarySuccess(float damage)
    {
        GameEvents.EnemyHit(damage);
    }

    protected override void OnFireSecondarySuccess(float damage)
    {
        GameEvents.EnemyHit(damage);
    }

    public override IEnumerator SpeedPowerUp(float value, float time)
    {
        float originalSpeed = agent.speed;
        agent.speed *= value;
        yield return new WaitForSeconds(time);
        agent.speed = originalSpeed;
        yield return null;
    }

    public override IEnumerator DamagePowerUp(float value, float time)
    {
        float originalPrimaryDamage = controller.primaryDamage;
        float originalSecondaryDamage = controller.secondaryDamage;
        controller.primaryDamage *= value;
        controller.secondaryDamage *= value;
        yield return new WaitForSeconds(time);
        controller.primaryDamage = originalPrimaryDamage;
        controller.secondaryDamage = originalSecondaryDamage;
        yield return null;
    }

    public override void HealPowerUp(float value)
    {
        Heal(value);
    }

    public override void CooldownPowerUp(float value)
    {
        controller.primaryActualCooldown /= value;
        controller.secondaryActualCooldown /= value;
    }

    private void HandlePowerUpSpawn(PowerUp powerUp)
    {
        if (nearestPowerUp == null)
        {
            nearestPowerUp = powerUp;
        }
        else
        {
            Vector3 position = transform.position;
            float prevDistance = Vector3.Distance(nearestPowerUp.transform.position, position);
            float nextDistance = Vector3.Distance(powerUp.transform.position, position);
            if (nextDistance < prevDistance)
            {
                nearestPowerUp = powerUp;
            }
        }
    }

    private void HandlePowerUpPick(PowerUp powerUp, Collider other)
    {
        if (nearestPowerUp != null && powerUp.id == nearestPowerUp.id)
        {
            nearestPowerUp = null;
        }
    }
    
    private Vector3 FindBestValidPoint()
    {
        int numberOfDirections = 8;
        Vector3 bestPoint = Vector3.zero;
        float maxDistance = 0;
        float maxFireRadius = Mathf.Max(GameManager.Instance.player.controller.primaryFireRadius, GameManager.Instance.player.controller.secondaryFireRadius);

        for (int i = 0; i < numberOfDirections; i++)
        {
            float angle = i * (360f / numberOfDirections);
            Vector3 position = transform.position;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * (position - GameManager.Instance.player.transform.position);
            Vector3 possiblePoint = position + dir.normalized * maxFireRadius;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(possiblePoint, out navHit, maxFireRadius, NavMesh.AllAreas))
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

    protected virtual void Update()
    {
        attackCooldown -= Time.deltaTime;
        
        // If there are PowerUp in the map, follow him 
        if (nearestPowerUp != null)
        {
            agent.destination = nearestPowerUp.transform.position;
        }
        // If there are no PowerUp, but ability are ready, focus on attack
        else if ((controller.isPrimaryFireReady() || controller.isSecondaryFireReady()) && attackCooldown <= 0)
        {
            agent.destination = GameManager.Instance.player.transform.position;
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 3)
            {
                if (controller.isPrimaryFireReady())
                {
                    FirePrimary();
                    attackCooldown = 3;
                }
                else if (controller.isSecondaryFireReady())
                {
                    FireSecondary();
                    attackCooldown = 3;
                }
            }
        }
        // Escape in other case
        else
        {
            float maxFireRadius = Mathf.Max(GameManager.Instance.player.controller.primaryFireRadius, GameManager.Instance.player.controller.secondaryFireRadius);
            Vector3 position = transform.position;
            Vector3 directionAwayFromPlayer = position - GameManager.Instance.player.transform.position;
            Vector3 destination = position + directionAwayFromPlayer.normalized * maxFireRadius;

            // Verifying that final destination is valid
            if (NavMesh.SamplePosition(destination, out var navHit, maxFireRadius, NavMesh.AllAreas))
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
        }
    }
}
