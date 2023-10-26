using System;
using System.Collections;
using UnityEngine;

public class Player : PlayableEntity
{
    [SerializeField] private float speed;
    [SerializeField] public float fireRadius;

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

    protected override void Update()
    {
        base.Update();
        float xVariation = Input.GetAxisRaw("Horizontal");
        float yVariation = Input.GetAxisRaw("Vertical");
        
        gameObject.transform.position += new Vector3(speed * xVariation * Time.deltaTime, 0, speed * yVariation * Time.deltaTime);
        
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
