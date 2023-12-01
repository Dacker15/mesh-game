using System.Collections;
using UnityEngine;

public class CubeAttack : AttackController
{
    [SerializeField] private AnimationClip primaryAnimation;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private AudioClip primaryRotationClip;
    [SerializeField] private AudioClip secondaryShotClip;
    [SerializeField] private ParticleSystem secondaryShotParticle;
    private Animator animator;
    private CubeAnimation animationManager;
    private bool isPrimaryFireActive;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animationManager = GetComponentInChildren<CubeAnimation>();
        animationManager.onRotationStart += PlayPrimaryRotationSound;
        animationManager.onWeaponOut += HandleStartPrimaryFire;
    }

    private void OnDestroy()
    {
        animationManager.onRotationStart -= PlayPrimaryRotationSound;
        animationManager.onWeaponOut -= HandleStartPrimaryFire;
    }

    public override void FirePrimaryInput()
    {
        inputChangeCallback(true, false, false);
        animator.Play(primaryAnimation.name);
    }

    public override void FireSecondaryInput()
    {
        FireSecondary();
        secondaryShotParticle.Stop();
        secondaryShotParticle.Play();
        inputChangeCallback(true, true, false);
        animator.Play(secondaryAnimation.name);
        AudioSource.PlayClipAtPoint(secondaryShotClip, transform.position, 1);
        ResetSecondaryCooldown();
        ResetInput();
    }

    private void HandleStartPrimaryFire(float seconds)
    {
        StartCoroutine(HandlePrimaryFire(seconds));
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
    
    private IEnumerator HandlePrimaryFire(float seconds)
    {
        isPrimaryFireActive = true;
        yield return new WaitForSeconds(primaryAnimation.length - seconds);
        isPrimaryFireActive = false;
        ResetPrimaryCooldown();
        ResetInput();
    }
    
}