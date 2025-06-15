using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHealthDisplay : MonoBehaviour
{
    [Header("Hearts Setup")]
    [Tooltip("Drag here the hearts icons")]
    public List<Image> hearts;

    [Tooltip("Full Heart Sprite")]
    public Sprite heartFull;
    [Tooltip("Empty Heart Sprite")]
    public Sprite heartEmpty;

    private int maxLives;

    void Start()
    {
        maxLives = hearts.Count;
        SetLives(maxLives);
    }

    public void SetLives(int currentLives)
    {
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
}
