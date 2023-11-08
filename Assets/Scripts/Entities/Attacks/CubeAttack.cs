using System.Collections;
using UnityEngine;

public class CubeAttack : AttackController
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;
    private bool isPrimaryFireActive;
    
    public override void FirePrimaryInput()
    {
        inputChangeCallback(true, false, false);
        animator.Play(primaryAnimation.name);
        StartCoroutine(WaitResetPrimaryCooldown());
    }

    public override void FireSecondaryInput()
    {
        FireSecondary();
        inputChangeCallback(true, true, false);
        animator.Play(secondaryAnimation.name);
        ResetSecondaryCooldown();
        ResetInput();
    }

    protected override void OnPrimaryFireUpdate()
    {
        if (isPrimaryFireActive)
        {
            FirePrimary();
        }
    }

    protected override void OnSecondaryFireUpdate() { }

    private IEnumerator WaitResetPrimaryCooldown()
    {
        isPrimaryFireActive = true;
        yield return new WaitForSeconds(primaryAnimation.length);
        isPrimaryFireActive = false;
        ResetPrimaryCooldown();
        ResetInput();
    }
    
}