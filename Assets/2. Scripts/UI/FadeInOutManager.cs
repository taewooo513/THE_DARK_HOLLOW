using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class FadeInOutManager : MonoUI
{
    private Image fadeImage;
    public float fadeInSpeed;
    public float fadeOutSpeed;

    float alpha = 1;

    void Start()
    {
        uiKey = "FadeManager";
        UIManager.Instance.AddUIManager(uiKey, this);
        fadeImage = GetComponentInChildren<Image>();
    }

    public bool FadeIn()
    {
        Color color = fadeImage.color;
        alpha -= fadeInSpeed * Time.deltaTime;
        if (alpha < 0)
        {
            alpha = 0;
        }
        color.a = alpha;
        fadeImage.color = color;

        if (alpha <= 0)
            return true;
        else
            return false;
    }

    public bool FadeOut()
    {
        Color color = fadeImage.color;
        alpha += fadeInSpeed * Time.deltaTime;
        color.a = alpha;
        fadeImage.color = color;

        if (alpha > 1)
            return true;
        else
            return false;
    }

}
