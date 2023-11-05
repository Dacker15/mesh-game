using UnityEngine;

public class SpherePlayer : Player
{
    [SerializeField] private float primaryRotationSpeed;
    private float primaryActualDistance;
    [SerializeField] private float secondaryDuration;
    private float secondaryActualDuration;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;
    
    protected override void FirePrimary()
    {
        base.FirePrimary();
        isUserControlActive = false;
        primaryActualDistance = 0;
    }

    protected override void FireSecondary()
    {
        isUserControlActive = false;
        secondaryActualDuration = secondaryDuration;
        animator.Play(secondaryAnimation.name);
    }

    protected override void OnFireSecondarySuccess()
    {
        base.OnFireSecondarySuccess();
        secondaryActualDuration = -1;
    }

    protected override void Awake()
    {
        base.Awake();
        primaryActualDistance = -1;
        secondaryActualDuration = -1;
    }

    protected override void Update()
    {
        base.Update();

        secondaryActualCooldown -= Time.deltaTime;
        
        if (primaryActualDistance >= 0 && primaryActualDistance <= primaryFireRadius)
        {
            float frameDistance = primaryRotationSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * frameDistance);
            transform.Rotate(Vector3.forward * frameDistance);
            primaryActualDistance += frameDistance;
        }
        else if (secondaryActualDuration >= 0)
        {
            base.FireSecondary();
            secondaryActualDuration -= Time.deltaTime;
        }
        else
        {
            isUserControlActive = true;
        }
    }
}
