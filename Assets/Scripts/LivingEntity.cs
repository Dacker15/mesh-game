using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [SerializeField] protected float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Heal(float heal)
    {
        health += heal;
    }
}
