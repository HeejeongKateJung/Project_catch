using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyIdCarrier : MonoBehaviour
{
    private static GameObject IdCarrier;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(IdCarrier == null){
            IdCarrier = gameObject;
        } else{
            DestroyObject(gameObject);
        }
    }
}
