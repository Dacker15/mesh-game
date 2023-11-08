using System;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public delegate void FireSuccess(float damage);
    public delegate void FireFail();
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
    private FireSuccess primaryFireSuccessCallback;
    private FireFail primaryFireFailCallback;
    private FireSuccess secondaryFireSuccessCallback;
    private FireFail secondaryFireFailCallback;

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

    public void Initialize(FireSuccess primaryFireSuccessCallback, FireSuccess secondaryFireSuccessCallback, FireFail primaryFireFailCallback = null, FireFail secondaryFireFailCallback = null)
    {
        this.primaryFireSuccessCallback = primaryFireSuccessCallback;
        this.primaryFireFailCallback = primaryFireFailCallback;
        this.secondaryFireSuccessCallback = secondaryFireSuccessCallback;
        this.secondaryFireFailCallback = secondaryFireFailCallback;
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

    protected virtual void FirePrimary()
    {
        if (Fire(primaryFireType, primaryFireRadius))
        {
            primaryFireSuccessCallback(primaryDamage);
        }
        else
        {
            primaryFireFailCallback?.Invoke();
        }
    }
    

    protected virtual void FireSecondary()
    {
        if (Fire(secondaryFireType, secondaryFireRadius))
        {
            secondaryFireSuccessCallback(secondaryDamage);
        }
        else
        {
            secondaryFireFailCallback?.Invoke();
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

    public float GetPrimaryFireRadius()
    {
        return primaryFireRadius;
    }

    public float GetSecondaryFireRadius()
    {
        return secondaryFireRadius;
    }
    
    public abstract void FirePrimaryInput();
    public abstract void FireSecondaryInput();
    protected abstract void OnPrimaryFireUpdate();
    protected abstract void OnSecondaryFireUpdate();
}
