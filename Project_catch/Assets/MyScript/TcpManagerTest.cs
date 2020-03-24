using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;


public class TcpManagerTest : MonoBehaviour
{

    private TcpClient socket;
    private NetworkStream ns;
    private Thread recvThread;
    public string ipAddress = "35.243.93.175";
    public const int portNum = 8000;
    
    private int SendDataLength = 100; //send data length, byte;
    private int ReceivedDataLength = 100; //Receive data length, byte;

    private byte[] Sendbyte;    //Data encoding to send.
    private byte[] Receivebyte;    //Receive data by this array to save
    private string ReceiveString;   //Receive bytes to string
    
    
    void Awake()
    {
        //Socket Create
        Debug.Log("Awake");
        socket = new TcpClient();
        
        //Socket connect
        try{
            
            socket.Connect(ipAddress, portNum);
            ns = socket.GetStream();
            
            recvThread = new Thread(new ThreadStart(RecvThread));
            recvThread.IsBackground = true;
            recvThread.Start();
            

        }catch(SocketException SCE){
            Debug.Log("Socket connection error: " + SCE.ToString());
            return;
        }

    }


    public void RecvThread()
    {
        Sendbyte = new byte[SendDataLength];
        Receivebyte = new byte[ReceivedDataLength];
        string msg = "TESTING";


        try{
                ns.Read(Receivebyte, 0, Sendbyte.Length);
                msg = Encoding.ASCII.GetString(Receivebyte);
                Debug.Log(msg);

                Sendbyte = Encoding.ASCII.GetBytes(msg);
                ns.Write(Sendbyte, 0, Sendbyte.Length);
                
                
        }catch(Exception e){
                Debug.Log(e.Message);
        }

        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        if(recvThread != null)
        {
            recvThread.Abort();
            Debug.Log("recvThread Alive? : " + recvThread.IsAlive);
        }
    }
}
