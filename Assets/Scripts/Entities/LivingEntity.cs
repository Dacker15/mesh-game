using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float invulnerableCooldown;
    protected float invulnerableActualCooldown;

    public float TakeDamage(float damage)
    {
        if (invulnerableActualCooldown < 0)
        {
            health -= damage;  
        } 
        return health;
    }

    public float Heal(float heal)
    {
        health += heal;
        return health;
    }

    public void MakeInvulnerable()
    {
        invulnerableActualCooldown = invulnerableCooldown;
    }

    protected virtual void Update()
    {
        invulnerableActualCooldown -= Time.deltaTime;
    }
}
