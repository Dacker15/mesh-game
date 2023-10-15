using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    private static readonly Lazy<GameManager> lazy = new Lazy<GameManager>(() => new GameManager());
    public static GameManager Instance { get { return lazy.Value; } }

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
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (Math.Abs(distance) < player.FireRadius)
        {
            enemy.TakeDamage(damage);
        }
    }
}