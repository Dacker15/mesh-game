using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager>
{
    public Player player;
    public Enemy enemy;
    public List<PowerUp> spawnedPickUps;
    public List<GameObject> powerUpSpawnPoints;
    public List<PowerUpData> spawnablePowerUps;
    public float pickUpCooldown = 20;
    private float pickUpActualCooldown;

    private void OnEnable()
    {
        GameEvents.onPlayerHit += HandlePlayerHit;
        GameEvents.onEnemyHit += HandleEnemyHit;
        GameEvents.onPowerUpPick += HandlePowerUpPick;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerHit -= HandlePlayerHit;
        GameEvents.onEnemyHit -= HandleEnemyHit;
        GameEvents.onPowerUpPick -= HandlePowerUpPick;
    }

    private void HandlePlayerHit(float damage)
    {
        float nextHealth = enemy.TakeDamage(damage);
        enemy.MakeInvulnerable();
        Debug.LogFormat("Enemy got hit, health is now {0}", nextHealth);
    }

    private void HandleEnemyHit(float damage)
    {
        float nextHealth = player.TakeDamage(damage);
        player.MakeInvulnerable();
        Debug.LogFormat("Player got hit, health is now {0}", nextHealth);
    }

    private void HandlePowerUpPick(PowerUp powerUp, Collider other)
    {
        spawnedPickUps.Remove(powerUp);
        PlayableEntity playableEntity = other.CompareTag("Player") ? player : enemy;
        switch (powerUp.type)
        {
            case PowerUpType.SPEED:
                StartCoroutine(playableEntity.SpeedPowerUp(powerUp.boostValue, powerUp.timeValue));
                break;
            case PowerUpType.DAMAGE:
                StartCoroutine(playableEntity.DamagePowerUp(powerUp.boostValue, powerUp.timeValue));
                break;
            case PowerUpType.HEAL:
                playableEntity.HealPowerUp(powerUp.boostValue);
                break;
            case PowerUpType.COOLDOWN:
                playableEntity.CooldownPowerUp(powerUp.boostValue);
                break;
            default:
                Debug.Log("What kind of power up you picked?");
                break;
        }
    }
    

    protected override void Awake()
    {
        base.Awake();
        pickUpActualCooldown = pickUpCooldown;
    }

    private GameObject FindPowerUpSpawn()
    {
        float average = float.NegativeInfinity;
        GameObject selectedSpawnPoint = null;
        for (int i = 0; i < powerUpSpawnPoints.Count; i++)
        {
            GameObject spawnPoint = powerUpSpawnPoints[i];
            Vector3 position = spawnPoint.transform.position;
            float enemyDistance = Vector3.Distance(position, enemy.transform.position);
            float playerDistance = Vector3.Distance(position, player.transform.position);
            float nextAverage = (enemyDistance * 2 + playerDistance) / 3;
            if (nextAverage > average)
            {
                average = nextAverage;
                selectedSpawnPoint = spawnPoint;
            }
        }

        return selectedSpawnPoint;
    }

    private void SpawnPowerUp()
    {
        float probability = Random.Range(0f, 100f);
        foreach (PowerUpData powerUpData in spawnablePowerUps)
        {
            if (probability > powerUpData.MinProbability && probability <= powerUpData.MaxProbability)
            {
                GameObject pickUpGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject selectedPowerUpSpawn = FindPowerUpSpawn();
                if (powerUpData.level == 1)
                {
                    pickUpGameObject.GetComponent<Renderer>().material.color = Color.green;   
                }
                if (powerUpData.level == 2)
                {
                    pickUpGameObject.GetComponent<Renderer>().material.color = Color.yellow;   
                }
                if (powerUpData.level == 3)
                {
                    pickUpGameObject.GetComponent<Renderer>().material.color = Color.red;   
                }
                PowerUp powerUpComponent = PowerUp.CreateComponent(pickUpGameObject, 0, powerUpData.Type, powerUpData.BoostValue, powerUpData.TimeValue);
                SphereCollider collider = pickUpGameObject.GetComponent<SphereCollider>();
                collider.isTrigger = true;
                spawnedPickUps.Add(powerUpComponent);
                pickUpGameObject.transform.position = selectedPowerUpSpawn.transform.position;
                Instantiate(pickUpGameObject);
                GameEvents.PowerUpSpawn(powerUpComponent);
                break;
            }
        }
    }

    private void Update()
    {
        pickUpActualCooldown -= Time.deltaTime;
        if (pickUpActualCooldown < 0)
        {
            pickUpActualCooldown = pickUpCooldown;
            SpawnPowerUp();
        }
    }
}