using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameOver gameOverPanel;

    private int alivePlayers = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        alivePlayers = players.Length;
        Debug.Log($"[UIManager] Escena cargada «{scene.name}»: alivePlayers = {alivePlayers}");
    }

    public void NotifyPlayerDeath()
    {
        alivePlayers = Mathf.Max(0, alivePlayers - 1);
        Debug.Log($"[UIManager] NotifyPlayerDeath → quedan {alivePlayers}");
        if (alivePlayers == 0)
        {
            gameOverPanel.ShowGameOver();
        }
    }
}
