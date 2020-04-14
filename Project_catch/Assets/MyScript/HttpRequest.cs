using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HttpRequest : MonoBehaviour
{
    public GameObject NicknamePanel;

    public void CheckFirstLogin(string userEmail)
    {
        StartCoroutine(IECheckFirstLogin(userEmail));
    }

    private string ipAddress = "http://35.243.93.175";
    IEnumerator IECheckFirstLogin(string userEmail){
        WWWForm form = new WWWForm();
        form.AddField("email", userEmail);
        UnityWebRequest webRequest = UnityWebRequest.Post(ipAddress+"/checkFirstLogin.php", form);
        yield return webRequest.SendWebRequest();
        
        string result = webRequest.downloadHandler.text;

        Debug.Log("httpRequest;"+result);

        //새로운 유저가 로그인 했다면 닉네임을 설정하는 패널을 띄운다
        if(result.CompareTo("new_user") == 0){
            NicknamePanel.SetActive(true);
        }else{
            SceneManager.LoadScene("Lobi");
        }
    }

    public void SaveNewNickname(string userNickname, string userEmail)
    {
        StartCoroutine(IESaveNickname(userNickname, userEmail));
    }

    IEnumerator IESaveNickname(string userNickname, string userEmail){
        WWWForm form = new WWWForm();
        form.AddField("nickname", userNickname);
        form.AddField("email", userEmail);
        UnityWebRequest webRequest = UnityWebRequest.Post(ipAddress+"/saveNickname.php", form);
        yield return webRequest.SendWebRequest();
        
        string result = webRequest.downloadHandler.text;

        Debug.Log("httpRequest; IESaveNickname;"+result);

        SceneManager.LoadScene("Lobi");
    }
}
