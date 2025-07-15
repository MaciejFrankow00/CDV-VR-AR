using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settings;

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

    public async void StartGame()
    {
        PlayButtonSound();
        await Task.Delay(500);
        SceneManager.LoadScene(destinationScene);
    }
    
    public async void OpenSettings()
    {
        PlayButtonSound();
        await Task.Delay(500);
        mainMenu.alpha = 0f;
        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;
        settings.alpha = 1f;
        settings.interactable = true;
        settings.blocksRaycasts = true;
    }

    public async void CloseSettings()
    {
        PlayButtonSound();
        await Task.Delay(500);
        settings.alpha = 0f;
        settings.interactable = false;
        settings.blocksRaycasts = false;
        mainMenu.alpha = 1f;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;
    }

    public async void ExitGame()
    {
        PlayButtonSound();
        await Task.Delay(500);
        PlayerPrefs.DeleteKey("LastScore");
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("Player has been exit the game");
    }

    private void PlayButtonSound()
    {
        SoundFXManager.instance.PlaySound2D(SoundType.BUTTON, transform, 1f);
    }
}
