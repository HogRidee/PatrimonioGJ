using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _inputScore;
    
    public UnityEvent <string, int> submitScoreEvent;

    public void SubmitScore()
    {
        Debug.Log("Player name and Score: " + NamePlayer.InstanceNamePlayer.PlayerName + " -- " +  _inputScore.text);
        submitScoreEvent.Invoke(NamePlayer.InstanceNamePlayer.PlayerName, int.Parse(_inputScore.text));
    }
}
