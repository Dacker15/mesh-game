using System;
using System.Collections;
using UnityEngine;

public abstract class PlayableEntity : LivingEntity
{
    public AttackController controller;

    protected virtual void Awake()
    {
        controller.Initialize(OnFirePrimarySuccess, OnFireSecondarySuccess);
    }

    protected void FirePrimary()
    {
        controller.FirePrimaryInput();
    }

    protected void FireSecondary()
    {
        controller.FireSecondaryInput();
    }

    protected abstract void OnFirePrimarySuccess(float damage);
    protected abstract void OnFireSecondarySuccess(float damage);
    public abstract IEnumerator SpeedPowerUp(float value, float time);
    public abstract IEnumerator DamagePowerUp(float value, float time);
    public abstract void HealPowerUp(float value);
    public abstract void CooldownPowerUp(float value);
}
