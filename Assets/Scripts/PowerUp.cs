using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int id { get; private set; }

    public static PowerUp CreateComponent(GameObject where, int id)
    {
        PowerUp component = where.AddComponent<PowerUp>();
        component.id = id;
        return component;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.PowerUpPick(this, other);
        Destroy(gameObject);
    }
}
