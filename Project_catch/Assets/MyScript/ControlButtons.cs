using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlButtons : MonoBehaviour
{

    public GameObject MakeRoomPanel;
    public GameObject LoadingPanel;
    public GameObject ErrorPanel;
    public GameObject TcpManager;
    public Text RoomNameText;
    public Text RoomPasswordText;
    public GameObject EnterRoomButton;
    public GameObject GridWithElements;


    //테스트용 스트링
    string userId = "hee46868@gmail.com";
    string nickname = "기보니";

    public ControlButtons(){
        
    }

    void Awake()
    {
        //Loading Panel 띄우기
        LoadingPanel.SetActive(true);

        bool result = TcpManager.GetComponent<TcpManager>().EnterUser(userId, nickname);
        
        //Loading Panel 띄우기
        LoadingPanel.SetActive(false);

        //요청이 실패하면 에러 패널을 띄우고 리턴한다.
        if(result == false){
            ErrorPanel.SetActive(true);
            return;
        }

        //방 목록을 업데이트한다.
        OnClickListRefreshButton();



    }


    //방 만들기 완료했을시 실행된다.
    //서버로 방 만들기 요청을 한 뒤 결과값을 받는다.
    //요청 성공 시 바로 방으로 이동한다.
    public void OkButton(){
        //방을 만드는 서버 작업을 해주어야 한다.

        /**게임 룸의 제목, 비밀번호, 방장 닉네임을 입력받아온다. */
        string ownerNickname = "기보니";
        string title = RoomNameText.text.ToString();
        string password = RoomPasswordText.text.ToString();
        
        //Loading Panel 띄우기
        LoadingPanel.SetActive(true);

        //GameRoom을 만들어 달라는 요청을 서버에게 보낸다.
        bool result = TcpManager.GetComponent<TcpManager>().MakeGameRoom(userId, ownerNickname, title, password);
        
        LoadingPanel.SetActive(false);
        
        //요청이 실패하면 에러 패널을 띄우고 리턴한다.
        if(result == false){
            ErrorPanel.SetActive(true);
            return;
        }
        

        //panel 닫기
        MakeRoomPanel.SetActive(false);

        //방으로 scene 옮기기
        // SceneManager.LoadScene("GameRoomScene");
    }


    //방 목록을 업데이트 해주는 메소드이다.
    //서버로 방 목록 업데이트를 요청한다. 유저가 Lobi에 입장하자마자 실행된다.
    public void OnClickListRefreshButton(){

        //로딩 패널 띄우기
        LoadingPanel.SetActive(true);

        //GridWithElement 아래의 모든 Instantiated Prefab 을 지운다.
        foreach(Transform child in GridWithElements.transform) {
            GameObject.Destroy(child.gameObject);
        }


        //TcpManager 로부터 roomlist 를 받는다. 받아온 roomlist 는 TcpManager의 Rooms, RoomData class 에 저장
        Rooms roomList = TcpManager.GetComponent<TcpManager>().GetRoomList(userId);


        //생성된 방이 없다면 아무것도 하지않음
        if(roomList == null){
            LoadingPanel.SetActive(false);
            Debug.Log("ControlButtons: roomList null");

            //비었다는 패널을 on 시킴.
            SetErrorPanelActive("방 목록이 없습니다");
            
            return;
        }

        int countRoom = roomList.rooms.Length;

        for(int i = 0 ; i < countRoom; i++){
            
            Text roomTitle = EnterRoomButton.transform.Find("RoomTitle").GetComponent<Text>();
            Text numPeople = EnterRoomButton.transform.Find("NumberPeople").GetComponent<Text>();
            Text roomId = EnterRoomButton.transform.Find("RoomId").GetComponent<Text>();

            roomTitle.text = roomList.rooms[i]._title;
            numPeople.text = roomList.rooms[i]._memNum + "/4";
            roomId.text = roomList.rooms[i]._roomId;

            GameObject enterRoomButton = Instantiate(EnterRoomButton) as GameObject;
            
            //oncick event 할당
            //onclick event 를 Instance 마다 달아주는 이유는 prefab 에는 달리지 않기 때문
            enterRoomButton.GetComponent<Button>().onClick.AddListener(() => OnClickEnterRoomButton(enterRoomButton));

            //GridWithElements 밑에다가 instantiate 한다
            enterRoomButton.transform.SetParent(GridWithElements.transform);
        }


        LoadingPanel.SetActive(false);

        Debug.Log("RefreshRoomList 성공");

    }

    //Instantiated GameObject 인 EnterRoomButton 의 onclick() 에 할당한다.
    //서버로 방의 입장처리를 요청하고 GameRoomScene 을 로드한다.
    public void OnClickEnterRoomButton(GameObject enterRoomButton){

        //로딩 패널 띄우기
        LoadingPanel.SetActive(true);

        string roomId = enterRoomButton.transform.Find("RoomId").GetComponent<Text>().text;

        string result = TcpManager.GetComponent<TcpManager>().RequestEnterRoom(userId, roomId);

        //만약 RequestEnterRoom 결과가 "0"이 아니라면(에러) 그 메시지를 error panel 에 표시한다.
        if(result.CompareTo("0") == 1){

            LoadingPanel.SetActive(false);
            SetErrorPanelActive(result);

            return;
        }
        
        LoadingPanel.SetActive(false);

        Debug.Log("ControlButtons; OnClickEnterRoomButton: 성공");

        //방으로 scene 옮기기
        SceneManager.LoadScene("GameRoomScene");
    }

    public void MakeRoomButton(){
        MakeRoomPanel.SetActive(true);
    }

    public void CancelButton(){
        MakeRoomPanel.SetActive(false);
    }

    public void ErrorPanelOkButton(){
        ErrorPanel.SetActive(false);
    }

    public void SetErrorPanelActive(string errorMsg){
        ErrorPanel.transform.Find("ErrorText").GetComponent<Text>().text = errorMsg;
        ErrorPanel.SetActive(true);
    }
}
