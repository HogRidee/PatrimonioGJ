using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NamePlayer01 : MonoBehaviour
{
    public static NamePlayer01 Instance;
    public string PlayerName;
    private void Awake()
    {
        if (NamePlayer01.Instance == null)
        {
            NamePlayer01.Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Nombre guardado: " + PlayerName);
        }
        else
        {
            Destroy(this.gameObject);  
        }
    }
}

