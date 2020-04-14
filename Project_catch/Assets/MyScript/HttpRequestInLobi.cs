// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.SceneManagement;
// using System;

// public class HttpRequestInLobi : MonoBehaviour
// {
//     private string _nickname;
//     public string GetNicknameFromServer(string userEmail)
//     {
//         StartCoroutine(IEGetNicknameFromServer(userEmail, (result)=>{
            
//             _nickname = result;
//             Debug.Log("HttpRequestInLobi:nickname:"+_nickname);
//         }));
//         // return _nickname;
//     }

//     private string ipAddress = "http://35.243.93.175";
//     IEnumerator IEGetNicknameFromServer(string userEmail, Action<string> callback){
//         WWWForm form = new WWWForm();
//         form.AddField("email", userEmail);
//         UnityWebRequest webRequest = UnityWebRequest.Post(ipAddress+"/returnNickname.php", form);
//         yield return webRequest.SendWebRequest();
        
//         string result = webRequest.downloadHandler.text;

//         callback(result);
        

//     }

// }
