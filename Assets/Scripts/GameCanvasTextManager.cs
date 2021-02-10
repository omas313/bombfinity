using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameCanvasTextManager : MonoBehaviour
{
    public static GameCanvasTextManager Instance { get; private set;}

    [SerializeField] GameObject _instructions;
    [SerializeField] TextMeshProUGUI _youDiedText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] PopUpTextPanel _popupLevelText;

    int _score;

    public IEnumerator ShowPopup(string text)
    {
        yield return _popupLevelText.EnterWithText(text);
    }

    public void UpdateScore(int level)
    {
        _score += level * 10;
        _scoreText.SetText(_score.ToString());
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
        _instructions.SetActive(true);
        _youDiedText.gameObject.SetActive(false);

        var player = FindObjectOfType<PlayerController>();
        player.Died += OnPlayerDied;
        player.Ready += OnPlayerReady;

        EnemyManager.Instance.EnemyDestroyed += OnEnemyDestroyed;   
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
