using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePlayer : MonoBehaviour
{
    public static NamePlayer InstanceNamePlayer;
    public string PlayerName;
    private void Awake()
    {
        // Verificar si ya existe una instancia persistente del Puntaje
        if (NamePlayer.InstanceNamePlayer == null)
        {
            NamePlayer.InstanceNamePlayer = this;
            DontDestroyOnLoad(this.gameObject);  // Este objeto no se destruye entre escenas
        }
        else
        {
            Destroy(this.gameObject);  // Destruye la nueva instancia si ya existe una
        }
    }
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
