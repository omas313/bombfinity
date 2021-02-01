using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIGameCanvasManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _spaceToStartText;

    int _score;

    void Start()
    {
        LevelManager.Instance.LevelStarted += OnLevelStarted;
        
        var enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.EnemyDestroyed += OnEnemyDestroyed;
        enemyManager.AllEnemiedDestroyed += OnAllEnemiesDestroyed;

        FindObjectOfType<PlayerController>().Ready += OnPlayerReady;
        FindObjectOfType<PlayerController>().Died += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        PlayerPrefs.SetInt("HS", Mathf.Max(PlayerPrefs.GetInt("HS"), _score));
    }

    private void OnPlayerReady()
    {
        _spaceToStartText.gameObject.SetActive(false);
    }

    void OnAllEnemiesDestroyed() => UpdateScore(LevelManager.Instance.CurrentLevel * 50);

    void OnEnemyDestroyed(int score) => UpdateScore(score);

    private void UpdateScore(int points)
    {
        _score += points;
        _scoreText.SetText(_score.ToString());
    }

    void OnLevelStarted(int level)
    {
        _levelText.SetText(level.ToString());
    }
}
