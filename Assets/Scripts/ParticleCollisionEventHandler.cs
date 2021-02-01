using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionEventHandler : MonoBehaviour
{
    public event Action<GameObject> Collided;

    void OnParticleCollision(GameObject other)
    {
        Collided?.Invoke(other);
    }
}
