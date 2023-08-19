using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private GameObject enemy;
    private float health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float mainAbilityDamage;
    [SerializeField]
    private float secondaryAbilityDamage;
    [SerializeField]
    private float mainAbilityCooldown;
    [SerializeField]
    private float secondaryAbilityCooldown;
    private float currentMainAbilityCooldown;
    private float currentSecondaryAbilityCooldown;

    protected virtual void UseMainAbility()
    {
        if (enemy == null) return;
        Enemy enemyBehaviour = enemy.GetComponent<Enemy>();
        enemyBehaviour.TakeDamage(mainAbilityDamage);
    }

    protected virtual void UseSecondaryAbility()
    {
        if (enemy == null) return;
        Enemy enemyBehaviour = enemy.GetComponent<Enemy>();
        enemyBehaviour.TakeDamage(secondaryAbilityDamage);
    }

    public void TakeDamage(float attackPoint)
    {
        health -= attackPoint;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = null;
        }
    }

    private void Start()
    {
        health = 100;
    }

    protected virtual void Update()
    {
        float xVariation = Input.GetAxisRaw("Horizontal");
        float yVariation = Input.GetAxisRaw("Vertical");
        
        gameObject.transform.position += new Vector3(speed * xVariation * Time.deltaTime, 0, speed * yVariation * Time.deltaTime);
        
        currentMainAbilityCooldown -= Time.deltaTime;
        currentSecondaryAbilityCooldown -= Time.deltaTime;
        
        if (Input.GetAxisRaw("Fire1") > 0 && currentMainAbilityCooldown <= 0)
        {
            UseMainAbility();
            currentMainAbilityCooldown = mainAbilityCooldown;
        } else if (Input.GetAxisRaw("Fire2") > 0 && currentSecondaryAbilityCooldown <= 0)
        {
            UseSecondaryAbility();
            currentSecondaryAbilityCooldown = secondaryAbilityCooldown;
        }
    }
}
