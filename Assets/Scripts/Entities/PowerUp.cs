using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int id { get; private set; }
    public PowerUpType type { get; private set; }
    public float boostValue { get; private set; }
    public float timeValue { get; private set;  }

    public static PowerUp CreateComponent(GameObject where, int id, PowerUpType type, float boostValue, float timeValue)
    {
        PowerUp component = where.AddComponent<PowerUp>();
        component.id = id;
        component.type = type;
        component.boostValue = boostValue;
        component.timeValue = timeValue;
        return component;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.PowerUpPick(this, other);
        Destroy(gameObject);
    }
}
