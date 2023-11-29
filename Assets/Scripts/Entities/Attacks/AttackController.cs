using System;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public delegate void FireSuccess(float damage);
    public delegate void FireFail();
    public delegate void OnInputActiveChange(bool transformActive, bool rotationActive, bool inputActive);
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
    protected FireSuccess primaryFireSuccessCallback;
    protected FireFail primaryFireFailCallback;
    protected FireSuccess secondaryFireSuccessCallback;
    protected FireFail secondaryFireFailCallback;
    protected OnInputActiveChange inputChangeCallback;
    

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

    public void Initialize(OnInputActiveChange inputCallback, FireSuccess primaryFireSuccessCallback, FireSuccess secondaryFireSuccessCallback, FireFail primaryFireFailCallback = null, FireFail secondaryFireFailCallback = null)
    {
        this.inputChangeCallback = inputCallback;
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
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                return distance < fireRadius;   
            }
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

    protected void ResetInput()
    {
        inputChangeCallback(true, true, true);
    }
    
    public abstract void FirePrimaryInput();
    public abstract void FireSecondaryInput();
    protected abstract void OnPrimaryFireUpdate();
    protected abstract void OnSecondaryFireUpdate();
}
