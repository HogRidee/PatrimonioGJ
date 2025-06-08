using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour 
{
    public void Play()
    {
        SceneManager.LoadScene(2);
    }
    public void Controls()
    {
        SceneManager.LoadScene(2);
    }
    public void mainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Leaderboard()
    {
        SceneManager.LoadScene(5);
    }
    public void Exit()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
