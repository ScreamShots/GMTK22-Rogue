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
    [SerializeField]
    AudioSource audioSource;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;

    public AudioClip moveClip;
    public AudioClip attackClip;
    public AudioClip damageClip;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = monsterRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        monsterRenderer
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
