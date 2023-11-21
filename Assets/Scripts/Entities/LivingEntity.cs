using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float health;
    private float invulnerableActualCooldown;

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

    public void MakeInvulnerable(float invulnerableCooldown)
    {
        invulnerableActualCooldown = invulnerableCooldown;
    }

    protected virtual void Update()
    {
        invulnerableActualCooldown -= Time.deltaTime;
    }
}
