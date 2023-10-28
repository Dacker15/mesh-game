using System;
using System.Collections;
using UnityEngine;

public abstract class PlayableEntity : LivingEntity
{
    [SerializeField] protected float primaryDamage;
    [SerializeField] protected float secondaryDamage;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;
    [SerializeField] protected float primaryFireRadius;
    [SerializeField] protected float secondaryFireRadius;
    [SerializeField] protected HitType primaryFireType;
    [SerializeField] protected HitType secondaryFireType;
    protected float primaryActualCooldown = 0;
    protected float secondaryActualCooldown = 0;
    
    protected bool Fire(HitType type, float fireRadius, string tag)
    {
        if (type == HitType.MEELE)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag(tag);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            return Math.Abs(distance) < fireRadius;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up, Vector3.up + transform.forward * fireRadius, Color.black, 3, false);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward + Vector3.up, out var hit, fireRadius))
            {
                return hit.collider.gameObject.CompareTag(tag);
            }
        }

        return false;
    }
    
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
