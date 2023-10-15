using UnityEngine;

public sealed class GameEvents : MonoBehaviour
{
    public delegate void PlayerHitHandler(float damage);
    public static event PlayerHitHandler onPlayerHit;
    public delegate void EnemyHitHandler(float damage);
    public static event EnemyHitHandler onEnemyHit;

    public static void PlayerHit(float damage)
    {
        onPlayerHit?.Invoke(damage);
    }

    public static void EnemyHit(float damage)
    {
        onEnemyHit?.Invoke(damage);
    }

}