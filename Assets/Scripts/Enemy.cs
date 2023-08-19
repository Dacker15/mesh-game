using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float health;
    [SerializeField] 
    private float mainAbilityDamage;
    [SerializeField]
    private float mainAbilityCooldown;
    private float currentMainAbilityCooldown;

    void Start()
    {
        health = 100;
    }

    void Update()
    {
        currentMainAbilityCooldown -= Time.deltaTime;
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    protected void UseMainAbility()
    {
        if (player == null) return;
        player.GetComponent<Player>().TakeDamage(mainAbilityDamage);
    }

    public void TakeDamage(float attackPoint)
    {
        health -= attackPoint;
        Debug.LogFormat("Next health: {0}", health);
        if (health <= 0)
        {
            Debug.Log("Enemy died");
        }
    }
}
