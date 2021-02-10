using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPickupText : MonoBehaviour
{
    Animation _animation;
    TextMeshProUGUI _text;
    
    void Awake()
    {
        _animation = GetComponent<Animation>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        FindObjectOfType<PickupSpawner>().PickupSpawned += OnPickupSpawned;
    }

    void OnPickupSpawned(Pickup pickup)
    {   
        pickup.PickedUp += OnPickupPickedUp;
        pickup.Destroyed += OnPickupDestroyed;
    }

    void OnPickupDestroyed(Pickup pickup)
    {
        pickup.PickedUp -= OnPickupPickedUp;
        pickup.Destroyed -= OnPickupDestroyed;
    }

    void OnPickupPickedUp(Pickup pickup)
    {
        _text.SetText(pickup.PickupText);
        _animation.Play();
    }
}
