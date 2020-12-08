using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ButtonControl : MonoBehaviour
{
    public GameObject ErrorPanel;
    // GameObject USER_INFO = GameObject.Find("USER_INFO");
    void Start()
    {
        // USER_INFO.USER_ID;
    }
    public void btn_getRoomList(){

        Debug.Log("btn_getRoomList Clicked");
    }

    public void btn_makeRoom(){
        Debug.Log("btn_makeRoom Clicked");
    }

    public void btn_startNow(){
        Debug.Log("btn_startNow Clicked");
    }

    public void Set_errorPanel_active(string msg){
        ErrorPanel.transform.Find("ErrorText").GetComponent<Text>().text = msg;
        ErrorPanel.SetActive(true);
    }


}
