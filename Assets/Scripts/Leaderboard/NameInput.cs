using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    [SerializeField] private bool _isPlayer01 = true;
    public void SavePlayerName()
    {
        if (nameInputField != null)
        {
            if (_isPlayer01)
            {
                if (NamePlayer01.Instance == null)
                    Debug.LogError("NamePlayer01 no ha sido inicializado aún.");

                NamePlayer01.Instance.PlayerName = nameInputField.text;
                //Debug.Log("Se guardo Player name 01: " + NamePlayer01.Instance.PlayerName);
            }
            else
            {
                if (NamePlayer02.Instance == null)
                    Debug.LogError("NamePlayer02 no ha sido inicializado aún.");
                NamePlayer02.Instance.PlayerName = nameInputField.text;
                //Debug.Log("Se guardo Player name 02: " + NamePlayer02.Instance.PlayerName);
            }
        }
    }

}
