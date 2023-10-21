using System.Collections;
using UnityEngine;

public abstract class PlayableEntity : LivingEntity
{
    [SerializeField] protected float primaryDamage;
    [SerializeField] protected float secondaryDamage;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;
    protected float primaryActualCooldown = 0;
    protected float secondaryActualCooldown = 0;

    public abstract void FirePrimary();
    public abstract void FireSecondary();

    public abstract IEnumerator SpeedPowerUp(float value, float time);
    public abstract IEnumerator DamagePowerUp(float value, float time);
    public abstract void HealPowerUp(float value);
    public abstract void CooldownPowerUp(float value);
    
    protected virtual void Update()
    {
        primaryActualCooldown -= Time.deltaTime;
        secondaryActualCooldown -= Time.deltaTime;
    }
}
