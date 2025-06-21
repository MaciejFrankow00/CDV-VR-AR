using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmp;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settings;

    int score = 0;
    public int Score => score;

    
    void Update()
    {
        string _score = score.ToString();
        tmp.text = _score;
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Player has been exit the game");
    }
}
