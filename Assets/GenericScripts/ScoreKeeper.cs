using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour, IReset
{
    private int score;
    public int Score => score;

    public void IncreaseScore(int points)
    {
        score += points;
    }

    public void Reset()
    {
        score = 0;
    }

    public bool TryToSaveHighScore(string gameName)
    {
        if (!PlayerPrefs.HasKey(gameName))
        {
            PlayerPrefs.SetInt(gameName, score);
            PlayerPrefs.Save();
            return true;
        }
        if (score > PlayerPrefs.GetInt(gameName))
        {
            PlayerPrefs.SetInt(gameName,score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public int GetHighscore(string gameName)
    {
        return PlayerPrefs.GetInt(gameName);
    }
}
