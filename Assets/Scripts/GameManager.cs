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
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player picked up power up");
        } else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy picked up power up");
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
            PowerUp powerUpComponent = PowerUp.CreateComponent(pickUpGameObject, 0);
            SphereCollider collider = pickUpGameObject.GetComponent<SphereCollider>();
            collider.isTrigger = true;
            spawnedPickUps.Add(powerUpComponent);
            Instantiate(pickUpGameObject);
            GameEvents.PowerUpSpawn(powerUpComponent);
            pickUpActualCooldown = pickUpCooldown;
        }
    }
}