using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasicPopup : MonoBehaviour
{
    public Button BtnYes;
    public Button BtnNo;

    public void InitPopup(UnityAction onClickBtnYes, UnityAction onClickBtnNo)
    {
        BtnNo.onClick.AddListener(onClickBtnNo);
        BtnYes.onClick.AddListener(onClickBtnYes);
    }
}