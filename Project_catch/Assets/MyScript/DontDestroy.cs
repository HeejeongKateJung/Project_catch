using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject tcpObject;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(tcpObject == null){
            tcpObject = gameObject;
        } else{
            DestroyObject(gameObject);
        }
    }
}
