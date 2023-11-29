using System.Collections;
using UnityEngine;

public class SphereAttack : AttackController
{
    [SerializeField] private float primaryEffectiveFireRadius;
    [SerializeField] private float primaryRotationSpeed;
    [SerializeField] private float secondaryDuration;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private AudioClip primaryShotClip;
    [SerializeField] private AudioClip secondaryRotationClip;
    [SerializeField] private AudioClip secondaryImpactClip;
    [SerializeField] private GameObject secondaryImpactArea;
    private float primaryActualDistance;
    private bool isSecondaryFireActive;
    private Animator animator;
    private SphereAnimation animationManager;

    protected override void Awake()
    {
        base.Awake();
        primaryActualDistance = -1;
        animator = GetComponentInChildren<Animator>();
        animationManager = GetComponentInChildren<SphereAnimation>();
        animationManager.onRotationStart += PlayRotationSound;
        animationManager.onImpactStart += HandleImpactStart;
    }
    
    protected override void FirePrimary()
    {
        if (Fire(HitType.MEELE, primaryEffectiveFireRadius))
        {
            primaryFireSuccessCallback(primaryDamage);
        }
        else
        {
            primaryFireFailCallback?.Invoke();
        }
    }

    public override void FirePrimaryInput()
    {
        inputChangeCallback(false, false, false);
        primaryActualDistance = 0;
        AudioSource.PlayClipAtPoint(primaryShotClip, transform.position, 10);
        StartCoroutine(HandlePrimaryFire());
    }

    public override void FireSecondaryInput()
    {
        inputChangeCallback(false, false, false);
        animator.Play(secondaryAnimation.name);
    }

    private void PlayRotationSound()
    {
        AudioSource.PlayClipAtPoint(secondaryRotationClip, transform.position);
    }

    private void HandleImpactStart()
    {
        StartCoroutine(HandleSecondaryFire());
    }

    protected override void OnPrimaryFireUpdate() { }

    protected override void OnSecondaryFireUpdate()
    {
        if (isSecondaryFireActive)
        {
            FireSecondary();   
        }
    }

    private IEnumerator HandlePrimaryFire()
    {
        while (primaryActualDistance >= 0 && primaryActualDistance <= primaryFireRadius)
        {
            float frameDistance = primaryRotationSpeed * Time.deltaTime;
            transform.Translate(0, 0, frameDistance);
            transform.Rotate(frameDistance, 0, 0);
            primaryActualDistance += frameDistance;
            FirePrimary();
            yield return null;
        }

        Vector3 prevRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, prevRotation.y, prevRotation.z);
        ResetPrimaryCooldown();
        ResetInput();
    }

    private IEnumerator HandleSecondaryFire()
    {
        AudioSource.PlayClipAtPoint(secondaryImpactClip, transform.position);
        secondaryImpactArea.SetActive(true);
        isSecondaryFireActive = true;
        yield return new WaitForSeconds(secondaryDuration);
        isSecondaryFireActive = false;
        secondaryImpactArea.SetActive(false);
        ResetSecondaryCooldown();
        ResetInput();
    }
}