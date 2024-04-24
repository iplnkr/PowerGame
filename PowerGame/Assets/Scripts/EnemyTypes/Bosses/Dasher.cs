using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : Enemy
{
    private PlayerMovement playerObj;
    private Vector3 playerMemPos;
    private float aimCooldown = 2;
    private float dashCooldown = 0;
    private int xMult = 1;
    private int yMult = 1;
    private bool followMode = false;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
        playerMemPos = playerObj.transform.position;
    }

    public override void FixedUpdateAddOn()
    {
        //aim every few seconds
        aimCooldown = aimCooldown - Time.fixedDeltaTime;
        if(aimCooldown <= 0.5f)
        {
            //do animations if not already
            if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.SpikeRollDark"))
            {                
                //animate ready to launch
                GetComponent<Animator>().Play("Base Layer.SpikeRollDark");
                lightSelf.GetComponent<Animator>().Play("Base Layer.SpikeRollLight");
            }
        }
        if(aimCooldown <= 0)
        {
            //set aim, then wait before 
            aimCooldown = 5f;
            playerMemPos = playerObj.transform.position;
            dashCooldown = 0.0025f;
            followMode = true;
        }

        //when in following mode
        if(followMode)
        {
            //wait until dash time
            dashCooldown = dashCooldown - Time.fixedDeltaTime;
            if(dashCooldown <= 0)
            {
                //move to the destination, but dont go through objects
                //scale speed on health
                Vector3 destination = Vector3.Normalize((playerMemPos - transform.position) - new Vector3(0,0,(playerMemPos - transform.position).z)) * Mathf.Max(2, 25f * (1.01f - (currentHealth/maxHealth))) * Time.fixedDeltaTime;
                transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
                //if reached, stop chasing
                if(Vector3.Distance(playerMemPos, transform.position) <= 0.5f)
                {
                    aimCooldown = 1.5f;
                    followMode = false;
                    //animate spike out
                    GetComponent<Animator>().Play("Base Layer.SpikeOutDark");
                    lightSelf.GetComponent<Animator>().Play("Base Layer.SpikeOutLight");
                }
            }
        }
    }

    //set size of hitbox
    public void SetColliderScale(float radius)
    {
        GetComponent<CircleCollider2D>().radius = radius;
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
