using UnityEngine;
using TMPro;
using System;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI _text;
    int _score;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        EnemyManager.Instance.EnemyDestroyed += OnEnemyDestroyed;   
        LevelManager.Instance.LevelCompleted += OnLevelCompleted;
    }

    void OnLevelCompleted(int level)
    {
        _score += level * 10;
        _text.SetText(_score.ToString());
    }

    void OnEnemyDestroyed(int points)
    {
        _score += points;
        _text.SetText(_score.ToString());
    }

}
