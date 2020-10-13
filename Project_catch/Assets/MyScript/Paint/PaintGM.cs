using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGM : MonoBehaviour
{
    public Transform baseDot;
    public KeyCode touchTrigger;
    public static string toolType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 touchPosition = new Vector2(
            Input.mousePosition.x, 
            Input.mousePosition.y);

        Vector2 objPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        if (Input.GetKey(touchTrigger))
        {
            Instantiate(baseDot, objPosition, baseDot.rotation);
        }
    }
}
