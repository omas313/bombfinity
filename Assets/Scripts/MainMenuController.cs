using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _highscoreText;
    [SerializeField] CanvasGroup _highscoreGroup;

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelAfterDelay());
    }

    IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        var highscore = PlayerPrefs.GetInt("HS");
        if (highscore > 0)
        {
            _highscoreText.gameObject.SetActive(true);
            _highscoreText.SetText(highscore.ToString());
            _highscoreGroup.alpha = 1f;
        }
        else
        {
            _highscoreGroup.alpha = 0f;
        }
    }
}
