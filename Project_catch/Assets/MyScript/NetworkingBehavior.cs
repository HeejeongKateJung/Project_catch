using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Threading.Tasks;


public class NetworkingBehavior : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream ns;
    private GameObject USER_INFO;
    private GameObject ButtonControl;

    string ipAddress = "34.84.9.236";
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
                USER_INFO = GameObject.Find("USER_INFO");

                /// <summary>
                /// 데이터를 받아서 분석하는 Task
                /// </summary>

                //연결 성공 후 사용자 정보를 서버로 보냄
                string userId = USER_INFO.GetComponent<StaticVariableHolder>().USER_ID;
                string nickname = USER_INFO.GetComponent<StaticVariableHolder>().USER_NICKNAME;
                int score = USER_INFO.GetComponent<StaticVariableHolder>().USER_SCORE;
                UserInfo ui = new UserInfo("report_user_info", userId, nickname, score);
                SendObject(ui);
                var task1 = new Task<string>(()=> this.ReadFunction());
                // task1.ContinueWith((task =>
                // {
                //     Debug.Log("task1 done");
                //     return task1.Result;
                // }));
                
                task1.Start();
                Debug.Log("기다리는중,,,");
            }

            //
            // var task1 = new Task<string>(()=> this.ReadFunction());
            // // task1.ContinueWith((task =>
            // // {
            // //     Debug.Log("task1 done");
            // //     return task1.Result;
            // // }));
            
            // task1.Start();
            // Debug.Log("기다리는중,,,");
            // // Debug.Log(task1.Result);

        }catch(SocketException SCE){
            Debug.Log("Socket connection error: " + SCE.ToString());

            //에러 패널 띄우라는 신호            
            ButtonControl.GetComponent<ButtonControl>().Set_errorPanel_active("[network]Server Connection Failed");
        }
    }
    
    public void Send(String data){
        byte[] bytedata = Encoding.UTF8.GetBytes(data);
        ns.Write(bytedata, 0, bytedata.Length);
    }

    public void SendObject(object ob){
        String data = JsonUtility.ToJson(ob);
        Send(data);
    }

    /// <summary>
    ///  비동기로 서버로부터 데이터를 읽어오는 task
    /// </summary>
    // public async Task ReadAsync(){

    //     byte[] Receivebyte = new byte[100];
    //     Task<byte[]> task = ns.Read(Receivebyte, 0, Receivebyte.Length);

    //     await task;
    //     string result = Encoding.UTF8.GetString(Receivebyte);
    //     Debug.Log(result);
    
    //     return new ReceivedData();
    // }

    /// <summary>
    ///  동기적으로 stream 으로부터 데이터를 읽어오는 task
    /// </summary>
    public string ReadFunction(){
        byte[] data = new byte[1024];


        //먼저 데이터 사이즈를 받는다.
        byte[] dataSizeBuffer = new byte[100];

        Debug.Log("읽기를 기다리는 중,,,");
        ns.Read(dataSizeBuffer, 0, 100);

        // if(BitConverter.IsLittleEndian){
        //     Array.Reverse(dataSizeBuffer);
        //     Debug.Log("리틀 엔디언이래");
        // }
        int dataSize = BitConverter.ToInt32(dataSizeBuffer, 0);
        Debug.Log("dataSize: "+dataSize);



        // ns.Read(data,0,data.Length);
        // string result = Encoding.UTF8.GetString(data);
        // // Debug.Log("result: " + );
        // Debug.Log("result: " + result);
        

        return "dataSize.ToString()";
    }

    // public async Task Doit(){
    //     Task<UserInfo> userInfoTask = sibal();
    //     UserInfo d = await userInfoTask;

    // }

    // public Task<UserInfo> sibal(){
    //     return new Task<UserInfo>();
    // }
}

[Serializable]
public class UserInfo {
    public string _requestCode;
    public string _userId;
    public string _nickname;
    public int _score;
    public UserInfo(){
        
    }

    public UserInfo(string requestCode, string userId, string nickname, int score) {
        _requestCode = requestCode;
        _userId = userId;
        _nickname = nickname;
        _score = score;
    }
}

public class ReceivedData {
    public string _code;
    public string _msg;

    public ReceivedData(){}
}
