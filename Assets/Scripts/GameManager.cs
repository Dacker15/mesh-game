using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameManager : Singleton<GameManager>
{
    public Player player;
    public Enemy enemy;
    public AttackController playerController;
    public AttackController enemyController;
    [SerializeField] public List<PowerUp> spawnedPickUps;
    [SerializeField] public List<GameObject> powerUpSpawnPoints;
    [SerializeField] public List<PowerUpData> spawnablePowerUps;
    [SerializeField] public float pickUpCooldown;
    private float pickUpActualCooldown;
    [SerializeField] public float matchTime;
    [SerializeField] public float outsideDamage;
    [SerializeField] public float attackInvulnerableTime;
    [SerializeField] public float outsideInvulnerableTime;
    private bool isPaused;

    private static Vector3 playerSpawnPosition = new Vector3(0, 0.5f, -20);
    private static Vector3 enemySpawnPosition = new Vector3(0, 0.5f, 20);
    private static Quaternion playerSpawnRotation = Quaternion.Euler(0, 0, 0);
    private static Quaternion enemySpawnRotation = Quaternion.Euler(0, 180, 0);
    
    protected override void Awake()
    {
        base.Awake();
        pickUpActualCooldown = pickUpCooldown;
        
        GameEvents.onPlayerHit += HandlePlayerHit;
        GameEvents.onEnemyHit += HandleEnemyHit;
        GameEvents.onPowerUpPick += HandlePowerUpPick;
        GameEvents.onPlayerOutside += HandlePlayerOutside;
        GameEvents.onEnemyOutside += HandleEnemyOutside;
        GameEvents.onPlay += HandleTimeStart;
        GameEvents.onPause += HandleTimePause;
        GameEvents.onEnd += HandleGameEnd;

        GameObject playerObject;
        GameObject enemyObject;
        
        if (GameSettings.Instance.playerType == 0)
        {
            playerObject = Instantiate(GameSettings.Instance.cubePlayer, playerSpawnPosition, playerSpawnRotation);
            enemyObject = Instantiate(GameSettings.Instance.cubeEnemy, enemySpawnPosition, enemySpawnRotation);
        }
        else
        {
            playerObject = Instantiate(GameSettings.Instance.spherePlayer, playerSpawnPosition, playerSpawnRotation);
            enemyObject = Instantiate(GameSettings.Instance.sphereEnemy, enemySpawnPosition, enemySpawnRotation);
        }

        player = playerObject.GetComponent<Player>();
        enemy = enemyObject.GetComponent<Enemy>();
        playerController = playerObject.GetComponent<AttackController>();
        enemyController = enemyObject.GetComponent<AttackController>();
    }
    
    private void Update()
    {
        pickUpActualCooldown -= Time.deltaTime;
        matchTime -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                GameEvents.GamePlay();
            }
            else
            {
                GameEvents.GamePause();   
            }

            isPaused = !isPaused;
        }
        
        if (pickUpActualCooldown < 0)
        {
            pickUpActualCooldown = pickUpCooldown;
            SpawnPowerUp();
        }

        if (matchTime < 0)
        {
            GameEvents.GameEnd(EndGameWinner.NONE);
        }
    }

    private void OnDestroy()
    {
        GameEvents.onPlayerHit -= HandlePlayerHit;
        GameEvents.onEnemyHit -= HandleEnemyHit;
        GameEvents.onPowerUpPick -= HandlePowerUpPick;
        GameEvents.onPlayerOutside -= HandlePlayerOutside;
        GameEvents.onEnemyOutside -= HandleEnemyOutside;
        GameEvents.onPlay -= HandleTimeStart;
        GameEvents.onPause -= HandleTimePause;
        GameEvents.onEnd -= HandleGameEnd;
    }

    private void HandlePlayerHit(float damage)
    {
        float nextHealth = enemy.TakeDamage(damage);
        enemy.MakeInvulnerable(attackInvulnerableTime);
        Debug.LogFormat("Enemy got hit, health is now {0}", nextHealth);
        if (nextHealth <= 0)
        {
            GameEvents.GameEnd(EndGameWinner.PLAYER);
        }
    }

    private void HandleEnemyHit(float damage)
    {
        float nextHealth = player.TakeDamage(damage);
        player.MakeInvulnerable(attackInvulnerableTime);
        Debug.LogFormat("Player got hit, health is now {0}", nextHealth);
        if (nextHealth <= 0)
        {
            GameEvents.GameEnd(EndGameWinner.ENEMY);
        }
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

    private void HandleTimeStart()
    {
        Time.timeScale = 1;
    }

    private void HandleTimePause()
    {
        Time.timeScale = 0;
    }

    private void HandleGameEnd(EndGameWinner winner)
    {
        Time.timeScale = 0;
        switch (winner)
        {
            case EndGameWinner.PLAYER:
                player.gameObject.SetActive(false);
                break;
            case EndGameWinner.ENEMY:
                enemy.gameObject.SetActive(false);
                break;
        }
    }
    
    private void HandlePlayerOutside()
    {
        float nextHealth = player.TakeDamage(outsideDamage);
        player.MakeInvulnerable(outsideInvulnerableTime);
        player.transform.position = new Vector3(0, 0.5f, -20);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.LogFormat("Player is outside, health is now {0}", nextHealth);
    }
    private void HandleEnemyOutside()
    {
        float nextHealth = enemy.TakeDamage(outsideDamage);
        enemy.MakeInvulnerable(outsideInvulnerableTime);
        player.transform.position = new Vector3(0, 0.5f, 20);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        Debug.LogFormat("Enemy is outside, health is now {0}", nextHealth);
    }
}