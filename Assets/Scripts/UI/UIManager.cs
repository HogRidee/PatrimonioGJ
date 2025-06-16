using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameOver gameOverPanel;

    private int alivePlayers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            alivePlayers = players.Length;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NotifyPlayerDeath()
    {
        alivePlayers = Mathf.Max(0, alivePlayers - 1);
        if (alivePlayers == 0)
        {
            gameOverPanel.ShowGameOver();
        }
    }
}
