public sealed class GameManager : Singleton<GameManager>
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
        enemy.TakeDamage(damage);
    }
}