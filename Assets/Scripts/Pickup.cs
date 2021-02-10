using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public float ChanceToSpawn => _chanceToSpawn;

    protected AudioSource audioSource;
    protected abstract void ActivateEffect();

    [SerializeField] AudioClip _pickupSoundClip;
    [SerializeField] float _chanceToSpawn;
    [SerializeField] float _lifetime = 5f;

    float _timer;
    bool _hasBeenActivated;

    protected virtual void DisableSelf()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        var particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
            particles.Stop();
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    void Update()
    {
        if (_hasBeenActivated)
            return;
            
        _timer += Time.deltaTime;

        if (_timer > _lifetime)
            DisableSelf();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        _hasBeenActivated = true;
        DisableSelf();
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
