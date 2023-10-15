using UnityEngine;

public abstract class Player : PlayableEntity
{
    [SerializeField] private float speed;
    private GameObject enemy;
    
    protected override void Update()
    {
        base.Update();
        float xVariation = Input.GetAxisRaw("Horizontal");
        float yVariation = Input.GetAxisRaw("Vertical");
        
        gameObject.transform.position += new Vector3(speed * xVariation * Time.deltaTime, 0, speed * yVariation * Time.deltaTime);
        
        if (Input.GetAxisRaw("Fire1") > 0 && primaryActualCooldown <= 0)
        {
            FirePrimary();
            primaryActualCooldown = primaryCooldown;
        } else if (Input.GetAxisRaw("Fire2") > 0 && secondaryActualCooldown <= 0)
        {
            FireSecondary();
            secondaryActualCooldown = secondaryCooldown;
        }
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = null;
        }
    }
}
