using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePlayer : Player
{
    [SerializeField] private float primaryRotationSpeed;
    private float primaryActualDistance;
    
    public override void FirePrimary()
    {
        isUserControlActive = false;
        primaryActualDistance = 0;
    }

    public override void FireSecondary()
    {
        Debug.Log("Secondary fire in Sphere Player");
    }

    protected override void Awake()
    {
        base.Awake();
        primaryActualDistance = -1;
    }

    protected override void Update()
    {
        base.Update();

        if (primaryActualDistance >= 0 && primaryActualDistance <= primaryFireRadius)
        {
            float frameDistance = primaryRotationSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * frameDistance);
            transform.Rotate(Vector3.forward * frameDistance);
            primaryActualDistance += frameDistance;
        }
        else
        {
            isUserControlActive = true;
        }
    }
}
