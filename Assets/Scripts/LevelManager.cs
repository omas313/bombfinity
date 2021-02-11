using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}

    public event Action<int> LevelStarted;
    public event Action<int> LevelCompleted;
    public int CurrentLevel { get; private set; }
    public bool GameStarted => !_isWaitingForLevelStart;

    bool _isWaitingForLevelStart = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);    
    }

    void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        player.Ready += OnPlayerReady;
        player.Died += OnPlayerDied;
        EnemyManager.Instance.AllEnemiesDestroyed += OnAllEnemiesDestroyed;    
    }

    void OnPlayerDied()
    {
        StartCoroutine(LoadSceneAfterSeconds(3f));
    }
    IEnumerator LoadSceneAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }

    void OnPlayerReady()
    {
        if (_isWaitingForLevelStart)
            StartNewLevel(++CurrentLevel);
    }

    void OnAllEnemiesDestroyed()
    {
        _isWaitingForLevelStart = true;
        LevelCompleted?.Invoke(CurrentLevel);
        GameCanvasTextManager.Instance.UpdateScore(CurrentLevel);

        StartNewLevel(++CurrentLevel);
    }

    void StartNewLevel(int level)
    {
        LevelStarted?.Invoke(level);
        _isWaitingForLevelStart = false;
        GameCanvasTextManager.Instance.UpdateLevelText(CurrentLevel);
        StartCoroutine(ShowUIAndSpawnEnemies(level));
    }

    IEnumerator ShowUIAndSpawnEnemies(int level)
    {
        yield return GameCanvasTextManager.Instance.ShowPopup($"Level {level}");
        EnemyManager.Instance.SpawnEnemiesForLevel(level);
    }
}

