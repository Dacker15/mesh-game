using System.Collections;
using UnityEngine;

public class Player : PlayableEntity
{
    [SerializeField] private float speed;

    private bool isPaused;


    protected override void OnFirePrimarySuccess(float damage)
    {
        GameEvents.PlayerHit(damage);
    }

    protected override void OnFireSecondarySuccess(float damage)
    {
        GameEvents.PlayerHit(damage);
    }

    public override IEnumerator SpeedPowerUp(float value, float time)
    {
        float originalSpeed = speed;
        speed *= value;
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
        yield return null;
    }

    public override IEnumerator DamagePowerUp(float value, float time)
    {
        float originalPrimaryDamage = controller.primaryDamage;
        float originalSecondaryDamage = controller.secondaryDamage;
        controller.primaryDamage *= value;
        controller.secondaryDamage *= value;
        yield return new WaitForSeconds(time);
        controller.primaryDamage = originalPrimaryDamage;
        controller.secondaryDamage = originalSecondaryDamage;
        yield return null;
    }

    public override void HealPowerUp(float value)
    {
        Heal(value);
    }

    public override void CooldownPowerUp(float value)
    {
        float nextValue = 1 - value;
        controller.primaryActualCooldown *= nextValue;
        controller.secondaryActualCooldown *= nextValue;
    }

    private void HandlePlay()
    {
        isPaused = false;
    }

    private void HandlePause()
    {
        isPaused = true;   
    }

    protected override void Awake()
    {
        base.Awake();
        GameEvents.onPlay += HandlePlay;
        GameEvents.onPause += HandlePause;
    }

    protected override void Update()
    {
        base.Update();
        
        if (isTransformActive)
        {
            transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime * speed);   
        }

        if (isRotationActive)
        {
            transform.Rotate(0, Input.GetAxisRaw("Horizontal") * 0.75f, 0);   
        }

        if (Input.GetAxisRaw("Fire1") > 0 && controller.isPrimaryFireReady() && isInputActive && !isPaused)
        {
            FirePrimary();
        }
        else if (Input.GetAxisRaw("Fire2") > 0 && controller.isSecondaryFireReady() && isInputActive && !isPaused)
        {
            FireSecondary();
        }
    }

    private void OnDestroy()
    {
        GameEvents.onPlay -= HandlePlay;
        GameEvents.onPause -= HandlePause;
    }
}