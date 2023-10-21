using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager>
{
    public Player player;
    public Enemy enemy;
    public List<PowerUp> spawnedPickUps;
    public float pickUpCooldown = 20;
    private float pickUpActualCooldown;

    private void OnEnable()
    {
        GameEvents.onPlayerHit += HandlePlayerHit;
        GameEvents.onPowerUpPick += HandlePowerUpPick;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerHit -= HandlePlayerHit;
        GameEvents.onPowerUpPick -= HandlePowerUpPick;
    }

    private void HandlePlayerHit(float damage)
    {
        enemy.TakeDamage(damage);
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
            case PowerUpType.COOLDOWN:
                playableEntity.HealPowerUp(powerUp.boostValue);
                break;
            case PowerUpType.HEAL:
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

    private void Update()
    {
        pickUpActualCooldown -= Time.deltaTime;
        if (pickUpActualCooldown < 0)
        {
            GameObject pickUpGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pickUpGameObject.transform.position = new Vector3(0, 0, 0);
            PowerUp powerUpComponent = PowerUp.CreateComponent(pickUpGameObject, 0, PowerUpType.DAMAGE, 5, 10);
            SphereCollider collider = pickUpGameObject.GetComponent<SphereCollider>();
            collider.isTrigger = true;
            spawnedPickUps.Add(powerUpComponent);
            Instantiate(pickUpGameObject);
            GameEvents.PowerUpSpawn(powerUpComponent);
            pickUpActualCooldown = pickUpCooldown;
        }
    }
}