using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCredit : MonoBehaviour {

    public Button BtnClose;
    // Use this for initialization
    void Start ()
    {
        BtnClose.onClick.AddListener(this.OnClickBtnClose);
    }

    private void OnClickBtnClose()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
}
