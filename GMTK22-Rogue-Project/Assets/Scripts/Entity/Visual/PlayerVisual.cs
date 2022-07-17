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

    [SerializeField] AudioSource audioSource;

    [HorizontalLine]

    [SerializeField]
    float deathAnimDuration;

    [SerializeField] Sprite healthySprite;
    [SerializeField] Sprite hurtSprite;
    [SerializeField] Sprite nearDeathSprite;

    public AudioClip attackClip;
    public AudioClip moveClip;
    public AudioClip damageClip;


    public void OnDeathAnimation(Action CallBack)
    {
        Color currentColor = playerRender.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        playerRender
            .DOColor(targetColor, deathAnimDuration)
            .OnComplete(() => CallBack?.Invoke());
    }

    public void ChangeHealthState(float maxHP, float currentHP)
    {
        if (currentHP > maxHP * 0.5)
        {
            playerRender.sprite = healthySprite;
        }
        else if (currentHP <= maxHP * 0.5 && currentHP > maxHP * 0.25)
        {
            playerRender.sprite = hurtSprite;
        }
        else
        {
            playerRender.sprite = nearDeathSprite;
        }
    }

    public void PlayClipOnce (AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.Play();
    }
}
