using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{
    public event Action<int, int> HealthChanged;
    public event Action Ready;
    public event Action Died;

    [SerializeField] int _maxHealth = 10;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _bombCooldown = 0.5f;
    [SerializeField] Transform _launchPoint;
    [SerializeField] GameObject _bombPrefab;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] AudioClip _hitSound;



    Rigidbody2D _rigidbody;
    Shield _shield;
    float _xThrow;
    float _yThrow;

    int _currentHealth;
    float _bombTimer;
    bool _isReady;

    AudioSource _audioSource;

    public void TakeDamage(int damage)
    {
        if (_shield != null && _shield.IsActive)
            return;

        // Debug.Log("taking dmage");

        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
        _audioSource.PlayOneShot(_hitSound);

        if (_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("player died");


        _rigidbody.simulated = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (_explosionPrefab != null)
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Died?.Invoke();
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _shield = GetComponentInChildren<Shield>();
        _audioSource = GetComponent<AudioSource>();

        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    void Update()
    {
        if (!_isReady && Input.GetButtonDown("Ready"))
        {
            _isReady = true;
            Ready?.Invoke();
        }

        _xThrow = Input.GetAxis("Horizontal");
        _yThrow = Input.GetAxis("Vertical");

        _bombTimer += Time.deltaTime;

        if (ShouldDropBomb())
        {
            DropBomb();
            _bombTimer = 0f;
        }
    }

    private bool ShouldDropBomb() => Input.GetButtonDown("Fire1") && _bombTimer > _bombCooldown;

    void DropBomb()
    {
        Instantiate(_bombPrefab, _launchPoint.position, Quaternion.identity);
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xThrow, _yThrow).normalized * _moveSpeed;    
    }
}
