using System.Collections;
using UnityEngine;

public class Player : PlayableEntity
{
    [SerializeField] private float speed;


    protected override void OnFirePrimarySuccess(float damage)
    {
        GameEvents.PlayerHit(damage);
    }

    protected override void OnFireSecondarySuccess(float damage)
    {
        GameEvents.PlayerHit(damage);
    }

    public override IEnumerator SpeedPowerUp(float value, float time)
    {
        float originalSpeed = speed;
        speed *= value;
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
        yield return null;
    }

    public override IEnumerator DamagePowerUp(float value, float time)
    {
        float originalPrimaryDamage = controller.primaryDamage;
        float originalSecondaryDamage = controller.secondaryDamage;
        controller.primaryDamage *= value;
        controller.secondaryDamage *= value;
        yield return new WaitForSeconds(time);
        controller.primaryDamage = originalPrimaryDamage;
        controller.secondaryDamage = originalSecondaryDamage;
        yield return null;
    }

    public override void HealPowerUp(float value)
    {
        Heal(value);
    }

    public override void CooldownPowerUp(float value)
    {
        controller.primaryActualCooldown /= value;
        controller.secondaryActualCooldown /= value;
    }

    protected virtual void Update()
    {
        transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime * speed);
        transform.Rotate(0, Input.GetAxisRaw("Horizontal") * 0.75f, 0);

        if (Input.GetAxisRaw("Fire1") > 0 && controller.isPrimaryFireReady())
        {
            FirePrimary();
            // primaryActualCooldown = primaryCooldown;
        }
        else if (Input.GetAxisRaw("Fire2") > 0 && controller.isSecondaryFireReady())
        {
            FireSecondary();
            // secondaryActualCooldown = secondaryCooldown;
        }
    }
}