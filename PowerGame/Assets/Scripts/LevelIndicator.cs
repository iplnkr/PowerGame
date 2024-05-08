using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private Image[] circles;
    [SerializeField] private Text leveltext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevelIndication(string levelName)
    {
        //if there are enough circles
        if(circles.Length >= 7)
        {
            //reset color of all
            for(int i = 0; i < circles.Length; i++)
            {
                circles[i].color = new Color(0.8f, 0.8f, 0.8f, 0.4f);
            } 
            //set name and color
            leveltext.text = levelName;
            switch (levelName)
            {
                case "Your Room":
                    circles[0].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "3rd Floor":
                    circles[1].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "3rd Floor Stairwell":
                    circles[2].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "2nd Floor":
                    circles[3].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "2nd Floor Stairwell":
                    circles[4].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "1st Floor":
                    circles[5].color = new Color(1, 1, 1, 0.6f);
                    break;
                case "1st Floor Stairwell":
                    circles[6].color = new Color(1, 1, 1, 0.6f);
                    break;
            }
        }
    }
}
