using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupEnd : BasicPopup
{
    // Use this for initialization
    void Start()
    {
        this.InitPopup(OnRetry, OnStop);
    }

    void OnRetry()
    {
        MainGameController.Retry();
    }

    void OnStop()
    {
        MainGameController.Quit();
    }
}