using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{
    
    bool bWait = false;
    public Text WarningText;
    public GameObject HttpManager;
    public GameObject IdCarrier;
    void Awake()
    {

        //로그인 시 토큰을 받기위해 구글 플레이 컨피그를 만들어둔다
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            // .RequestServerAuthCode(false)
            .RequestEmail()
            // .RequestIdToken()
            .Build();
        
        //GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

    }
    
    public void OnLogin(){
        GoogleAuth();
    }

    private void GoogleAuth(){
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Debug.Log("구글 로그인 실패");
                    return;
                }

                // 로그인이 성공되었습니다.
                string email = ((PlayGamesLocalUser)Social.localUser).Email;
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("UserName - " + Social.localUser.userName);
                Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());

                IdCarrier.GetComponent<Text>().text = email;
                Debug.Log("IdCarrier 에 " + IdCarrier.GetComponent<Text>().text + "담김");

                //HTTP Manager 에게 현재 로그인한 유저가 첫번째 유저인지 체크한다.
                HttpManager.GetComponent<HttpRequest>().CheckFirstLogin(email);
            });
        }
        // SceneManager.LoadScene("Lobi");
    }

    public void OnLogout(){
        ((PlayGamesPlatform)Social.Active).SignOut();
        WarningText.GetComponent<Text>().text = "Logout";
    }
    


    private string GetTokens(){
        if(PlayGamesPlatform.Instance.localUser.authenticated){
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            return _IDtoken;
        }else{
            Debug.Log("접속되어 있지 않으므로 다시 시도");
            GoogleAuth();
            return null;
        }
    }

}
