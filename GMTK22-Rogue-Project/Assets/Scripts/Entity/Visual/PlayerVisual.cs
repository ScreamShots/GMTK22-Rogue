using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer playerRender;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = playerRender.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        playerRender
            .DOColor(targetColor, deathAnimDuration)
            .OnComplete(() => CallBack?.Invoke());
    }
}
