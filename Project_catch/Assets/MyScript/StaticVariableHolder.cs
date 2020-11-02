using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariableHolder : MonoBehaviour
{

    public string USER_ID = "hee4686";
    public string USER_NICKNAME = "기보니";
    public int USER_SCORE = 0;
    public string ipAddress = "35.243.93.175";
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }



}
