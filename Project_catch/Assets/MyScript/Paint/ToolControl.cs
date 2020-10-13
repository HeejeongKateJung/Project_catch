using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControl : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickEraser()
    {
        PaintGM.toolType = "eraser";
        Debug.Log("eraser selected");
    }

    public void OnClickPencil()
    {
        PaintGM.toolType = "pencil";
        Debug.Log("pencil selected");
    }
}
