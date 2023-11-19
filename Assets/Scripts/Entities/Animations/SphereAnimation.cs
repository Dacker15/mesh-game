using UnityEngine;

public class SphereAnimation : MonoBehaviour
{
    public event OnAnimationFire onRotationStart;
    public event OnAnimationFire onImpactStart;

    private void OnRotationStart()
    {
        onRotationStart?.Invoke();
    }

    private void OnImpactStart()
    {
        onImpactStart?.Invoke();
    }
}
