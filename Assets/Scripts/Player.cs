using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public float speed;
    public GameObject enemy;

    protected abstract void UseMainAbility();

    protected abstract void UseSecondaryAbility();

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

    protected void Update()
    {
        float xVariation = Input.GetAxisRaw("Horizontal");
        float yVariation = Input.GetAxisRaw("Vertical");
        
        gameObject.transform.position += new Vector3(speed * xVariation * Time.deltaTime, 0, speed * yVariation * Time.deltaTime);
        
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            UseMainAbility();
        } else if (Input.GetAxisRaw("Fire2") > 0)
        {
            UseSecondaryAbility();
        }
    }
}
