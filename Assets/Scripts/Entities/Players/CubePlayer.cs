using UnityEngine;

public class CubePlayer : Player
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;

    public override void FirePrimary()
    {
        base.FirePrimary();
        animator.Play(primaryAnimation.name);
    }

    public override void FireSecondary()
    {
        base.FireSecondary();
        animator.Play(secondaryAnimation.name);
    }
    
}
