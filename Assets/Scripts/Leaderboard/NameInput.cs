using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void SavePlayerName()
    {
        if (nameInputField != null)
        {
            NamePlayer.InstanceNamePlayer.PlayerName = nameInputField.text;
            Debug.Log("Nombre guardado: " + NamePlayer.InstanceNamePlayer.PlayerName);
        }
    }
}
