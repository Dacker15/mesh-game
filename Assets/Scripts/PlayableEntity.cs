using UnityEngine;

public abstract class PlayableEntity : LivingEntity
{
    [SerializeField] protected float primaryDamage;
    [SerializeField] protected float secondaryDamage;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;
    protected float primaryActualCooldown = 0;
    protected float secondaryActualCooldown = 0;

    protected abstract void FirePrimary();
    protected abstract void FireSecondary();
    
    protected virtual void Update()
    {
        primaryActualCooldown -= Time.deltaTime;
        secondaryActualCooldown -= Time.deltaTime;
    }
}
