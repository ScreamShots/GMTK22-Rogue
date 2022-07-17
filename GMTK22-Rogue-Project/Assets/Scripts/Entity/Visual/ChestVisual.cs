using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChestVisual : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer chestRenderer;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = chestRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        chestRenderer
            .DOColor(targetColor, deathAnimDuration)
            .OnComplete(() => CallBack?.Invoke());
    }
}
