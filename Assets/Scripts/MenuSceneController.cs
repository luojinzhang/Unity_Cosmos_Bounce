using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour {

    public Button BtnPlay, BtnCredit, BtnExit;
    public GameObject PopupCredit;
    public Animator BlackOnClick;
    public AudioSource AudioBG;
    public RectTransform Title, Menu;
    float UiOffSetX;
    float UiOffSetY;
    float aspectRatio;

    void Start()
    {
        Time.timeScale = 1;
        BtnPlay.onClick.AddListener(this.OnClickBtnPlay);
        BtnCredit.onClick.AddListener(this.OnClickBtnCredit);
        BtnExit.onClick.AddListener(this.OnClickBtnExit);
        aspectRatio = ((float)Screen.width / (float)Screen.height);
        UiOffSetX = (float)Screen.width / 6f;
        UiOffSetY = (float)Screen.height / 5f;
        Menu.offsetMin = new Vector2(UiOffSetX, UiOffSetY - (float)Screen.height / 7f);
        Menu.offsetMax = new Vector2(-UiOffSetX, -UiOffSetY - (float)Screen.height / 3f);
        Title.offsetMin = new Vector2(UiOffSetX, UiOffSetY + (float)Screen.height / 3f);
        Title.offsetMax = new Vector2(-UiOffSetX, -UiOffSetY - (float)Screen.height / 7f);

        AudioPlay();
    }

    public void AudioPlay()
    {
        AudioBG.Play();
    }

    public void AudioStop()
    {
        AudioBG.Stop();
    }

    private void OnClickBtnExit()
    {
        Application.Quit();
    }

    private void OnClickBtnCredit()
    {
        PopupCredit.SetActive(true);
    }

    private void OnClickBtnPlay()
    {
        //Time.timeScale = 1;
        //SceneManager.LoadScene("MainScene");
        BlackOnClick.SetTrigger("OnClick");
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
