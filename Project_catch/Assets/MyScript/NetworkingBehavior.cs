using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;


public class NetworkingBehavior : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream ns;
    private GameObject USER_INFO;
    private GameObject ButtonControl;

    string ipAddress = "35.243.93.175";
    // Start is called before the first frame update
    void Awake() {
        client = new TcpClient();
        GameObject ButtonControl = GameObject.Find("ButtonControl");
        USER_INFO = GameObject.Find("USER_INFO");
    }

    void Start()
    {
        try{

            var task = client.ConnectAsync(ipAddress, 9000);
            task.Wait();
            
            if(client.Connected)
            {
                Debug.Log("9000포트 연결 성공");
                ns = client.GetStream();
            }
            

        }catch(SocketException SCE){
            Debug.Log("Socket connection error: " + SCE.ToString());

            //에러 패널 띄우라는 신호            
            ButtonControl.GetComponent<ButtonControl>().Set_errorPanel_active("[network]Server Connection Failed");
        }
    }
    
    
    /**GameObject 를 byte array 로 바꾼다.*/
    private byte[] ObjectToBytes(object gameObject){
        //gameRoom object 를 json string 으로 변환한다.
        string jsonString;
        jsonString = JsonUtility.ToJson(gameObject);

        //json string 을 byte 로 변환한다.

        byte[] data = Encoding.UTF8.GetBytes(jsonString);

        return data;
    }


    private void Send_user_info(GameObject USER_INFO, NetworkStream ns) {
        string userId = USER_INFO.GetComponent<StaticVariableHolder>().USER_ID;
        string nickname = USER_INFO.GetComponent<StaticVariableHolder>().USER_NICKNAME;
        int score = USER_INFO.GetComponent<StaticVariableHolder>().USER_SCORE;

        UserInfo ui = new UserInfo("report_user_info", userId, nickname, score);
        byte[] data = ObjectToBytes(ui);
        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);
    }
}

[Serializable]
public class UserInfo {
    public string _requestCode;
    public string _userId;
    public string _nickname;
    public int _score;
    public UserInfo(){}

    public UserInfo(string requestCode, string userId, string nickname, int score) {
        _requestCode = requestCode;
        _userId = userId;
        _nickname = nickname;
        _score = score;
    }
}
