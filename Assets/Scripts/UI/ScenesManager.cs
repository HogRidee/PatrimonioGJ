using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour 
{
    /*
    public void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Don´t exist Scene: {sceneName}");
        }
    }
    */

    private void Start()
    {
        Debug.Log("El tiempo es: " + Time.timeScale);
    }
    public void Play_Cuy_Test_01()
    {
        SceneManager.LoadScene(2);
    }
    public void Play_Cuy_Test_02()
    {
        SceneManager.LoadScene(4);
    }
    public void Play_CinematicScene01()
    {
        SceneManager.LoadScene(7);
    }

    public void mainScene()
    {
        SceneManager.LoadScene(1);
    }
    public void enterScene()
    {
        SceneManager.LoadScene(0);
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
