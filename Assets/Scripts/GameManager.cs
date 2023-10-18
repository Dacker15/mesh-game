public sealed class GameManager : Singleton<GameManager>
{
    public Player player;
    public Enemy enemy;
    public int PowerUpCount { get; private set; } = 0;

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
}