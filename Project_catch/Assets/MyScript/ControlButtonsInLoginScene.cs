using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtonsInLoginScene : MonoBehaviour
{

    public Text NicknameText; 
    public GameObject HttpManager;

    public void OnClickOkButtonInNicknamePanel(){
        string userNickname = NicknameText.text.ToString();

        string userEmail = GameObject.FindGameObjectWithTag("IdCarrier").GetComponent<Text>().text;
        HttpManager.GetComponent<CheckLoginOptions>().SaveNewNickname(userNickname, userEmail);

    }
}
