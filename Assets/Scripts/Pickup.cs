using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip _pickupSoundClip;

    protected AudioSource audioSource;
    protected abstract void ActivateEffect();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        PlayPickupSound();
        ActivateEffect();
    }

    void PlayPickupSound()
    {
        audioSource.PlayOneShot(_pickupSoundClip);
    }
}
