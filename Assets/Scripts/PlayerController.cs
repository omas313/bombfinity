using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITakeHit, IHealth
{
    public event Action<int, int> HealthChanged;
    public event Action<Vector3> BombSpawned;
    public event Action Died;
    public event Action Ready;

    public float BombCooldown => _bombCooldown;

    [SerializeField] int _maxHealth = 100;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _deadZone = 0.1f;
    [SerializeField] Bomb _bombPrefab;
    [SerializeField] Transform _launchPoint;
    [SerializeField] float _bombCooldown = 3f;
    [SerializeField] GameObject _deathParticles;
    [SerializeField] AudioClip[] _hitSounds;
    [SerializeField] AudioClip _deathClip;

    bool CanFire => _bombCooldownTimer > _bombCooldown;
    bool IsShieldActive => _shield.IsActive;
    float _xThrow;
    float _yThrow;
    Rigidbody2D _rigidbody;
    AudioSource _audioSource;
    Shield _shield;
    float _bombCooldownTimer;
    int _currentHealth;

    public void TakeHit(int damage)
    {
        if (IsShieldActive)
        {
            _shield.TakeHit(damage);
            return;
        }

        PlayRandomHitSound();
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    public void AddHealth(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    void PlayRandomHitSound()
    {
        _audioSource.PlayOneShot(_hitSounds[UnityEngine.Random.Range(0, _hitSounds.Length)]);
    }

    void Die()
    {
        Died?.Invoke();
        if (_deathParticles != null)
            Instantiate(_deathParticles, transform.position, Quaternion.identity);
        

        _audioSource.PlayOneShot(_deathClip);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        enabled = false;
    }

    void Awake()
    {
        _shield = GetComponentInChildren<Shield>();
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();        
        _bombCooldownTimer = _bombCooldown;
        _currentHealth = _maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Ready?.Invoke();

        _xThrow = Input.GetAxis("Horizontal");
        _yThrow = Input.GetAxis("Vertical");

        _bombCooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && CanFire)
        {
            SpawnBomb();
            _bombCooldownTimer = 0f;
        }
    }

    void SpawnBomb()
    {
        Instantiate(_bombPrefab, _launchPoint.position, Quaternion.identity);
        BombSpawned?.Invoke(_launchPoint.position);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(_xThrow) < _deadZone)
            _xThrow = 0f;
        if (Mathf.Abs(_yThrow) < _deadZone)
            _yThrow = 0f;

        _rigidbody.velocity = new Vector2(_xThrow, _yThrow).normalized * _moveSpeed;
    }
}
