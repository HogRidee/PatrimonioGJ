using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Score02 : MonoBehaviour
{
    public static Score02 Instance;

    [SerializeField] private GameObject _scoreDisplay;
    private TextMeshProUGUI _scoreText;
    [SerializeField] private Player_Movement _player;
    private float _currentScore;
    private bool _isCounting = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeScoreDisplay();
        StartCoroutine(ScoreCounterRoutine());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeScoreDisplay()
    {
        if (_scoreDisplay != null)
        {
            _scoreText = _scoreDisplay.GetComponent<TextMeshProUGUI>();
            if (_scoreText != null)
            {
                _scoreText.text = _currentScore.ToString("0");
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component missing on score display");
            }
        }
        else
        {
            Debug.LogWarning("Score display GameObject reference not set");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeScoreDisplay();
        //FindPlayerReference();
    }

    private void FindPlayerReference()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Debug.Log("El jugador ha sido encontrado");
            _player = playerObject.GetComponent<Player_Movement>();
        }
    }

    private void Update()
    {
        if (_scoreText != null)
        {
            _scoreText.text = _currentScore.ToString("0");
        }
        if (_player != null && _player.GetHealth() <= 0)
        {
            _isCounting = false;
        }
    }

    public void AddPoints(float points)
    {
        if (_isCounting)
        {
            _currentScore += points;
        }
    }

    private IEnumerator ScoreCounterRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (_isCounting)
            {
                AddPoints(1f);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}