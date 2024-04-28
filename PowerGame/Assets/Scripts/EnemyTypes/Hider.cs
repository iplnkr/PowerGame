using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : Enemy
{
    private PlayerMovement playerObj;
    private Vector3 strollTarget;
    private int xMult = 1;
    private int yMult = 1;
    private float topbottomLimit = 3.5f;
    private float sideLimit = 6.5f;
    private float walkingTime;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
        //set first destination
        walkingTime = 2 + (Random.Range(-10,11)/10f);
        float newX = Mathf.Max(Mathf.Min(transform.localPosition.x + Random.Range(-2,3), sideLimit), -sideLimit);
        float newY = Mathf.Max(Mathf.Min(transform.localPosition.y + Random.Range(-2,3), topbottomLimit), -topbottomLimit);
        strollTarget = new Vector3(newX, newY, -1);
        //set facing direction
        if(newX - transform.localPosition.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public override void FixedUpdateAddOn()
    {
        //how to move when under torch
        if(toBurn >= 1)
        {
            //get mouse position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
            //compare angle for player to mouse and from player to this enemy
            Vector3 mouseDir = Vector3.Normalize(mouseWorldPosition - playerObj.transform.localPosition);
            Vector3 thisDir = Vector3.Normalize(transform.localPosition - playerObj.transform.localPosition);
            Vector3 angleDif = Vector3.Normalize(mouseDir - thisDir);
            if(Vector3.SignedAngle(mouseDir, thisDir, Vector3.forward) <= 0)
            {
                //rotate axis clockwise to pick a good direction to escape
                mouseDir = new Vector3(mouseDir.y, -mouseDir.x, transform.position.z);
                //follow escape route, but dont go through objects
                Vector3 destination = Vector3.Normalize(mouseDir) * 5f * Time.fixedDeltaTime;
                Vector3 proposedMove = new Vector3(transform.localPosition.x + destination.x * xMult, transform.localPosition.y + destination.y * yMult, transform.position.z);
                //ensure not out of bounds
                float newX = Mathf.Max(Mathf.Min(proposedMove.x, sideLimit), -sideLimit);
                float newY = Mathf.Max(Mathf.Min(proposedMove.y, topbottomLimit), -topbottomLimit);
                //set facing direction
                if(newX - transform.localPosition.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                //move
                transform.localPosition = new Vector3(newX, newY, -1);
                strollTarget = transform.localPosition;
            }
            else
            {
                //rotate axis anticlockwise to pick a good direction to escape
                mouseDir = new Vector3(-mouseDir.y, mouseDir.x, transform.position.z);
                //follow escape route, but dont go through objects
                Vector3 destination = Vector3.Normalize(mouseDir) * 5f * Time.fixedDeltaTime;
                Vector3 proposedMove = new Vector3(transform.localPosition.x + destination.x * xMult, transform.localPosition.y + destination.y * yMult, transform.localPosition.z);
                //ensure not out of bounds
                float newX = Mathf.Max(Mathf.Min(proposedMove.x, sideLimit), -sideLimit);
                float newY = Mathf.Max(Mathf.Min(proposedMove.y, topbottomLimit), -topbottomLimit);
                //set facing direction
                if(newX - transform.localPosition.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                //move                
                transform.localPosition = new Vector3(newX, newY, -1);
                strollTarget = transform.localPosition;
            }
        }
        else//when not under torch
        {
            walkingTime = walkingTime - Time.fixedDeltaTime;
            //follow random route, but dont go through objects
            Vector3 destination = Vector3.Normalize((strollTarget - transform.localPosition) - new Vector3(0,0,(strollTarget - transform.localPosition).z)) * 2.5f * Time.fixedDeltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x + destination.x * xMult, transform.localPosition.y + destination.y * yMult, transform.position.z);
            //if reached or time reached, set next destination
            if((Vector3.Distance(strollTarget, transform.localPosition) <= 0.1f) || (walkingTime <= 0))
            {
                walkingTime = 2 + (Random.Range(-10,11)/10f);
                float newX = Mathf.Max(Mathf.Min(transform.localPosition.x + Random.Range(-2,3), sideLimit), -sideLimit);
                float newY = Mathf.Max(Mathf.Min(transform.localPosition.y + Random.Range(-2,3), topbottomLimit), -topbottomLimit);
                strollTarget = new Vector3(newX, newY, -1);
                //set facing direction
                if(newX - transform.localPosition.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    public override void OnTriggerEnter2DAddOn(Collider2D col)
    {
        //dont block on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            Vector3 colDir = Vector3.Normalize(col.transform.localPosition - transform.localPosition);
            Vector3 movDir = Vector3.Normalize(strollTarget - transform.localPosition);
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
