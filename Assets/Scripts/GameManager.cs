using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Enemy enemy;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } ;
    }

    // Update is called once per frame
    void Update()
    {
        enemy.GetComponent<NavMeshAgent>().destination = player.transform.position;
    }
}
