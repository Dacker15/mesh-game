using UnityEngine;

public delegate void OnAnimationFire();

public class CubeAnimation : MonoBehaviour
{
    public event OnAnimationFire onRotationStart;

    private void OnRotationStart()
    {
        onRotationStart?.Invoke();
    }
}
