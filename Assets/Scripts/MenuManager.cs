using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmp;
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup settings;

    public int score = 0;
    private void Start()
    {
        mainMenu.alpha = 1f;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
        settings.alpha = 0f;
        settings.interactable = false;
        settings.blocksRaycasts = false;
    }

    void Update()
    {
        string _score = score.ToString();
        tmp.text = _score;
    }

    public void OpenSettings()
    {
        mainMenu.alpha = 0f;
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
        settings.alpha = 1f;
        settings.interactable = true;
        settings.blocksRaycasts = true;
    }

    public void CloseSettings()
    {
        settings.alpha = 0f;
        settings.interactable = false;
        mainMenu.blocksRaycasts = true;
        mainMenu.alpha = 1f;
        mainMenu.interactable = true;
        settings.blocksRaycasts = false;
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Player has been exit the game");
    }
}
