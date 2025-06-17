using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private GameObject ui;

    [Header("Text Copying")]
    [SerializeField] private List<TextMeshProUGUI> sourceScores;

    [SerializeField] private List<TextMeshProUGUI> targetScores;


    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        darkOverlay.SetActive(true);
        Time.timeScale = 0f;
        int count = Mathf.Min(sourceScores.Count, targetScores.Count);
        bool multiple = count > 1;
        for (int i = 0; i < count; i++)
        {
            string val = sourceScores[i].text;
            string prefix;
            if (multiple)
                prefix = $"Puntuación jugador {i + 1}";
            else
                prefix = "Puntuación";
            targetScores[i].text = $"{prefix}: {val}";
            targetScores[i].gameObject.SetActive(true);
        }
    }

    public void OnRetry()
    {
        darkOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
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
