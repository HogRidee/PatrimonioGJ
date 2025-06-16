using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _names;
    [SerializeField] private List<TextMeshProUGUI> _scores;
    [SerializeField] private GameObject _leaderboard;

    private string _publibLeaderboardKey =
        "017cb4012be14247c60716c994d16147f07c2018ca19572e26fa6e5aa8360aea";

    private void Start()
    {
        _leaderboard.SetActive(false);
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        // Q: How do I reference my own leaderboard?
        // A: Leaderboards.<NameOfTheLeaderboard>
        
        LeaderboardCreator.GetLeaderboard(_publibLeaderboardKey,((msg) =>
        {
            if (msg == null || msg.Length == 0)
            {
                Debug.Log("Error: No se pudo obtener el leaderboard. Verifica la conexión o la clave pública.");
                return;
            }
            
            int loopLenght = (msg.Length < _names.Count) ? msg.Length : _names.Count;
            for (int i = 0;  i < loopLenght; i++)
            {
                if (_names[i] == null || _scores[i] == null)
                {
                    Debug.Log($"Error: El TextMeshProUGUI en la posición {i} no está asignado.");
                    continue;
                }
                _names[i].text = msg[i].Username;
                _scores[i].text = msg[i].Score.ToString();
            }
            _leaderboard.SetActive(true);
        }));   
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        Debug.Log($"La entrada es: {username} - {score}");

        // Verificamos que el GUID esté autorizado
        if (!PlayerPrefs.HasKey("LC_USER_GUID") || string.IsNullOrEmpty(PlayerPrefs.GetString("LC_USER_GUID")))
        {
            Debug.LogWarning("No puedes subir score. El jugador no está autorizado.");
            return;
        }

        // Esto no es obligatorio si ya usaste SetUserGuid en el bootstrapper,
        // pero si quieres asegurar, puedes hacerlo aquí de nuevo.
        //LeaderboardCreator.SetUserGuid(PlayerPrefs.GetString("LC_USER_GUID"));

        LeaderboardCreator.UploadNewEntry(_publibLeaderboardKey, username, score, (msg) =>
        {
            //Debug.Log($"Entrada subida con éxito: {username} - {score}");
            GetLeaderboard();
        });

    }
}