using UnityEngine;

public class GameMusicManager : Singleton<GameMusicManager>
{
    [SerializeField] private AudioSource backgroundMusic;

    private void HandleMusicPlay()
    {
        backgroundMusic.Play();
    }

    private void HandleMusicPause()
    {
        backgroundMusic.Pause();
    }
    
    protected override void Awake()
    {
        base.Awake();
        GameEvents.onPlay += HandleMusicPlay;
        GameEvents.onPause += HandleMusicPause;
        GameEvents.onEnd += HandleMusicPause;
    }

    private void OnDestroy()
    {
        GameEvents.onPlay -= HandleMusicPlay;
        GameEvents.onPause -= HandleMusicPause;
        GameEvents.onEnd -= HandleMusicPause;
    }
}
