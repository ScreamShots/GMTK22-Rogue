using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    float currentScore;

    [HorizontalLine]

    [SerializeField]
    ScoreUI scoreUI;

    public event Action<float> ScoreModified;

    public void Init()
    {
        ScoreModified += scoreUI.UpdateScore;
        scoreUI.UpdateScore(0);
    }

    public void ResetScore()
    {
        currentScore = 0;
        ScoreModified?.Invoke(currentScore);
    }

    public void AddScore(float scoreToAdd)
    {
        currentScore += scoreToAdd;
        ScoreModified?.Invoke(currentScore);
    }
}
