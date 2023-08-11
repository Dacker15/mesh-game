using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float health;

    void Start()
    {
        health = 100;
        gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(0, 0, 0);
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
