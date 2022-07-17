using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using NaughtyAttributes;

public class MonsterVisual : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer monsterRenderer;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = monsterRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        monsterRenderer
            .DOColor(targetColor, deathAnimDuration)
            .OnComplete(() => CallBack?.Invoke());
    }
}
