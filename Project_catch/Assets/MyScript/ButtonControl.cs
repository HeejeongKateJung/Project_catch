using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class ButtonControl : MonoBehaviour
{
    public GameObject ErrorPanel;
    public GameObject RoomListContent;
    // GameObject USER_INFO = GameObject.Find("USER_INFO");
    void Start()
    {
        // USER_INFO.USER_ID;
    }
    public void btn_getRoomList(string jsonString){
        Debug.Log("{\"rooms\":" + jsonString + @"\}");
        Debug.Log(@"\}");
        String.Concat(jsonString, "}");
        Debug.Log(String.Concat(jsonString, "}"));
        string json = new JavaScriptSerializer().Serialize(jsonString);
        Debug.Log(json);

        Rooms roomList = JsonUtility.FromJson<Rooms>("{\"rooms\":" + jsonString + "}");
        
        Transform parent = GameObject.Find("RoomListContent").GetComponent<Transform>();
        GameObject prefab = Resources.Load ("Prefabs/RoomButton") as GameObject;

        int countRoom = roomList.rooms.Length;

        for(int i = 0 ; i < countRoom; i++){
            //Prefab 을 인스턴스화 한다.
            GameObject room = Instantiate(prefab);
            room.transform.parent = parent;
            
            //update 할 object text 들을 가져온다.
            Text roomTitle = room.transform.Find("RoomTitle").GetComponent<Text>();
            Text numPeople = room.transform.Find("Capability").GetComponent<Text>();
            Text roomId = room.transform.Find("RoomId").GetComponent<Text>();

            //패킷 데이터들을 object 의 text component 에 할당한다.
            roomTitle.text = roomList.rooms[i]._title;
            numPeople.text = roomList.rooms[i]._memNum + "/4";
            roomId.text = roomList.rooms[i]._roomId;

            //oncick event 할당
            //onclick event 를 Instance 마다 달아주는 이유는 prefab 에는 달리지 않기 때문
            // room.GetComponent<Button>().onClick.AddListener(() => OnClickEnterRoomButton(enterRoomButton));

            //GridWithElements 밑에다가 instantiate 한다
            // enterRoomButton.transform.SetParent(GridWithElements.transform);
        }
        
        Debug.Log("btn_getRoomList Clicked");

    }

    public void btn_makeRoom(){
        Debug.Log("btn_makeRoom Clicked");
    }

    public void btn_startNow(){
        Debug.Log("btn_startNow Clicked");
    }

    public void Set_errorPanel_active(string msg){
        ErrorPanel.transform.Find("ErrorText").GetComponent<Text>().text = msg;
        ErrorPanel.SetActive(true);
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
