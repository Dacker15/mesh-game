using UnityEngine;

public class CubeAnimation : MonoBehaviour
{
    public event OnAnimationFire onRotationStart;
    public event OnAnimationFireAfter onWeaponOut;

    private void OnRotationStart()
    {
        onRotationStart?.Invoke();
    }

    private void OnWeaponOut(float seconds)
    {
        onWeaponOut?.Invoke(seconds);
    }
}
