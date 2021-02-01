using System;
using UnityEngine;

public class GunParticlesCollisionHandler : MonoBehaviour
{
    public event Action<PlayerController> HitPlayer;

    void OnParticleCollision(GameObject other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        HitPlayer?.Invoke(player);        
    }
}
