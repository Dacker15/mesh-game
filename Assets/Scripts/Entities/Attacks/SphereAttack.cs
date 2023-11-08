using System.Collections;
using UnityEngine;

public class SphereAttack : AttackController
{
    [SerializeField] private float primaryRotationSpeed;
    private float primaryActualDistance;
    [SerializeField] private float secondaryDuration;
    private bool isSecondaryFireActive;
    [SerializeField] private AnimationClip secondaryAnimation;
    [SerializeField] private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        primaryActualDistance = 0;
    }

    protected override void FirePrimaryInput()
    {
        FirePrimary();
        primaryActualDistance = 0;
    }

    protected override void FireSecondaryInput()
    {
        animator.Play(secondaryAnimation.name);
        StartCoroutine(ResetSecondaryCooldown());
    }

    protected override void OnPrimaryFireUpdate()
    {
        if (primaryActualDistance >= 0 && primaryActualDistance <= primaryFireRadius)
        {
            float frameDistance = primaryRotationSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * frameDistance);
            transform.Rotate(Vector3.forward * frameDistance);
            primaryActualDistance += frameDistance;
        }
    }

    protected override void OnSecondaryFireUpdate()
    {
        if (isSecondaryFireActive)
        {
            FireSecondary();   
        }
    }

    private IEnumerator ResetSecondaryCooldown()
    {
        isSecondaryFireActive = true;
        yield return new WaitForSeconds(secondaryDuration);
        isSecondaryFireActive = false;
    }
}