using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    // event for which powerup spawned to show popup

    [SerializeField] Pickup[] _pickupsPrefabs;
    [SerializeField] int _levelToStartSpawning = 1;
    [SerializeField] float _spawnPeriod = 20f;

    Collider2D _collider;
    public float _timer;

    bool _isActivated;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();    
    }

    void Start()
    {
        LevelManager.Instance.LevelStarted += OnLevelStarted;
    }

    void OnLevelStarted(int level)
    {
        if (level >= _levelToStartSpawning)
            _isActivated = true;
    }

    void Update()
    {
        if (!_isActivated)
            return;

        _timer += Time.deltaTime;

        if (ShouldSpawnPickup())        
            SpawnRandomPickup();
    }

    bool ShouldSpawnPickup() => _timer > _spawnPeriod;

    void SpawnRandomPickup()
    {
        _timer = 0f;
        var pickupPrefab = GetRandomPickupPrefab();
        if (UnityEngine.Random.value < pickupPrefab.ChanceToSpawn)
            return;

        Instantiate(pickupPrefab, GetRandomPosition(), Quaternion.identity);    
    }

    Vector3 GetRandomPosition() => new Vector3(
        UnityEngine.Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
        UnityEngine.Random.Range(_collider.bounds.min.y, _collider.bounds.max.y),
        0f
    );

    Pickup GetRandomPickupPrefab() => _pickupsPrefabs[UnityEngine.Random.Range(0, _pickupsPrefabs.Length)];
}
