using System;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public delegate void FireSuccess(float damage);
    public delegate void FireFail();
    
    public float primaryDamage;
    public float secondaryDamage;
    public float primaryCooldown;
    public float secondaryCooldown;
    public float primaryFireRadius;
    public float secondaryFireRadius;
    public HitType primaryFireType;
    public HitType secondaryFireType;
    [SerializeField] protected string opponentTag;
    [NonSerialized] public float primaryActualCooldown;
    [NonSerialized] public float secondaryActualCooldown;
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
    
    public abstract void FirePrimaryInput();
    public abstract void FireSecondaryInput();
    protected abstract void OnPrimaryFireUpdate();
    protected abstract void OnSecondaryFireUpdate();
}
