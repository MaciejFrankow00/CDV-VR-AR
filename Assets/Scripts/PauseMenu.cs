using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading.Tasks;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainGroup;
    [SerializeField] private CanvasGroup options;
    [SerializeField] private CanvasGroup settings;

    [Header("Lightsaber and controllers")]
    [SerializeField] private GameObject[] controllerVisuals;
    [SerializeField] private GameObject lightsaber;

    void Start()
    {
        settings.alpha = 0f;
        settings.interactable = false;
        settings.blocksRaycasts = false;
        options.alpha = 1f;
        options.interactable = true;
        options.blocksRaycasts = true;
        mainGroup.alpha = 0f;
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;

        //Do not remove crucial line below!
        Time.timeScale = 1;

        GameMode(true);
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            DisplayPauseMenu();
    }
    
    public async void DisplayPauseMenu()
    {
        if (mainGroup.alpha == 1f)
        {
            CloseSettings();
            await Task.Delay(500);

            mainGroup.alpha = 0f;
            mainGroup.interactable = false;
            mainGroup.blocksRaycasts = false;
            
            //Do not remove crucial line below!
            Time.timeScale = 1;

            GameMode(true);
        }
        else if (mainGroup.alpha == 0f)
        {
            mainGroup.alpha = 1f;
            mainGroup.interactable = true;
            mainGroup.blocksRaycasts = true;

            //Do not remove crucial line below!
            Time.timeScale = 0;

            GameMode(false);
        }
    }

    public async void OpenSettings()
    {
        PlayButtonSound();
        await Task.Delay(500);
        options.alpha = 0f;
        options.interactable = false;
        options.blocksRaycasts = false;
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
        options.alpha = 1f;
        options.interactable = true;
        options.blocksRaycasts = true;
    }

    public async void RestartGame()
    {
        PlayButtonSound();
        await Task.Delay(500);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public async void BackToMainMenu()
    {
        PlayButtonSound();
        await Task.Delay(500);
        SceneManager.LoadScene("MainMenu");
    }

    private void GameMode(bool showSaber)
    {
        foreach (GameObject controller in controllerVisuals)
        {
            if (controller != null)
                controller.SetActive(!showSaber);
        }

        if (lightsaber != null)
            lightsaber.SetActive(showSaber);
    }

    private void PlayButtonSound()
    {
        SoundFXManager.instance.PlaySound2D(SoundType.BUTTON, transform, 1f);
    }
}
