using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour, IStop, IReset
{
    [SerializeField] private GameObject endPanel;

    [SerializeField] private TextMeshProUGUI labelCurrentScore, labelHighScore;

    public void UpdateCurrentScore(int score)
    {
        labelCurrentScore.text = "Score: " + score.ToString();
    }

    public void UpdateCurrentHighscore(int highscore)
    {
        labelHighScore.text = "Highscore: " + highscore.ToString();
    }

    public void Stop()
    {
        endPanel.SetActive(true);
    }

    public void Reset()
    {
        endPanel.SetActive(false);
    }
}
