using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool IsActive => _isActive;


    [SerializeField] float _cooldown = 3f;
    [SerializeField] float _duration = 3f;
    

    Collider2D _collider;
    SpriteRenderer _spriteRenderer;
    float _cooldownTimer;
    float _durationTimer;

    bool _isActive;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Deactivate();
    }

    void Update()
    {

        if (_isActive)
            _durationTimer += Time.deltaTime;
        else
            _cooldownTimer += Time.deltaTime;

        if (_durationTimer > _duration)
            Deactivate();
        
        if (_cooldownTimer > _cooldown && Input.GetButtonDown("Shield"))
            Activate();
    }

    void Activate()
    {
        _cooldownTimer = 0f;
        _durationTimer = 0f;
        _isActive = true;
        
        _collider.enabled = true;
        _spriteRenderer.enabled = true;
    }

    void Deactivate()
    {
        _isActive = false;
        _cooldownTimer = 0f;
        _durationTimer = 0f;

        _collider.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
