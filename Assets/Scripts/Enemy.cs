using UnityEngine;

public class Enemy : PlayableEntity
{
    private GameObject player;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    protected override void FirePrimary()
    {
        Debug.Log("Primary fire fired");
    }

    protected override void FireSecondary()
    {
        Debug.Log("Secondary fire fired");
    }
}
