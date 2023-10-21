using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager>
{
    public Player player;
    public Enemy enemy;
    public List<PickUp> spawnedPickUps;
    public float pickUpCooldown = 20;
    public float pickUpActualCooldown;

    private void OnEnable()
    {
        GameEvents.onPlayerHit += HandlePlayerHit;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerHit -= HandlePlayerHit;
    }

    private void HandlePlayerHit(float damage)
    {
        enemy.TakeDamage(damage);
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
            PickUp pickUpComponent = PickUp.CreateComponent(pickUpGameObject, 0);
            spawnedPickUps.Add(pickUpComponent);
            Instantiate(pickUpGameObject);
            GameEvents.PowerUpSpawn(pickUpComponent);
            pickUpActualCooldown = pickUpCooldown;
        }
    }
}