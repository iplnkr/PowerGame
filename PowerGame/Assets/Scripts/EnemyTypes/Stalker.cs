using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    private PlayerMovement playerObj;
    private float cooldown = 0;
    private int xMult = 1;
    private int yMult = 1;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
    }

    public override void FixedUpdateAddOn()
    {
        cooldown = cooldown - Time.fixedDeltaTime;
        if(cooldown <= 0)
        {
            //how to move when under torch
            if(toBurn >= 1)
            {
                //stay still under torch
            }
            else
            {
                //follow player, but dont go through objects
                Vector3 destination = Vector3.Normalize((playerObj.transform.position - transform.position) - new Vector3(0,0,(playerObj.transform.position - transform.position).z)) * 2f * Time.fixedDeltaTime;
                transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
            }
        }
        if(toDamagePlayer)
        {
            cooldown = 1.5f;
        }
    }

    public override void OnTriggerEnter2DAddOn(Collider2D col)
    {
        //dont block on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            Vector3 colDir = Vector3.Normalize(col.transform.position - transform.position);
            Vector3 movDir = Vector3.Normalize(playerObj.transform.position - transform.position);
            Vector3 angleDif = Vector3.Normalize(colDir - movDir);
            //block direction based on collision
            if(Mathf.Abs(angleDif.x) <= Mathf.Abs(angleDif.y))
            {
                xMult = 0;
            }
            if(Mathf.Abs(angleDif.y) <= Mathf.Abs(angleDif.x))
            {
                yMult = 0;
            }
        }
    }

    public override void OnTriggerExit2DAddOn(Collider2D col)
    {
        //dont block on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            xMult = 1;
            yMult = 1;
        }
    }
}
