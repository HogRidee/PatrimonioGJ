using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NamePlayer02 : MonoBehaviour
{
    public static NamePlayer02 Instance;
    public string PlayerName;
    private void Awake()
    {
        if (NamePlayer02.Instance == null)
        {
            NamePlayer02.Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Nombre guardado: " + PlayerName);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}