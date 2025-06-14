using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputScore01;
    [SerializeField] private TextMeshProUGUI _inputScore02;
    [SerializeField] private Player_Movement _player;
    [SerializeField] private Player_Movement _player02;
    public UnityEvent <string, int> submitScoreEvent01;
    public UnityEvent <string, int> submitScoreEvent02;
    private bool _hasSubmitted01 = false;
    private bool _hasSubmitted02 = false;

    public void Update()
    {
        if (_player != null && _player.GetHealth() == 0 && !_hasSubmitted01)
        {
            //Debug.Log("Enviar Data al Leadeboard01");
            SubmitScore01();
            _hasSubmitted01 = true;
        }

        if (_player02 != null && _player02.GetHealth() == 0 && !_hasSubmitted02)
        {
            //Debug.Log("Enviar Data al Leadeboard02");
            SubmitScore02();
            _hasSubmitted02 = true;
        }
    }
    public void SubmitScore01()
    {
        //Debug.Log("Player name and Score 01: " + NamePlayer01.Instance.PlayerName + " -- " +  _inputScore01.text);
        string name = NamePlayer01.Instance.PlayerName;
        LeaderboardCreator.SetUserGuid(NamePlayer01.Instance.PlayerName + "_" + SystemInfo.deviceUniqueIdentifier);
        submitScoreEvent01.Invoke(name, int.Parse(_inputScore01.text));
    }

    public void SubmitScore02()
    {
        //Debug.Log("Player name and Score 02: " + NamePlayer02.Instance.PlayerName + " -- " + _inputScore02.text);
        string name = NamePlayer02.Instance.PlayerName;
        LeaderboardCreator.SetUserGuid(NamePlayer02.Instance.PlayerName + "_" + SystemInfo.deviceUniqueIdentifier);
        submitScoreEvent01.Invoke(name, int.Parse(_inputScore02.text));
    }
}
