using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyNicknameCarrier : MonoBehaviour
{
    private static GameObject NicknameCarrier;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(NicknameCarrier == null){
            NicknameCarrier = gameObject;
        } else{
            DestroyObject(gameObject);
        }
    }
}
