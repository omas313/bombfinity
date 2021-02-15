using UnityEngine;

public class CloudPickup : Pickup
{
    protected override void ActivateEffect()
    {
        GameObject.FindGameObjectWithTag("Cloud").GetComponent<Animation>().Play();
    }
}
