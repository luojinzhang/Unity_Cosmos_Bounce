using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SplashFaded : MonoBehaviour
{
    public Image splashImage;

    void Awake()
    {
#if UNITY_STAND
        Screen.SetResolution(720, 1280, false);
#endif
    }

    IEnumerator Start()
    {
        splashImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(3.0f);
        FadeOut();
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MenuScene");
    }

    private void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    private void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, 2.0f, false);
    }
}