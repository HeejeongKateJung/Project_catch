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
    public Text text;

    void Awake()
    {
        
        // PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        // PlayGamesPlatform.DebugLogEnabled = true;
        // PlayGamesPlatform.Activate();

        text.text = "no Login";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnLogin(){
        // if(!Social.localUser.authenticated){
        //     Social.localUser.Authenticate((bool bSuccess) =>
        //     {
        //         if(bSuccess){
        //             Debug.Log("Success : " + Social.localUser.userName);
        //             text.text = "로그인 성공";

                    SceneManager.LoadScene("Lobi");
                    
        //         }else{
        //             Debug.Log("Fail");
        //             text.text = "Fail";
        //         }
        //     });
        // }
    }

}
