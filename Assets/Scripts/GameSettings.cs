using UnityEngine;

public class GameSettings : UnloadableSingleton<GameSettings>
{
    public GameObject cubePlayer;
    public GameObject cubeEnemy;
    public GameObject spherePlayer;
    public GameObject sphereEnemy;
    
    public int playerType;
}