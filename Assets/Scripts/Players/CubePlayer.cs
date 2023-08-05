using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : Player
{
    public int angle;
    private bool isPlayerRotating;
    private int maxPlayerRotation;
    private int currentPlayerRotation = 0;
    protected override void UseMainAbility()
    {
        isPlayerRotating = true;
        Debug.Log("Cube main ability");
    }

    protected override void UseSecondaryAbility()
    {
        Debug.Log("Cube secondary ability");
    }

    private void Start()
    {
        maxPlayerRotation = (360 * 3) / angle;
    }

    void Update()
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
        }
    }
    
}
