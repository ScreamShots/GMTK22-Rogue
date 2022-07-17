using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DebugFillableBar : MonoBehaviour
{
    [SerializeField]
    Image fillableBar;
    [SerializeField]
    float animationSpeed;

    Tween currentAnimation;

    public void UpdateFillable(float currentValue, float maxValue)
    {
        currentAnimation?.Kill();

        currentAnimation = DOTween.To
            (
            () => fillableBar.fillAmount,
            x => fillableBar.fillAmount = x,
            currentValue / maxValue, 
            (1 - (currentValue / maxValue)) * animationSpeed
            );
    }

    private void OnDestroy()
    {
        currentAnimation?.Kill();
    }


}
