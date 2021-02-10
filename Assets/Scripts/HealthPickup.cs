using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int _healthToAdd = 3;

    protected override void ActivateEffect()
    {
        FindObjectOfType<PlayerController>().AddHealth(_healthToAdd);
        Destroy(gameObject, 1f);
    }
}
