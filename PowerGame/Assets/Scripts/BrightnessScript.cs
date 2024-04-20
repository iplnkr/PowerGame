using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrightnessScript : MonoBehaviour
{
    private int min = 50;
    private int max = 255;
    private static float brightness = 50;
    private Tilemap floorToCol;

    void Start()
    {
        floorToCol = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        //buttons for editing brightness
        if(Input.GetKey("o"))
        {
            brightness-=0.1f;
            if(brightness < min)
            {
                brightness = min;
            }
        }
        if(Input.GetKey("p"))
        {
            brightness+=0.1f;
            if(brightness > max)
            {
                brightness = max;
            }
        }
    }

    void FixedUpdate()
    {
        floorToCol.color = new Color(brightness/255f, brightness/255f, brightness/255f, 1);
    }
}
