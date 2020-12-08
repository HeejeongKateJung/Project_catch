using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;

public class ReportHeartbeat : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream ns;
    void Awake(){
        Debug.Log("[checknetworkstatus]");
        GameObject USER_INFO = GameObject.Find("USER_INFO");
        //Socket Create
        client = new TcpClient();
        string ipAddress = "34.84.9.236";
        
        //Socket connect
        try{
            // string ipAddress = USER_INFO.GetComponent<StaticVariableHolder>().ipAddress;
            var task = client.ConnectAsync(ipAddress, 7000);
            task.Wait();
            

            if(client.Connected)
            {
                Debug.Log("it works!");

                ns = client.GetStream();
                StartCoroutine("Send_heartbeat", ns);


            }
            

        }catch(SocketException SCE){
            Debug.Log("Socket connection error: " + SCE.ToString());

            
        }finally {
            //에러 패널 띄우라는 신호
            GameObject ButtonControl = GameObject.Find("ButtonControl");
            ButtonControl.GetComponent<ButtonControl>().Set_errorPanel_active("[hb]Server Connection Failed");
        }
    }

    
    IEnumerator Send_heartbeat(NetworkStream stream){
        byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
        stream.Write(data, 0, data.Length);
        Debug.Log("heartbeat 보냄");


        yield return new WaitForSeconds(5.0f);
        StartCoroutine("Send_heartbeat", stream);
    }
}
