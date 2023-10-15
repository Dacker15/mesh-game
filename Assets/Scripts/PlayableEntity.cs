using UnityEngine;

public abstract class PlayableEntity : LivingEntity
{
    [SerializeField] protected float primaryDamage;
    [SerializeField] protected float secondaryDamage;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;
    protected float primaryActualCooldown;
    protected float secondaryActualCooldown;

    protected abstract void FirePrimary();
    protected abstract void FireSecondary();
    
    protected virtual void Start()
    {
        primaryActualCooldown = primaryCooldown;
        secondaryActualCooldown = secondaryCooldown;
    }
    
    protected virtual void Update()
    {
        primaryActualCooldown -= Time.deltaTime;
        secondaryActualCooldown -= Time.deltaTime;
    }
}
