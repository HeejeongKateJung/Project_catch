using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlButtonsInMainRoom : MonoBehaviour
{
    private string userId;
    private string nickname;
    private GameObject TcpManager;
    public Text ChatInputFieldText;
    public GameObject ChatBox;
    public GameObject GridWithChats;

    void Start(){
        userId = GameObject.FindGameObjectWithTag("IdCarrier").GetComponent<Text>().text;
        nickname = GameObject.FindGameObjectWithTag("NicknameCarrier").GetComponent<Text>().text;
    }
    void Awake(){
        TcpManager = GameObject.Find("TcpManager");
    }
    public void OnClickSendChatButton(){

        ChatInputFieldText.text = "";
        string chatData = ChatInputFieldText.text.ToString();
        TcpManager.GetComponent<TcpManager>().SendChat(userId, nickname, chatData);

        string msgToPrint = "[" + nickname + "]" + chatData;
        GameObject chatBox = Instantiate(ChatBox) as GameObject;
        chatBox.transform.Find("ChatMsg").GetComponent<Text>().text = msgToPrint;

        chatBox.transform.SetParent(GridWithChats.transform);

    }

    public void ExitButton(){
        TcpManager.GetComponent<TcpManager>().RequestQuitRoom(userId);
        SceneManager.LoadScene("Lobi");
    }

    public void PrintChatMessage(ReceivingPacket packet){

        string nickname = packet.datas[0]._sentNickname;
        string msg = packet.datas[0]._chatMsg;

        string msgToPrint = "[" + nickname + "]" + msg;
        Debug.Log("ControlButtonsInMainRoom; PrintChatMessage; msgToPrint: " + msgToPrint);
        GameObject chatBox = Instantiate(ChatBox) as GameObject;
        chatBox.transform.Find("ChatMsg").GetComponent<Text>().text = msgToPrint;

        chatBox.transform.SetParent(GridWithChats.transform);
        

    }
}
