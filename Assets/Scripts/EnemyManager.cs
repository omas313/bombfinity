using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action<int> EnemyDestroyed;
    public event Action AllEnemiedDestroyed;

    [SerializeField] EnemyController _enemyPrefab;
    [SerializeField] Collider2D _spawnAreaCollider;
    [SerializeField] Transform _parent;

    int _totalEnemyCount;

    void Start()
    {
    }

    void SpawnEnemy(int level)
    {
        var position = GetRandomSpawnPosition();
        var enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _parent).GetComponent<EnemyController>();
        enemy.SetStats(level);
        enemy.Destroyed += OnEnemyDestroyed;
        _totalEnemyCount++;
    }

    public void SpawnEnemiesForLevel(int level)
    {
        var count = level + (int)Math.Floor(level * 1.5f);
        for (int i = 0; i < count; i++)
            SpawnEnemy(level);
    }

    private Vector3 GetRandomSpawnPosition() => new Vector3(
            UnityEngine.Random.Range(_spawnAreaCollider.bounds.min.x, _spawnAreaCollider.bounds.max.x),
            UnityEngine.Random.Range(_spawnAreaCollider.bounds.min.y, _spawnAreaCollider.bounds.max.y),
            0f);

    void OnEnemyDestroyed(int score)
    {
        _totalEnemyCount--;
        EnemyDestroyed?.Invoke(score);

        if (_totalEnemyCount <= 0)
            AllEnemiedDestroyed?.Invoke();
    }

    [ContextMenu("spawn enemies 1")]
    void SpawnEnemyTest()
    {
        SpawnEnemiesForLevel(1);
    }

    [ContextMenu("spawn enemies 5")]
    void SpawnEnemyTest2()
    {
        foreach(var enemy in FindObjectsOfType<EnemyController>())
            Destroy(enemy.gameObject);

        SpawnEnemiesForLevel(5);
    }
}
