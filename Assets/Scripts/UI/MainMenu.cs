using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayGameMultiplayer()
    {
        SceneManager.LoadScene(3);
    }

    public void ShowLeaderboard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
