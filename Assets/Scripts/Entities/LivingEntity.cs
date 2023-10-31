using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [SerializeField] protected float health;

    public float TakeDamage(float damage)
    {
        health -= damage;
        return health;
    }

    public float Heal(float heal)
    {
        health += heal;
        return health;
    }
}
