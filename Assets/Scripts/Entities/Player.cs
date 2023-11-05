using System;
using System.Collections;
using UnityEngine;

public class Player : PlayableEntity
{
    protected bool isUserControlActive;
    [SerializeField] private float speed;

    public override void FirePrimary()
    {
        if (Fire(primaryFireType, primaryFireRadius, "Enemy"))
        {
            GameEvents.PlayerHit(primaryDamage);
        }
    }

    public override void FireSecondary()
    {
        if (Fire(secondaryFireType, secondaryFireRadius, "Enemy"))
        {
            GameEvents.PlayerHit(secondaryDamage);
        }
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
        float originalPrimaryDamage = primaryDamage;
        float originalSecondaryDamage = secondaryDamage;
        primaryDamage *= value;
        secondaryDamage *= value;
        yield return new WaitForSeconds(time);
        primaryDamage = originalPrimaryDamage;
        secondaryDamage = originalSecondaryDamage;
        yield return null;
    }

    public override void HealPowerUp(float value)
    {
        Heal(value);
    }

    public override void CooldownPowerUp(float value)
    {
        primaryActualCooldown /= value;
        secondaryActualCooldown /= value;
    }

    protected virtual void Awake()
    {
        isUserControlActive = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isUserControlActive)
        {
            transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime * speed);
            transform.Rotate(0, Input.GetAxisRaw("Horizontal") * 0.75f, 0);
        
            if (Input.GetAxisRaw("Fire1") > 0 && primaryActualCooldown <= 0)
            {
                FirePrimary();
                primaryActualCooldown = primaryCooldown;
            } else if (Input.GetAxisRaw("Fire2") > 0 && secondaryActualCooldown <= 0)
            {
                FireSecondary();
                secondaryActualCooldown = secondaryCooldown;
            }
        }
        
    }
}
