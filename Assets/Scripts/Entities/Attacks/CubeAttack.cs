using System.Collections;
using UnityEngine;

public class CubeAttack : AttackController
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip primaryRotationClip;
    [SerializeField] private AudioClip secondaryShotClip;
    private bool isPrimaryFireActive;
    
    public override void FirePrimaryInput()
    {
        inputChangeCallback(true, false, false);
        StartCoroutine(WaitResetPrimaryCooldown());
        StartCoroutine(ManagePrimaryAnimationSound());
    }

    public override void FireSecondaryInput()
    {
        FireSecondary();
        inputChangeCallback(true, true, false);
        animator.Play(secondaryAnimation.name);
        AudioSource.PlayClipAtPoint(secondaryShotClip, transform.position, 1);
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

    private IEnumerator ManagePrimaryAnimationSound()
    {
        float reproductionSoundTime = primaryAnimation.length / 2;
        animator.Play(primaryAnimation.name);
        AudioSource.PlayClipAtPoint(primaryRotationClip, transform.position, 200);
        yield return new WaitForSeconds(reproductionSoundTime);
        AudioSource.PlayClipAtPoint(primaryRotationClip, transform.position, 200);
    }
    
    private IEnumerator WaitResetPrimaryCooldown()
    {
        isPrimaryFireActive = true;
        yield return new WaitForSeconds(primaryAnimation.length);
        isPrimaryFireActive = false;
        ResetPrimaryCooldown();
        ResetInput();
    }
    
}