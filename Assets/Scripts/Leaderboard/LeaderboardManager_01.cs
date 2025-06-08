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

    private string _publibLeaderboardKey = 
        "10984da475b628159780c45d3dde9e3b62d9993468ebf53c05e5254f80e29838";

    private void Start()
    {
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
        }));   
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        Leaderboards.CuySouls.UploadNewEntry(username, score, isSuccesful =>
        {
            if(isSuccesful)
                Debug.Log($"Entrada subida con éxito: {username} - {score}");
                GetLeaderboard();
            
        });
        LeaderboardCreator.ResetPlayer();

        //LeaderboardCreator.UploadNewEntry(_publibLeaderboardKey,username,score,((msg) =>
        //{
        //if(System.Array.IndexOf(badWords,name)!=-1) return;
        //Debug.Log($"Entrada subida con éxito: {username} - {score}"); // Se llama solo una vez después de subir la entrada
        //GetLeaderboard();
        //}));   
    }
}