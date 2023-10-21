using UnityEngine;

public sealed class GameEvents : MonoBehaviour
{
    public delegate void PlayerHitHandler(float damage);
    public static event PlayerHitHandler onPlayerHit;
    public delegate void EnemyHitHandler(float damage);
    public static event EnemyHitHandler onEnemyHit;

    public delegate void PowerUpSpawnHandler(PickUp pickUp);

    public static event PowerUpSpawnHandler onPowerUpSpawn;

    public delegate void PowerUpPickHandler(PickUp pickUp);

    public static event PowerUpPickHandler onPowerUpPick;

    public static void PlayerHit(float damage)
    {
        onPlayerHit?.Invoke(damage);
    }

    public static void EnemyHit(float damage)
    {
        onEnemyHit?.Invoke(damage);
    }

    public static void PowerUpSpawn(PickUp pickUp)
    {
        onPowerUpSpawn?.Invoke(pickUp);
    }
    
}