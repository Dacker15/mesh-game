using System;
using System.Collections;
using UnityEngine;

public class CubeAttack : AttackController
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private AudioClip primaryRotationClip;
    [SerializeField] private AudioClip secondaryShotClip;
    private Animator animator;
    private CubeAnimation animationManager;
    private bool isPrimaryFireActive;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animationManager = GetComponentInChildren<CubeAnimation>();
        animationManager.onRotationStart += PlayPrimaryRotationSound;
    }

    private void OnDestroy()
    {
        animationManager.onRotationStart -= PlayPrimaryRotationSound;
    }

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

    private void PlayPrimaryRotationSound()
    {
        AudioSource.PlayClipAtPoint(primaryRotationClip, transform.position, 10);
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