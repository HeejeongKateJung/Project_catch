using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public class CheckNetworkStatus : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(checkInternetConnection((isConnected) =>{
    //         //handle connection status here
    //         Debug.Log("false");
    //     }));  
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // IEnumerator checkInternetConnection(Action<bool> action){
    //     WWW www = new WWW("https://google.com");
    //     yield return www;

    //     if(www.error != null){
    //         action(false);
    //         Debug.Log("false");
    //     }else{
    //         action (true);
    //         Debug.Log("true");
    //     }
    // }
}
