using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmp;
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup settings;

    [Header("Start Button")]
    [SerializeField] string destinationScene;
    
    private void Start()
    {
        mainMenu.alpha = 1f;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
        settings.alpha = 0f;
        settings.interactable = false;
        settings.blocksRaycasts = false;

        int score = PlayerPrefs.GetInt("LastScore", 0);
        tmp.text = score.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(destinationScene);
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
        PlayerPrefs.DeleteKey("LastScore");
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("Player has been exit the game");
    }
}
