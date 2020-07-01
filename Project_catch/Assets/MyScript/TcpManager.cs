using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;


public class TcpManager : MonoBehaviour
{

    private TcpClient socket;
    private NetworkStream ns;
    private Thread recvThread;
    private string ipAddress = "35.243.93.175";
    public const int portNum = 8000;

    //요청 코드
    public const string CODE_ENTERUSER = "11";
    public const string CODE_MAKEROOM = "21";
    public const string CODE_ENTERROOM = "22";
    public const string CODE_QUITROOM = "23";
    public const string CODE_REFRESHROOMLIST = "31";
    public const string CODE_REFRESHUSERLIST = "51";
    public const string CODE_SENDCHAT = "41";
    
    
    void Awake()
    {
        //Socket Create
        socket = new TcpClient();
        
        //Socket connect
        try{
            
            socket.Connect(ipAddress, portNum);
            ns = socket.GetStream();
            

        }catch(SocketException SCE){
            Debug.Log("Socket connection error: " + SCE.ToString());

            //에러 패널 띄우라는 신호
            return;
        }

    }

    public NetworkStream GetNetworkStream(){
        return ns;
    }

    /**GameRequest 를 byte 로 바꾼다.*/
    public byte[] ObjectToBytes(object gameRequest){
        //gameRoom object 를 json string 으로 변환한다.
        string jsonString;
        jsonString = JsonUtility.ToJson(gameRequest);

        //json string 을 byte 로 변환한다.

        byte[] data = Encoding.UTF8.GetBytes(jsonString);

        return data;
    }

    //유저가 로비에 들어오자마자 실행된다.
    public bool EnterUser(string userId, string nickname){
        //유저가 들어오면 방에 입장한게 아니므로 
        //GameRequest(_requestCode, _userId, _nickname, _title, _password) 중 title과 password 는 임의로 작성
        GameRequest enterRequest = new GameRequest(CODE_ENTERUSER, userId, nickname, "-", "-");

        byte[] data = ObjectToBytes(enterRequest);
        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);

        //서버로부터 성공(0) 혹은 실패(-1) 코드를 받는다.
        byte[] Receivebyte = new byte[100];
        ns.Read(Receivebyte, 0, Receivebyte.Length);


        string result = Encoding.UTF8.GetString(Receivebyte);

        //요청이 실패하면 에러메시지를 받게 된다 출력하고 false 값 리턴.
        if(result.CompareTo("0") == 1){
            Debug.Log(result);
            return false;
        }

