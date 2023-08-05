using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : Player
{
    protected override void UseMainAbility()
    {
        Debug.Log("Cube main ability");
    }

    protected override void UseSecondaryAbility()
    {
        Debug.Log("Cube secondary ability");
    }
    
}
