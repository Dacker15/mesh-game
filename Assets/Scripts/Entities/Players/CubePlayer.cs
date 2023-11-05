using UnityEngine;

public class CubePlayer : Player
{
    private float primaryAttackDuration;
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;

    protected override void FirePrimary()
    {
        animator.Play(primaryAnimation.name);
        primaryAttackDuration = primaryAnimation.length;
    }

    protected override void FireSecondary()
    {
        base.FireSecondary();
        animator.Play(secondaryAnimation.name);
    }

    protected override void OnFirePrimarySuccess()
    {
        base.OnFirePrimarySuccess();
        primaryAttackDuration = -1;
    }

    protected override void Update()
    {
        base.Update();
        if (primaryAttackDuration >= 0)
        {
            base.FirePrimary();
            primaryAttackDuration -= Time.deltaTime;
        }
    }
}
