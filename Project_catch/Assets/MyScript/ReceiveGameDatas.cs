using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;



/**
    <ReceiveGameDatas> class: 서버로부터 Broadcast 되어 날아오는 데이터들을 받아서
    알맞은 처리를 하는 클래스이다

    TcpManager.GetReceive() 함수로 들어오는 JsonArray 형태의 string 을 받아서
    request code 를 보고 알맞는 역할을 수행한다.
*/
public class ReceiveGameDatas : MonoBehaviour
{
    private NetworkStream ns;
    private GameObject TcpManager;
    private GameObject CSGameManager;
    public const int CODE_CHATMSG = 111;     //채팅 메시지
    public const int CODE_NOTICE = 112;    //공지사항


    void Awake()
    {
        TcpManager = GameObject.Find("TcpManager");
        CSGameManager = GameObject.Find("CSGameManager");
        ns = TcpManager.GetComponent<TcpManager>().GetNetworkStream();
    }
    

    void Update()
    {
        //매번 게임 데이터가 왔는지 확인한다.
        if(ns.DataAvailable){
            string result = TcpManager.GetComponent<TcpManager>().GetReceive();

            ReceivingPacket packet = JsonUtility.FromJson<ReceivingPacket>("{\"datas\":" + result + "}");
            int _dataTypeCode = packet.datas[0]._dataTypeCode;
            switch(_dataTypeCode){
                case CODE_CHATMSG:
                    CSGameManager.GetComponent<ControlButtonsInMainRoom>().PrintChatMessage(packet);
                    break;

            }
            
        }
    }
}
[System.Serializable]
public class ReceivingData
{
    
    public int _dataTypeCode;
    public string _sentUserId;
    public string _sentNickname;
    public string _chatMsg;
    public string _time;
}

[System.Serializable]
public class ReceivingPacket
{
    public ReceivingData[] datas;

}

