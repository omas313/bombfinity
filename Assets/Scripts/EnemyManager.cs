using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set;}

    public event Action<int> EnemyDestroyed;
    public event Action AllEnemiesDestroyed;

    [SerializeField] Transform _parent;
    [SerializeField] Island _enemyPrefab;

    BoxCollider2D _collider;

    int _currentEnemyCount;


    int __level = 1;
    [ContextMenu("increment")]
    public void IncrementLevelAndSPawn()
    {
        __level++;
        SpawnEnemiess();
    }

    [ContextMenu("spawn enemies")]
    public void SpawnEnemiess()
    {
        foreach (var enemy in FindObjectsOfType<Island>())
            Destroy(enemy.gameObject);

        SpawnEnemiesForLevel(__level);
    }

    public void SpawnEnemiesForLevel(int level)
    {
        var count = level + (int)Math.Floor(Math.Sqrt(level) * 0.25f);
        StartCoroutine(SpawnEnemiesWithDelay(level, count));
    }

    IEnumerator SpawnEnemiesWithDelay(int level, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
            SpawnEnemy(level);
        }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);    
    }

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    void SpawnEnemy(int level)
    {
        var position = new Vector2(
            UnityEngine.Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
            UnityEngine.Random.Range(_collider.bounds.min.y, _collider.bounds.max.y)
        );

        var enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _parent).GetComponent<Island>();
        enemy.SetStats(level);
        enemy.Destroyed += OnEnemyDestroyed;
        _currentEnemyCount++;
    }

    void OnEnemyDestroyed(Island enemy)
    {
        EnemyDestroyed?.Invoke(enemy.KillScore);
        enemy.Destroyed -= OnEnemyDestroyed;
        _currentEnemyCount--;

        if (_currentEnemyCount <= 0)
            AllEnemiesDestroyed?.Invoke();
    }

}
