using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVDBouncer : Enemy
{
    private float topbottomLimit = 3.5f;
    private float sideLimit = 6.5f;

    private float speedSlide = 0.075f;
    private float xMovement = 1;
    private float yMovement = 1;
    
    public override void StartAddOn()
    {
        xMovement = ((Random.Range(0, 2) * 2) - 1) * speedSlide;
        yMovement = ((Random.Range(0, 2) * 2) - 1) * speedSlide;
        if(xMovement < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public override void FixedUpdateAddOn()
    {
        transform.position = transform.position + new Vector3(xMovement, yMovement, 0) * Time.fixedDeltaTime * 50;
        
        if(Mathf.Abs(transform.localPosition.x) >= sideLimit)
        {
            xMovement = xMovement * -1;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if(Mathf.Abs(transform.localPosition.y) >= topbottomLimit)
        {
            yMovement = yMovement * -1;
        }
        
    }

    public override void OnTriggerEnter2DAddOn(Collider2D col)
    {
        //dont bounce on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            Vector3 colDir = Vector3.Normalize(col.transform.position - transform.position);
            //bounce based on angle to a collider and distance
            if((Mathf.Sign(colDir.x) == Mathf.Sign(xMovement)) && (Mathf.Abs(colDir.x) > transform.lossyScale.x/2))
            {
                xMovement = xMovement * -1;
            }
            if((Mathf.Sign(colDir.y) == Mathf.Sign(yMovement)) && (Mathf.Abs(colDir.y) > transform.lossyScale.y/2))
            {
                yMovement = yMovement * -1;
            }
        }
    }
}
