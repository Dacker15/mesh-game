using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : Player
{
    public int angle;
    private bool isPlayerRotating;
    private int maxPlayerRotation;
    private int currentPlayerRotation = 0;
    [SerializeField]
    private GameObject weapon;
    protected override void UseMainAbility()
    {
        base.UseMainAbility();
        isPlayerRotating = true;
        weapon.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Cube main ability");
    }

    protected override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        Debug.Log("Cube secondary ability");
    }

    protected void Start()
    {
        maxPlayerRotation = (360 * 3) / angle;
    }

    protected override void Update()
    {
        base.Update();
        if (isPlayerRotating && currentPlayerRotation < maxPlayerRotation)
        {
            gameObject.transform.Rotate(new Vector3(0, -1, 0), angle);
            currentPlayerRotation++;
        }
        else
        {
            isPlayerRotating = false;
            currentPlayerRotation = 0;
            gameObject.transform.rotation = Quaternion.identity;
            weapon.GetComponent<BoxCollider>().enabled = true;
        }
    }
    
}
