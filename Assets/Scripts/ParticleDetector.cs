using UnityEngine;

public class ParticleDetector : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Clear();
        }
    }
}
