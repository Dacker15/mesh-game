using System;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    protected delegate void FireSuccess(float damage);
    protected delegate void FireFail();
    [SerializeField] protected float primaryDamage;
    [SerializeField] protected float secondaryDamage;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;
    [SerializeField] protected float primaryFireRadius;
    [SerializeField] protected float secondaryFireRadius;
    [SerializeField] protected HitType primaryFireType;
    [SerializeField] protected HitType secondaryFireType;
    [SerializeField] protected string opponentTag;
    private float primaryActualCooldown;
    private float secondaryActualCooldown;

    protected virtual void Awake()
    {
        primaryActualCooldown = 0;
        secondaryActualCooldown = 0;
    }

    protected void Update()
    {
        primaryActualCooldown -= Time.deltaTime;
        secondaryActualCooldown -= Time.deltaTime;
        OnPrimaryFireUpdate();
        OnSecondaryFireUpdate();
    }

    protected bool Fire(HitType type, float fireRadius)
    {
        if (type == HitType.MEELE)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag(opponentTag);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            return Math.Abs(distance) < fireRadius;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * fireRadius, Color.black, 10, false);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, fireRadius))
            {
                return hit.collider.CompareTag(opponentTag);
            }
        }

        return false;
    }

    protected virtual void FirePrimary(FireSuccess fireSuccessCallback, FireFail fireFailCallback = null)
    {
        if (Fire(primaryFireType, primaryFireRadius))
        {
            fireSuccessCallback(primaryDamage);
        }
        else
        {
            fireFailCallback();
        }
    }
    

    protected virtual void FireSecondary(FireSuccess fireSuccessCallback, FireFail fireFailCallback = null)
    {
        if (Fire(secondaryFireType, secondaryFireRadius))
        {
            fireSuccessCallback(secondaryDamage);
        }
        else
        {
            fireFailCallback();
        }
    }
    
    public virtual bool isPrimaryFireReady()
    {
        return primaryActualCooldown <= 0;
    }
    
    public virtual bool isSecondaryFireReady()
    {
        return secondaryActualCooldown <= 0;
    }
    
    public virtual void ResetPrimaryCooldown()
    {
        primaryActualCooldown = primaryCooldown;
    }
    
    public virtual void ResetSecondaryCooldown()
    {
        secondaryActualCooldown = secondaryCooldown;
    }
    
    protected abstract void FirePrimaryInput();
    protected abstract void FireSecondaryInput();
    protected abstract void OnPrimaryFireUpdate();
    protected abstract void OnSecondaryFireUpdate();
}
