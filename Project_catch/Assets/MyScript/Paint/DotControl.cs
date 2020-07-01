using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotControl : MonoBehaviour
{
    public KeyCode touchTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver(){
        if(PaintGM.toolType == "eraser"){
            Destroy(gameObject);
        }
    }
}
