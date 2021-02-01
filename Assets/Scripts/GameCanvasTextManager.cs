using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameCanvasTextManager : MonoBehaviour
{
    [SerializeField] GameObject _instructions;
    [SerializeField] TextMeshProUGUI _youDiedText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _levelText;

    int _score;

    void Start()
    {
        _youDiedText.gameObject.SetActive(false);

        var player = FindObjectOfType<PlayerController>();
        player.Died += OnPlayerDied;
        player.Ready += OnPlayerReady;

        EnemyManager.Instance.EnemyDestroyed += OnEnemyDestroyed;   
        LevelManager.Instance.LevelCompleted += OnLevelCompleted;
        LevelManager.Instance.LevelStarted += OnLevelStarted;
    }

    void OnPlayerReady()
    {
        _instructions.gameObject.SetActive(false);

    }

    void OnPlayerDied()
    {
        _youDiedText.gameObject.SetActive(true);
        SaveHighScore();
    }

    void OnLevelCompleted(int level)
    {
        _score += level * 10;
        _scoreText.SetText(_score.ToString());
    }

    void OnLevelStarted(int level)
    {
        _levelText.SetText(level.ToString());
    }

    void OnEnemyDestroyed(int points)
    {
        _score += points;
        _scoreText.SetText(_score.ToString());
    }

    void SaveHighScore()
    {
        var highScoreKey = "HIGHSCORE";
        PlayerPrefs.SetInt(highScoreKey, Mathf.Max(_score, PlayerPrefs.GetInt(highScoreKey)));

        var levelKey = "LEVEL";
        PlayerPrefs.SetInt(levelKey, Mathf.Max(LevelManager.Instance.CurrentLevel, PlayerPrefs.GetInt(levelKey)));
    }
}
