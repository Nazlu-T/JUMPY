using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    private CoinCollector coinCollector;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text sessionScoreText;
    [SerializeField] TMP_Text newHighScoreText; // "New High Score" mesajý için





    public static GameManager Instance { get; private set; }
    public bool IsGameActive { get; private set; } = true;
    


    void Start()
    {
        coinCollector = player.GetComponent<CoinCollector>();
        if (newHighScoreText != null) newHighScoreText.gameObject.SetActive(false); // Baþlangýçta gizle
        ScoreDisplay();
        AudioListener.volume = 1f;
        


    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void EndGame()
    {
        if (!IsGameActive) return;

        IsGameActive = false;

        // Get collected session coins
        // Get the session coins from CoinCollector
        int coinsCollected = coinCollector.EndGameAndResetSessionCoins();

        // Add the session coins to the total coins in GameDataManager
        GameDataManager.AddCoins(coinsCollected);

        // Update the UI to reflect the new total
        GameSharedUI.Instance.UpdateCoinsUIText();

        Debug.Log($"Session ended. Coins collected: {coinsCollected}. Total coins: {GameDataManager.GetCoins()}.");

        // High Score kontrolü
        int sessionScore = PointSystem.score;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (sessionScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", sessionScore);
            ShowNewHighScore(sessionScore);
        }
        else
        {
            ShowScores(sessionScore, highScore);
        }


        AudioListener.volume = 0f;

        // Show the end game panel
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }
    }
    private void ShowScores(int sessionScore, int highScore)
    {
        if (sessionScoreText != null) sessionScoreText.text = $"Score: {sessionScore}";
        if (highScoreText != null) highScoreText.text = $"High Score: {highScore}";
        sessionScoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        if (newHighScoreText != null) newHighScoreText.gameObject.SetActive(false);
    }

    private void ShowNewHighScore(int newHighScore)
    {
        if (newHighScoreText != null) newHighScoreText.text = $"NEW HIGH SCORE!\n{newHighScore}";
        sessionScoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        newHighScoreText.gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        // Unmute all sounds
        AudioListener.volume = 1f;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ScoreDisplay()
    {
        if (sessionScoreText != null) sessionScoreText.text = PointSystem.score.ToString();
    }

}

