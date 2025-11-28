using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public Button restartButton; // NEW: Reference to restart button
    
    [Header("Score")]
    public int score = 0;
    
    [Header("Win Condition")]
    public int winScore = 50;
    public int totalArrows = 5;
    
    private bool gameEnded = false;
    private int arrowsFinished = 0;
    
    void Awake()
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
    
    void Start()
    {
        UpdateScoreUI();
        
        // Hide restart button at start
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }
    }
    
    public void AddScore(int points)
    {
        if (gameEnded) return;
        
        score += points;
        UpdateScoreUI();
        Debug.Log("Score added! New score: " + score);
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    
    public void ResetScore()
    {
        if (gameEnded) return;
        
        score = 0;
        UpdateScoreUI();
        Debug.Log("Score Reset! Back to 0");
    }
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    
    public void OnArrowFinished()
    {
        if (gameEnded) return;
        
        arrowsFinished++;
        Debug.Log("Arrow finished! Total: " + arrowsFinished + "/" + totalArrows);
        Debug.Log("Current score: " + score);
        
        if (arrowsFinished >= totalArrows)
        {
            StartCoroutine(EndGameAfterDelay());
        }
    }
    
    IEnumerator EndGameAfterDelay()
    {
        Debug.Log("All arrows done! Score before delay: " + score);
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Score after delay: " + score);
        EndGame();
    }
    
    void EndGame()
    {
        gameEnded = true;
        
        Debug.Log("===== GAME OVER =====");
        Debug.Log("Final Score: " + score);
        
        if (score >= winScore)
        {
            ShowWinScreen();
        }
        else
        {
            ShowLoseScreen();
        }
        
        // Show restart button
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }
        
        Time.timeScale = 0f;
    }
    
    void ShowWinScreen()
    {
        Debug.Log("🎉 YOU WIN! 🎉");
        
        GameObject winTextObj = GameObject.Find("WinText");
        
        if (winTextObj != null)
        {
            TextMeshProUGUI winText = winTextObj.GetComponent<TextMeshProUGUI>();
            
            if (winText != null)
            {
                winText.text = "YOU WIN!";
                Color c = Color.yellow;
                c.a = 1f;
                winText.color = c;
            }
        }
    }
    
    void ShowLoseScreen()
    {
        Debug.Log("😢 GAME OVER - TRY AGAIN!");
        
        GameObject winTextObj = GameObject.Find("WinText");
        
        if (winTextObj != null)
        {
            TextMeshProUGUI winText = winTextObj.GetComponent<TextMeshProUGUI>();
            
            if (winText != null)
            {
                winText.text = "GAME OVER\nScore: " + score;
                Color c = Color.red;
                c.a = 1f;
                winText.color = c;
            }
        }
    }
    
    // NEW: Public method to restart game (called by button)
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        
        // Unfreeze time
        Time.timeScale = 1f;
        
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}