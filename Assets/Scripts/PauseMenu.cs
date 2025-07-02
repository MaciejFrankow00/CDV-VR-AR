using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        CloseSettings();
        DisplayPauseMenu();
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            DisplayPauseMenu();
    }
    
    public void DisplayPauseMenu()
    {
        if (mainGroup.alpha == 1f)
        {
            CloseSettings();
            
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

    public void OpenSettings()
    {
        options.alpha = 0f;
        options.interactable = false;
        options.blocksRaycasts = false;
        settings.alpha = 1f;
        settings.interactable = true;
        settings.blocksRaycasts = true;
    }

    public void CloseSettings()
    {
        settings.alpha = 0f;
        settings.interactable = false;
        settings.blocksRaycasts = false;
        options.alpha = 1f;
        options.interactable = true;
        options.blocksRaycasts = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
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
}
