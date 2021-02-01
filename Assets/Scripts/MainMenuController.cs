using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] CanvasGroup _highScoreGroup;
    [SerializeField] TextMeshProUGUI _highScoreText;

    bool _isLoading;

    public void LoadGame(float delay)
    {
        if (_isLoading)
            return;
        
        _isLoading = true;
        StartCoroutine(LoadGameAfterSeconds(delay));
    }

    IEnumerator LoadGameAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(1);
    }

    void Start()
    {
        SetHighScore();
    }

    void SetHighScore()
    {
        _highScoreGroup.alpha = 0f;

        var score = PlayerPrefs.GetInt("HIGHSCORE");
        if (score > 0)
        {
            _highScoreText.SetText(score.ToString());
            _highScoreGroup.alpha = 1f;
        }
    }

    #region Debugging

    [ContextMenu("Clear highscore")]
    void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("HIGHSCORE");
    }

    [ContextMenu("Set highscore 10")]
    void SetHighScoreTo10()
    {
        PlayerPrefs.SetInt("HIGHSCORE", 10);
    }

    [ContextMenu("Set highscore 50")]
    void SetHighScoreTo50()
    {
        PlayerPrefs.SetInt("HIGHSCORE", 50);
    }

    #endregion

}
