using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpScore;
    [SerializeField] private TMP_Text tmpMultiplier;

    private int score;
    private int multiplier = 1;

    void Update()
    {
        tmpScore.text = score.ToString();
        tmpMultiplier.text = "Multiplier: " + multiplier.ToString();
    }
    
    public void AddPoint()
    {
        score += 10 * multiplier;
        multiplier = Mathf.Min(multiplier + 1, 10);
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }
}
