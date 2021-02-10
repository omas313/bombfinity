using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public float ChanceToSpawn => _chanceToSpawn;

    [SerializeField] AudioClip _pickupSoundClip;
    [SerializeField] float _chanceToSpawn;

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

        var particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
            particles.Stop();

        PlayPickupSound();
        ActivateEffect();
    }

    void PlayPickupSound()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();    

        audioSource.PlayOneShot(_pickupSoundClip);
    }
}
