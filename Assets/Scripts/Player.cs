using System;
using UnityEngine;

public class Player : PlayableEntity
{
    [SerializeField] private float speed;
    [SerializeField] private float fireRadius;

    protected override void FirePrimary()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (Math.Abs(distance) < fireRadius)
        {
            GameEvents.PlayerHit(primaryDamage);
        }
    }

    protected override void FireSecondary()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (Math.Abs(distance) < fireRadius)
        {
            GameEvents.PlayerHit(secondaryDamage);
        }
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
