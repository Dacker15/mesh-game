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
        primaryActualDistance = -1;
    }

    public override void FirePrimaryInput()
    {
        FirePrimary();
        inputChangeCallback(false, false, false);
        primaryActualDistance = 0;
        StartCoroutine(MovePlayerOnPrimaryFire());
    }

    public override void FireSecondaryInput()
    {
        inputChangeCallback(false, false, false);
        animator.Play(secondaryAnimation.name);
        StartCoroutine(WaitResetSecondaryCooldown());
    }

    protected override void OnPrimaryFireUpdate() { }

    protected override void OnSecondaryFireUpdate()
    {
        if (isSecondaryFireActive)
        {
            FireSecondary();   
        }
    }

    private IEnumerator MovePlayerOnPrimaryFire()
    {
        while (primaryActualDistance >= 0 && primaryActualDistance <= primaryFireRadius)
        {
            float frameDistance = primaryRotationSpeed * Time.deltaTime;
            transform.Translate(0, 0, frameDistance);
            transform.Rotate(frameDistance, 0, 0);
            primaryActualDistance += frameDistance;
            yield return null;
        }

        Vector3 prevRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, prevRotation.y, prevRotation.z);
        ResetPrimaryCooldown();
        ResetInput();
    }

    private IEnumerator WaitResetSecondaryCooldown()
    {
        isSecondaryFireActive = true;
        yield return new WaitForSeconds(secondaryDuration);
        isSecondaryFireActive = false;
        ResetSecondaryCooldown();
        ResetInput();
    }
}