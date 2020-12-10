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

            client.Connect(ipAddress, 9000);
            // task.Wait();

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


                //유저리스트와 방 리스트를 받아온다.
                var task1 = new Task<string>(()=> this.ReadFunction());
                
                task1.Start();
                task1.Wait();
                Debug.Log(task1.Result);

            }


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
    ///  동기적으로 stream 으로부터 데이터를 읽어오는 task
    /// </summary>
    public string ReadFunction(){
        int bufferSize = 100;
        byte[] data = new byte[bufferSize];

        //온전한 데이터를 저장할 변수
        string result = "";
        while(true){
            //패킷은 여러개로 나뉘어 들어오기 때문에 끝까지 반복해서 읽어야한다
            ns.Read(data,0,bufferSize);
            string packet = Encoding.UTF8.GetString(data);
        
            //'\r\n' 이 포함되어 있다면
            if(result.Contains("\r\n")){
                Debug.Log("패킷 끝부분");
                //그 부분을 공백으로 대체한다.
                packet = packet.Replace("\r\n", "");
                result+=packet;
                Array.Clear(data, 0, bufferSize);
                break;
            }

            result+=packet;
            Array.Clear(data, 0, bufferSize);
        }

        // Debug.Log("result: " + result);
        return result;
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
