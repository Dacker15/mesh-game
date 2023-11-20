using UnityEngine;

public delegate void HitHandler(float damage);
public delegate void OutsideHandler();
public delegate void PowerUpSpawnHandler(PowerUp powerUp);
public delegate void PowerUpPickHandler(PowerUp powerUp, Collider other);

public static class GameEvents
{
    public static event HitHandler onPlayerHit;
    public static event HitHandler onEnemyHit;
    public static event OutsideHandler onPlayerOutside;
    public static event OutsideHandler onEnemyOutside;
    public static event PowerUpSpawnHandler onPowerUpSpawn;
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
    
    public static void PlayerOutside()
    {
        onPlayerOutside?.Invoke();
    }

    public static void EnemyOutside()
    {
        onEnemyOutside?.Invoke();
    }
}