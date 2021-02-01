using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public event Action<int> LevelStarted;

    public static LevelManager Instance { get; private set;}

    public int CurrentLevel { get; private set; }


    EnemyManager _enemyManager;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _enemyManager = FindObjectOfType<EnemyManager>();
        _enemyManager.AllEnemiedDestroyed += OnAllEnemiesDestroyed;

        FindObjectOfType<PlayerController>().Ready += OnPlayerReady;
        FindObjectOfType<PlayerController>().Died += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        StartCoroutine(LoadMainMenuAfterDelay());
    }

    IEnumerator LoadMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    void OnPlayerReady()
    {
        StartLevel(++CurrentLevel);
    }

    void OnAllEnemiesDestroyed() => StartCoroutine(StartLevelAfterDelay());

    IEnumerator StartLevelAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        StartLevel(++CurrentLevel);
    }

    void StartLevel(int level)
    {
        LevelStarted?.Invoke(level);
        _enemyManager.SpawnEnemiesForLevel(level);
    }
}
