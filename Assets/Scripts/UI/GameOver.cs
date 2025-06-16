using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject darkOverlay;

    private bool isGameOver = false;

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null && darkOverlay != null)
        {
            gameOverPanel.SetActive(true);
            darkOverlay.SetActive(true);
            Time.timeScale = 0f;
            isGameOver = true;
        }
        
    }

    public void OnRetry()
    {
        darkOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    public void OnLeaderboard()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LeaderBoard");
    }
}
