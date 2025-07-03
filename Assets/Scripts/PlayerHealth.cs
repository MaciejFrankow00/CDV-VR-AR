using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ScoreManager scoreManager;

    private int currentHealth;
    private bool playerIsStillBreathing = true;

    public int CurrentHealth => currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!playerIsStillBreathing)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (healthBar != null)
            healthBar.value = currentHealth;

        Debug.Log($"Plater lost {amount} HP!");

        //Reset score multiplier if player receive damage
        if (scoreManager != null)
            scoreManager.ResetMultiplier();

        if (currentHealth <= 0)
        {
            playerIsStillBreathing = false;

            //Tell scoreManager that it can save score as final
            if (scoreManager != null)
                scoreManager.SaveScore();

            StartCoroutine(GameOver());
            Debug.Log("GAME OVER!");
        }
    }

    private IEnumerator GameOver()
    {
        SoundFXManager.instance.PlaySound2D(SoundType.DEFEAT, transform);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MainMenu");
    }
}