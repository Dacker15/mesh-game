using System.Collections;
using UnityEngine;

public class CubeAttack : AttackController
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;
    private bool isPrimaryFireActive;
    
    protected override void FirePrimaryInput()
    {
        animator.Play(primaryAnimation.name);
        StartCoroutine(ResetPrimaryCooldown());
    }

    protected override void FireSecondaryInput()
    {
        FireSecondary();
        ResetSecondaryCooldown();
        animator.Play(secondaryAnimation.name);
    }

    protected override void OnPrimaryFireUpdate()
    {
        if (isPrimaryFireActive)
        {
            FirePrimary();
        }
    }

    protected override void OnSecondaryFireUpdate() { }

    private IEnumerator ResetPrimaryCooldown()
    {
        isPrimaryFireActive = true;
        yield return new WaitForSeconds(primaryAnimation.length);
        isPrimaryFireActive = false;
    }
    
}