        //성공하면 true 값 리턴
        return true;

        
    }


    //서버에게 방 만들기를 요청하기 위한 함수
    public string MakeGameRoom(string userId, string ownerNickname, string title, string password){
        
        GameRequest makeRoomRequest = new GameRequest(CODE_MAKEROOM 
        ,userId, ownerNickname, title, password);
        
        //gameRequest -> byte data
        byte[] data = ObjectToBytes(makeRoomRequest);
        //데이터는 먼저 데이터의 사이즈 4만큼 를 보내고 데이터를 보낸다.
        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);

        //서버로부터 성공(0) 혹은 실패(-1) 코드를 받는다.
        byte[] Receivebyte = new byte[100];
        ns.Read(Receivebyte, 0, Receivebyte.Length);


        string roomId = Encoding.UTF8.GetString(Receivebyte);
        //요청이 실패하면 0이 아닌 에러메시지를 받게 된다.
        Debug.Log(roomId);

        //성공하면 true 값 리턴
        return roomId;
        
        
    }

    /** <서버로부터 Packet 을 받아오는 함수>
        고정 버퍼 사이즈 만큼 byte 를 읽어서 합친 string data 를 리턴한다.
    */
    public string GetReceive(){
        /** 문제: packet이 짤려서 오는 문제... roomList 전부가 오질 않는다..*/

        //먼저 데이터 사이즈를 받는다.
        byte[] dataSizeBuffer = new byte[4];
        ns.Read(dataSizeBuffer, 0, 4);

        if(BitConverter.IsLittleEndian){
            Array.Reverse(dataSizeBuffer);
            // Debug.Log("리틀 엔디언이래");
        }
        
        int dataSize = BitConverter.ToInt32(dataSizeBuffer, 0);
        Debug.Log("dataSize: "+dataSize);


        //고정 버퍼사이즈를 receiveByte에 할당한다.
        int bufferSize = 100;
        byte[] receiveByte = new byte[bufferSize];

        int i = 0;
        string result = "";
        //그 bytearray 에 buffer size 만큼 읽는다.
        while(i < dataSize/bufferSize){
            ns.Read(receiveByte, 0, bufferSize);
            string EncodedString = Encoding.UTF8.GetString(receiveByte);
            // Debug.Log(EncodedString);

            //결과 값을 result 에 저장한다.
            result+=EncodedString;
            i++;

        }

        //bufferSize 보다 짧은 데이터들은 따로 받는다.
        byte[] receiveByte2 = new byte[dataSize%bufferSize];
        ns.Read(receiveByte2, 0, dataSize%bufferSize);
        string EncodedString2 = Encoding.UTF8.GetString(receiveByte2);
        // Debug.Log(EncodedString2);
        result+=EncodedString2;

        return result;
    }

    //서버에게 방 리스트를 받아오는 함수
    public Rooms GetRoomList(string userId){

        //먼저 새로고침 요청을 서버로 보낸다.
        GameRequest refreshRoomListRequest = new GameRequest(CODE_REFRESHROOMLIST, userId
        , "-", "-", "-");

        byte[] data = ObjectToBytes(refreshRoomListRequest);

        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);

        //서버로부터 JsonArray 형태의 "RoomList" string을 받게된다.
        string result = GetReceive();

        if(result == ""){
            return null;
        }


        //JsonArray가 비었는지 확인한다
        if(result.Equals("[]")){
            Debug.Log("TcpManager: no Result");
            return null;
        }
        //에러가 뜨지 않았다면 받아온 JsonArray에 담긴 roomList를 꺼낸다.
        Rooms roomList = JsonUtility.FromJson<Rooms>("{\"rooms\":" + result + "}");
        return roomList;

        
    }

    public Users GetUserList(string userId){

        //먼저 새로고침 요청을 서버로 보낸다.
        GameRequest refreshUserListRequest = new GameRequest(CODE_REFRESHUSERLIST, userId);

        byte[] data = ObjectToBytes(refreshUserListRequest);

        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);


        //서버로부터 JsonArray 형태의 "RoomList" string을 받게된다.
        Debug.Log("result 기다리는중");
        string result = GetReceive();
        Debug.Log("TcpManager: GetUserList: " + result);
        if(result == ""){
            return null;
        }


        //JsonArray가 비었는지 확인한다
        if(result.Equals("[]")){
            Debug.Log("TcpManager: no Result");
            return null;
        }
        //에러가 뜨지 않았다면 받아온 JsonArray에 담긴 roomList를 꺼낸다.
        Users userList = JsonUtility.FromJson<Users>("{\"users\":" + result + "}");
        return userList;

        
    }

   

    public String RequestEnterRoom(string userId, string roomId){
        GameRequest enterRoomRequest = new GameRequest(CODE_ENTERROOM, userId, roomId);

        byte[] data = ObjectToBytes(enterRoomRequest);

        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);

        //서버로부터 성공(0) 혹은 실패(오류메시지) 코드를 받는다.
        byte[] Receivebyte = new byte[100];
        ns.Read(Receivebyte, 0, Receivebyte.Length);
        string result = Encoding.UTF8.GetString(Receivebyte);

        Debug.Log("TcpManager; REquestEnterRoom 결과: " + result);

        //요청이 실패하면 0이 아닌 에러메시지를 받게 된다.
        if(result.CompareTo("0") == 1){
            Debug.Log("RequestEnterRoom 결과에러: " + result);
            return result;
        }

        //방으로 scene 옮기기
        SceneManager.LoadScene("GameRoomScene");
        return result;
    }

    public void RequestQuitRoom(string userId){
        GameRequest quitRoomRequest = new GameRequest(CODE_QUITROOM, userId);

        byte[] data = ObjectToBytes(quitRoomRequest);

        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);
    }

    public void SendChat(string userId, string nickname, string chatData){
        ChatInformation chatInformation = new ChatInformation(
            CODE_SENDCHAT, userId, nickname, System.DateTime.Now.ToString("HHmmss"), chatData
        );

        byte[] data = ObjectToBytes(chatInformation);

        ns.Write(BitConverter.GetBytes(data.Length), 0, 4);
        ns.Write(data, 0, data.Length);
    }
}
    
[Serializable]
public class GameRequest{

    public string _requestCode;
    public string _userId;
    public string _nickname;
    public string _title;
    public string _password;
    public string _roomId;
    public string _chatText;
    
    public GameRequest(){

    }

    public GameRequest(string requestCode, string userId, string nickname, string title, string password){
        _requestCode = requestCode;
        _userId = userId;
        _nickname = nickname;
        _title = title;
        _password = password;
        
    }

    public GameRequest(string requestCode, string userId, string roomId){
        
        _requestCode = requestCode;
        _userId = userId;
        _roomId = roomId;
    }

    public GameRequest(string requestCode, string userId){
        
        _requestCode = requestCode;
        _userId = userId;
    }
}


[Serializable]
public class ChatInformation{

    public string _requestCode;
    public string _userId;
    public string _nickname;
    public string _time;
    public string _chatText;

    public ChatInformation(){

    }
    public ChatInformation(
        string requestCode, string userId, string nickname, string time, string chatText)
        {
            _requestCode = requestCode;
            _userId = userId;
            _nickname = nickname;
            _time = time;
            _chatText = chatText;
        }
}

[System.Serializable]
public class RoomData
{
    
    public string _roomId;
    public int _memNum;
    public string _title;
    public string _password;
}

[System.Serializable]
public class Rooms
{
    public RoomData[] rooms;

}

[System.Serializable]
public class UserData
{
    
    public string _userId;
    public int _nickname;
    public string _priority;
    public string _roomId;
}

[System.Serializable]
public class Users
{
    public UserData[] users;

}


