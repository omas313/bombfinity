using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float _shootRate = 2f;
    [SerializeField] int _damagePerShot = 1;
    [SerializeField] float _aimSpeed = 1f;
    [SerializeField] [Range(3f, 15f)] float _bulletSpeed = 3f;

    public int KillScore => (int)Mathf.Ceil(_damagePerShot * _shootRate);

    ParticleSystem _particleSystem;
    private Transform _playerTransform;

    public void Release()
    {
        transform.parent = null;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        _particleSystem.Stop();
        StartCoroutine(DestroyAfterParticlesAreDead());
    }

    IEnumerator DestroyAfterParticlesAreDead()
    {
        yield return new WaitUntil(() => _particleSystem.particleCount == 0);
        Destroy(gameObject);
    }

    public void SetStats(int level)
    {
        _shootRate = UnityEngine.Random.Range(0.5f, level * 0.25f);
        _damagePerShot = UnityEngine.Random.Range(1, (int)(level * 0.25f));
        _aimSpeed = UnityEngine.Random.Range(0.5f, level * 0.5f);
        _bulletSpeed = Mathf.Clamp(UnityEngine.Random.Range(3f, 3f + level * 0.5f), 3f, 20f);

        InitParticleSystem();
    }

    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        GetComponentInChildren<ParticleCollisionEventHandler>().Collided += OnCollided;
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
        RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        if (_playerTransform == null)
            return;
            
        var vectorToPlayer = (_playerTransform.position - transform.position).normalized;
        var angle = Vector2.SignedAngle(Vector2.up, vectorToPlayer);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, angle), _aimSpeed * Time.deltaTime);
    }

    void OnCollided(GameObject other)
    {
        var hitTaker = other.GetComponentInChildren<ITakeHit>();
        if (hitTaker != null)
            hitTaker.TakeHit(_damagePerShot);
    }
}
