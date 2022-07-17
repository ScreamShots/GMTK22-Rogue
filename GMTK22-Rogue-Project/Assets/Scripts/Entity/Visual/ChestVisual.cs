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
    [SerializeField]
    AudioSource audioSource;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;

    public AudioClip deathClip;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = chestRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        chestRenderer
            .DOColor(targetColor, deathAnimDuration)
            .OnComplete(() => CallBack?.Invoke());
    }

    public void PlayClipOnce(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.Play();
    }
}
