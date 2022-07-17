using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreDisplay;

    public void UpdateScore(float currentScore)
    {
        scoreDisplay.text = currentScore.ToString();
    }
}
