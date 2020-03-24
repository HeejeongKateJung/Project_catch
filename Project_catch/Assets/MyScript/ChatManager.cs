using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    private string userId = "hee46868@gmail.com";
    private string nickname = "기보니";
    public GameObject TcpManager;
    public Text ChatInputFieldText;
    public void OnClickSendChatButton(){

        string chatData = ChatInputFieldText.text.ToString();
        TcpManager.GetComponent<TcpManager>().SendChat(userId, nickname, chatData);

    }

    // public const string CODE_CHAT = 41;
    // public const string CODE_NOTICE = 42;


    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
