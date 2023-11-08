using System.Collections;

public abstract class PlayableEntity : LivingEntity
{
    public AttackController controller;
    protected bool isTransformActive;
    protected bool isRotationActive;
    protected bool isInputActive;

    protected virtual void Awake()
    {
        controller.Initialize(OnInputActiveChange, OnFirePrimarySuccess, OnFireSecondarySuccess);
        isTransformActive = true;
        isRotationActive = true;
        isInputActive = true;
    }

    protected void FirePrimary()
    {
        controller.FirePrimaryInput();
    }

    protected void FireSecondary()
    {
        controller.FireSecondaryInput();
    }

    private void OnInputActiveChange(bool transformActive, bool rotationActive, bool inputActive)
    {
        isTransformActive = transformActive;
        isRotationActive = rotationActive;
        isInputActive = inputActive;
    }

    protected abstract void OnFirePrimarySuccess(float damage);
    protected abstract void OnFireSecondarySuccess(float damage);
    public abstract IEnumerator SpeedPowerUp(float value, float time);
    public abstract IEnumerator DamagePowerUp(float value, float time);
    public abstract void HealPowerUp(float value);
    public abstract void CooldownPowerUp(float value);
}
