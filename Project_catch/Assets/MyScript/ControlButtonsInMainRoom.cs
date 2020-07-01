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

        userId = "hee4686";
        nickname = "기보니";
        // userId = GameObject.FindGameObjectWithTag("IdCarrier").GetComponent<Text>().text;
        // nickname = GameObject.FindGameObjectWithTag("NicknameCarrier").GetComponent<Text>().text;

        //입장 메시지 출력
        GameObject chatBox = Instantiate(ChatBox) as GameObject;
        chatBox.transform.Find("ChatMsg").GetComponent<Text>().text = "[" + nickname + "] 님이 입장하셨습니다";
        chatBox.transform.SetParent(GridWithChats.transform);
    }
    void Awake(){
        TcpManager = GameObject.Find("TcpManager");
        Users users = TcpManager.GetComponent<TcpManager>().GetUserList(userId);
        // Debug.Log("users");
    }
    
    //채팅 보내기 버튼을눌렀을때 자신의 챗을 띄우고 서버로 메시지를 보내는 함수
    public void OnClickSendChatButton(){

        ChatInputFieldText.text = "";
        string chatData = ChatInputFieldText.text.ToString();

        //TcpManager를 통해서 SendChat한다
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


    public void PrintNotice(ReceivingPacket packet){
        string nickname = packet.datas[0]._sentNickname;
        string msg = packet.datas[0]._chatMsg;

        GameObject chatBox = Instantiate(ChatBox) as GameObject;
        chatBox.transform.Find("ChatMsg").GetComponent<Text>().text = msg;

        chatBox.transform.SetParent(GridWithChats.transform);
    }
}
