using UnityEngine;

public class PlayableArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEvents.PlayerOutside();
        }

        if (other.CompareTag("Enemy"))
        {
            GameEvents.EnemyOutside();
        }
    }
    
}
