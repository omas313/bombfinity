using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] float _aimSpeed = 1f;

    Transform _playerTransform;
    ParticleSystem _particleSystem;

    public int Score => CalculateScore();


    public void SetStats(int level)
    {
        _damage = UnityEngine.Random.Range(1, level);
        _aimSpeed = UnityEngine.Random.Range(1, level);

        var main = _particleSystem.main;
        main.startSpeed = Mathf.Clamp(UnityEngine.Random.Range(2f, level), 2f, 15f);
    }

    public void Release()
    {
        transform.parent = null;
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        StartCoroutine(DestroyAfterRelease());
    }

    IEnumerator DestroyAfterRelease()
    {
        var emission = _particleSystem.emission;
        emission.rateOverTime = 0f;

        yield return new WaitUntil(() => _particleSystem.particleCount == 0);
        Destroy(gameObject);
    }

    int CalculateScore() => (int)Math.Ceiling(2 * _damage + _aimSpeed);

    void Awake()
    {
        GetComponentInChildren<GunParticlesCollisionHandler>().HitPlayer += OnPlayerHit;
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    
    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void OnPlayerHit(PlayerController player)
    {
        player.TakeDamage(_damage);
    }

    void Update()
    {
        RotateToFacePlayer();        
    }

    void RotateToFacePlayer()
    {
        var vectorToPlayer = (_playerTransform.position - transform.position).normalized;
        var angle = Vector2.SignedAngle(Vector2.up, vectorToPlayer);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, angle)), _aimSpeed * Time.deltaTime);
    }
}
