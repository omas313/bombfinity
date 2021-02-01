using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ITakeHit
{
    public float Cooldown => _cooldown;
    public float CooldownTimer => _cooldownTimer;

    [SerializeField] float _cooldown = 3f;
    [SerializeField] float _duration = 3f;
    [SerializeField] AudioClip _soundClip;

    public bool IsActive => _collider.enabled && _spriteRenderer.enabled;

    Animator _animator;
    Collider2D _collider;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    
    float _cooldownTimer;
    float _durationTimer;

    public void TakeHit(int damage)
    {
        _animator?.SetTrigger("Flash");
    }


    
    void Activate()
    {
        _collider.enabled = true;
        _spriteRenderer.enabled = true;
        _cooldownTimer = 0f;
        _durationTimer = 0f;
        _audioSource?.PlayOneShot(_soundClip);
    }
    
    void Deactivate()
    {
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
        _cooldownTimer = 0f;
        _durationTimer = 0f;
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Deactivate();
    }

    void Update()
    {
        if (IsActive)
            HandleActiveShield();
        else
            HandleShieldCooldown();
    }

    private void HandleShieldCooldown()
    {
        _cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && _cooldownTimer > _cooldown)
            Activate();
    }

    private void HandleActiveShield()
    {
        _durationTimer += Time.deltaTime;

        if (_durationTimer > _duration)
            Deactivate();
    }
}
