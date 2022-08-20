using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviour
{
    private int score = 0;
    // Start is called before the first frame update
    public event Action onScoreChange;

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        if (onScoreChange != null)
        {
            onScoreChange();
        }
    }
}
