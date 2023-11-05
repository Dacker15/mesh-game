using UnityEngine;

public class SpherePlayer : Player
{
    [SerializeField] private float primaryRotationSpeed;
    private float primaryActualDistance;
    [SerializeField] private float secondaryDuration;
    private float secondaryActualDuration;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;
    
    public override void FirePrimary()
    {
        isUserControlActive = false;
        primaryActualDistance = 0;
    }

    public override void FireSecondary()
    {
        isUserControlActive = false;
        secondaryActualDuration = secondaryDuration;
        animator.Play(secondaryAnimation.name);
    }

    protected override void Awake()
    {
        base.Awake();
        primaryActualDistance = -1;
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
            if (Fire(secondaryFireType, secondaryFireRadius, "Enemy"))
            {
                GameEvents.PlayerHit(secondaryDamage);
                secondaryActualDuration = 0;
            }
            secondaryActualDuration -= Time.deltaTime;
        }
        else
        {
            isUserControlActive = true;
        }
    }
}
