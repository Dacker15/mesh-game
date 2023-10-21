using UnityEngine;

public class CubePlayer : Player
{
    public int angle;
    private bool isPlayerRotating;
    private int maxPlayerRotation;
    private int currentPlayerRotation = 0;
    [SerializeField]
    private GameObject weapon;

    public override void FirePrimary()
    {
        base.FirePrimary();
        isPlayerRotating = true;
    }

    public override void FireSecondary()
    {
        base.FireSecondary();
        Debug.Log("Secondary fire in Cube Player");
    }

    private void Start()
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
        }
    }
    
}
