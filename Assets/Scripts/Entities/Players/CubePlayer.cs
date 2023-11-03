using UnityEngine;

public class CubePlayer : Player
{
    private float primaryAttackDuration;
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;

    public override void FirePrimary()
    {
        animator.Play(primaryAnimation.name);
        primaryAttackDuration = primaryAnimation.length;
    }

    public override void FireSecondary()
    {
        base.FireSecondary();
        animator.Play(secondaryAnimation.name);
    }

    protected override void Update()
    {
        base.Update();
        if (primaryAttackDuration > 0)
        {
            if (Fire(primaryFireType, primaryFireRadius, "Enemy"))
            {
                GameEvents.PlayerHit(primaryDamage);
                primaryAttackDuration = 0;
            }
            primaryAttackDuration -= Time.deltaTime;
        }
    }
}
