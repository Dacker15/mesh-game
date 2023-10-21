using UnityEngine;

public sealed class GameEvents : MonoBehaviour
{
    public delegate void PlayerHitHandler(float damage);
    public static event PlayerHitHandler onPlayerHit;
    public delegate void EnemyHitHandler(float damage);
    public static event EnemyHitHandler onEnemyHit;

    public delegate void PowerUpSpawnHandler(PowerUp powerUp);

    public static event PowerUpSpawnHandler onPowerUpSpawn;

    public delegate void PowerUpPickHandler(PowerUp powerUp, Collider other);

    public static event PowerUpPickHandler onPowerUpPick;

    public static void PlayerHit(float damage)
    {
        onPlayerHit?.Invoke(damage);
    }

    public static void EnemyHit(float damage)
    {
        onEnemyHit?.Invoke(damage);
    }

    public static void PowerUpSpawn(PowerUp powerUp)
    {
        onPowerUpSpawn?.Invoke(powerUp);
    }

    public static void PowerUpPick(PowerUp powerUp, Collider collider)
    {
        onPowerUpPick?.Invoke(powerUp, collider);
    }
    
}