using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : PlayableEntity
{
    private float attackCooldown;
    private NavMeshAgent agent;
    private PowerUp nearestPowerUp;
    private PowerUp latestPowerUp;
    private short nextAbilityToUse;
    private short abilityUsedCount;
    private short impreciseShotCount;
    private short powerUpManagementCount;
    private Vector3[] corners;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        GameEvents.onPowerUpSpawn += HandlePowerUpFollow;
        GameEvents.onPowerUpPick += HandlePowerUpPick;
        nextAbilityToUse = (short)Random.Range(1, 2);
        abilityUsedCount = 0;
        
        corners = new Vector3[]
        {
            new Vector3(25, 0, 25),
            new Vector3(25, 0, -25),
            new Vector3(-25, 0, 25),
            new Vector3(-25, 0, -25),
        };
    }

    private void OnDestroy()
    {
        GameEvents.onPowerUpSpawn -= HandlePowerUpFollow;
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
    }

    public override void HealPowerUp(float value)
    {
        Heal(value);
    }

    public override void CooldownPowerUp(float value)
    {
        float nextValue = 1 - value;
        controller.primaryActualCooldown *= nextValue;
        controller.secondaryActualCooldown *= nextValue;
    }

    private void HandlePowerUpFollow(PowerUp powerUp)
    {
        latestPowerUp = powerUp;
        
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 prevPowerUpPosition = nearestPowerUp != null ? nearestPowerUp.transform.position : Vector3.positiveInfinity;
        Vector3 nextPowerUpPosition = powerUp.transform.position;
 
        bool isNextNearest = Vector3.Distance(nextPowerUpPosition, enemyPosition) < Vector3.Distance(prevPowerUpPosition, enemyPosition);
        bool isPlayerNearestToNextThanEnemy = Vector3.Distance(nextPowerUpPosition, playerPosition) < Vector3.Distance(nextPowerUpPosition, enemyPosition);
        if (isNextNearest && !isPlayerNearestToNextThanEnemy)
        {
            nearestPowerUp = powerUp;
        } 
        else if (isNextNearest)
        {
            nearestPowerUp = null;
        } 
        else if (isPlayerNearestToNextThanEnemy)
        {
            Debug.Log("Continue follow prev power up");
        }
        else
        {
            nearestPowerUp = null;
        }
    }

    private void HandlePowerUpPick(PowerUp powerUp, Collider other)
    {
        if (nearestPowerUp != null && powerUp.id == nearestPowerUp.id)
        {
            nearestPowerUp = null;
        }
        if (latestPowerUp != null && powerUp.id == latestPowerUp.id)
        {
            latestPowerUp = null;
        }
    }

    private void setAgentDestination(Vector3 destination)
    {
        if (isTransformActive && isRotationActive)
        {
            agent.SetDestination(destination);
        }
        else
        {
            agent.ResetPath();
        }
    }

    private void FindNextAbilityToUse()
    {
        float probabilityReduce = abilityUsedCount * 10;
        float minInclusive = nextAbilityToUse == 1 ? Math.Min(probabilityReduce, 50) : 0;
        float maxInclusive = nextAbilityToUse == 2 ? Math.Max(100 -  probabilityReduce, 50) : 100;
        Debug.LogFormat("Next Ability Probability Range: {0} - {1}", minInclusive, maxInclusive);
        float result = Random.Range(minInclusive, maxInclusive);
        short nextAbility = result <= 50 ? (short) 1 : (short) 2;
        if (nextAbilityToUse != nextAbility)
        {
            nextAbilityToUse = nextAbility;
            abilityUsedCount = 0;
        }
    }

    private float GetNextAbilityFireRadius()
    {
        return nextAbilityToUse == 1 ? controller.primaryFireRadius : controller.secondaryFireRadius;
    }

    private bool IsNextAbilityRaycast()
    {
        return nextAbilityToUse == 1
            ? controller.primaryFireType == HitType.HITSCAN
            : controller.secondaryFireType == HitType.HITSCAN;
    }

    private void ImpreciseLookAt()
    {
        float impreciseProbabilityStartRange = Math.Min(impreciseShotCount * 10, 50);
        float impreciseProbability = Random.Range(impreciseProbabilityStartRange, 100);
        bool isImprecise = impreciseProbability < 50;
        float imprecisionRate = 0;
        Debug.LogFormat("Imprecise Shot probability range: {0} - {1}", impreciseProbabilityStartRange, 100);
        if (isImprecise)
        {
            float imprecisionDistance = Random.Range(5, 15);
            imprecisionRate = Random.Range(0, 100) < 50 ? -imprecisionDistance : imprecisionDistance;
            impreciseShotCount += 1;
        }
        else
        {
            impreciseShotCount = 0;
        }
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 nextDirection = playerPosition - transform.position;
        Quaternion nextRotation = Quaternion.LookRotation(nextDirection);
        Vector3 nextEulerRotation = nextRotation.eulerAngles;
        nextEulerRotation.y += imprecisionRate;
        transform.rotation = Quaternion.Euler(nextEulerRotation);
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

    private bool IsCloseToCorner(Vector3 position)
    {
        foreach (Vector3 corner in corners)
        {
            if (Vector3.Distance(position, corner) < 5)
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 GoToNearestCorner()
    {
        Vector3 position = transform.position;
        Vector3 nearestCorner = Vector3.zero;
        float minDistance = float.PositiveInfinity;
        foreach (Vector3 corner in corners)
        {
            float distance = Vector3.Distance(position, corner);
            if (distance < 5)
            {
                break;
            }
            if (minDistance > distance)
            {
                minDistance = distance;
                nearestCorner = corner;
            }
        }

        return nearestCorner;
    }

    protected override void Update()
    {
        base.Update();
        
        attackCooldown -= Time.deltaTime;
        
        // If there are PowerUp in the map, follow him 
        if (nearestPowerUp != null)
        {
            setAgentDestination(nearestPowerUp.transform.position);
        }
        // If there are no PowerUp, but ability are ready, focus on attack
        else if ((controller.isPrimaryFireReady() || controller.isSecondaryFireReady()) && attackCooldown <= 0)
        {
            setAgentDestination(GameManager.Instance.player.transform.position);
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < GetNextAbilityFireRadius())
            {
                if (controller.isPrimaryFireReady() && nextAbilityToUse == 1 && isInputActive)
                {
                    if (IsNextAbilityRaycast())
                    {
                        ImpreciseLookAt();
                    }
                    FirePrimary();
                }
                else if (controller.isSecondaryFireReady() && nextAbilityToUse == 2 && isInputActive)
                {
                    if (IsNextAbilityRaycast())
                    {
                        ImpreciseLookAt();
                    }
                    FireSecondary();
                    
                }

                abilityUsedCount += 1;
                attackCooldown = 3;
                FindNextAbilityToUse();
            }
        }
        // Escape in other case
        else
        {
            if (latestPowerUp != null)
            {
                HandlePowerUpFollow(latestPowerUp);   
            }
            if (nearestPowerUp == null)
            {
                float maxFireRadius = Mathf.Max(GameManager.Instance.player.controller.primaryFireRadius, GameManager.Instance.player.controller.secondaryFireRadius);
                Vector3 position = transform.position;
                Vector3 directionAwayFromPlayer = position - GameManager.Instance.player.transform.position;
                Vector3 destination = position + directionAwayFromPlayer.normalized * maxFireRadius;

                // Verifying that final destination is valid
                if (NavMesh.SamplePosition(destination, out var navHit, maxFireRadius, NavMesh.AllAreas))
                {
                    if (!IsCloseToCorner(navHit.position))
                    {
                        setAgentDestination(navHit.position);   
                    }
                    else
                    {
                        GoToNearestCorner();
                    }
                }
                else
                {
                    if (IsCloseToCorner(transform.position))
                    {
                        GoToNearestCorner();
                    }
                    else
                    {
                        Vector3 bestValidPoint = FindBestValidPoint();
                        if (bestValidPoint != Vector3.zero)
                        {
                            setAgentDestination(bestValidPoint);
                        }   
                    }
                }   
            }
        }
    }
}
