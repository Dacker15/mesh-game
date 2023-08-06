using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;

    void Start()
    {
        health = 100;
    }

    public void pickDamage(float attackPoint)
    {
        health -= attackPoint;
        Debug.LogFormat("Next health: {0}", health);
        if (health <= 0)
        {
            Debug.Log("Enemy died");
        }
    }
}
