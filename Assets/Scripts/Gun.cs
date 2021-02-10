using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int KillScore => (int)Mathf.Ceil(_damagePerShot * _shootRate);

    float _shootRate = 2f;
    int _damagePerShot = 1;
    float _aimSpeed = 1f;
    float _bulletSpeed = 3f;

    float _reloadPeriod = 5f;
    float _reloadTime = 3f;
    float _reloadTimer;
    bool _isReloading;

    float _angleRange = 45f;
    bool _isPlayerOutOfRange;

    ParticleSystem _particleSystem;
    Transform _playerTransform;
    Animation _animation;

    public void Release()
    {
        transform.parent = null;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        _particleSystem.Stop();
        StartCoroutine(DestroyAfterParticlesAreDead());
    }

    public void SetStats(int level)
    {
        SetRandomStats(level);
        InitParticleSystem();
    }

    void Start()
    {
        _animation = GetComponent<Animation>();
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        GetComponentInChildren<ParticleCollisionEventHandler>().Collided += OnCollided;
    }
    
    IEnumerator DestroyAfterParticlesAreDead()
    {
        yield return new WaitUntil(() => _particleSystem.particleCount == 0);
        Destroy(gameObject);
    }

    void SetRandomStats(int level)
    {
        _shootRate = UnityEngine.Random.Range(0.5f, level * 0.25f);
        _damagePerShot = UnityEngine.Random.Range(1, (int)(level * 0.25f));
        _aimSpeed = UnityEngine.Random.Range(0.5f, level * 0.5f);
        _bulletSpeed = Mathf.Clamp(UnityEngine.Random.Range(3f, 3f + level * 0.5f), 3f, 20f);
        _reloadPeriod = UnityEngine.Random.Range(6f - _damagePerShot, 6f + _damagePerShot);
        _reloadTime = UnityEngine.Random.Range(2f, 2f + _shootRate);
        _reloadTimer = UnityEngine.Random.value > 0.5 ? _reloadTime : 0f;
        _angleRange = level == 1 ? 45f : 45f + Mathf.Log10(level) * 20f;
    }


    void InitParticleSystem()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();

        var emission = _particleSystem.emission;
        emission.rateOverTime = _shootRate;

        var main = _particleSystem.main;
        main.startSpeed = _bulletSpeed;
        main.startLifetime = 10f / _bulletSpeed + 10f;
        main.startSize = Mathf.Clamp(UnityEngine.Random.Range(0.1f, _damagePerShot * 0.25f), 0.1f, 0.3f);
    }

    private void Update()
    {
        HandleReload();
        RotateTowardsPlayer();
    }

    void HandleReload()
    {
        if (_isReloading)
            return;

        _reloadTimer += Time.deltaTime;

        if (_reloadTimer > _reloadPeriod)
            StartCoroutine(Reload());    
    }

    IEnumerator Reload()
    {
        _isReloading = true;
        _reloadTimer = 0f;
        _particleSystem.Stop();
        _animation.Play();
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
        _particleSystem.Play();
    }

    void RotateTowardsPlayer()
    {
        if (_playerTransform == null || _isReloading)
            return;
            
        var vectorToPlayer = (_playerTransform.position - transform.position).normalized;
        var angle = Vector2.SignedAngle(Vector2.up, vectorToPlayer);

        if (Mathf.Abs(angle) > _angleRange)
        {
            angle = _angleRange * Mathf.Sign(angle);
            _isPlayerOutOfRange = true;
        }
        else
            _isPlayerOutOfRange = false;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, angle), _aimSpeed * Time.deltaTime);
    }

    void OnCollided(GameObject other)
    {
        var hitTaker = other.GetComponentInChildren<ITakeHit>();
        if (hitTaker != null)
            hitTaker.TakeHit(_damagePerShot);
    }
}
