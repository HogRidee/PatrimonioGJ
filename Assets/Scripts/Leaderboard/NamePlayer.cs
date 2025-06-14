using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NamePlayer : MonoBehaviour
{
    public string PlayerName;

    protected virtual void Awake()
    {
        Debug.Log("Nombre inicial: " + PlayerName);
        DontDestroyOnLoad(gameObject);
    }
}
