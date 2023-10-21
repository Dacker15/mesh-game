using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int id { get; private set; }

    public static PickUp CreateComponent(GameObject where, int id)
    {
        PickUp component = where.AddComponent<PickUp>();
        component.id = id;
        return component;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.PowerUpPick(this, other);
        Destroy(gameObject);
    }
}
