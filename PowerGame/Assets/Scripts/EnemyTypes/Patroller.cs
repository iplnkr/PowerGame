using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : Enemy
{
    [SerializeField] private Vector3[] patrolArea;
    private int patrolTarget = 0;
    private int xMult = 1;
    private int yMult = 1;

    public override void StartAddOn()
    {
        //if the patrol area was not set, default to horizontal patrol
        if(patrolArea.Length == 0)
        {
            patrolArea = new Vector3[2];
            patrolArea[0] = new Vector3(transform.position.x - transform.localPosition.x + 7f, transform.position.y, transform.position.z);
            patrolArea[1] = new Vector3(transform.position.x - transform.localPosition.x - 7f, transform.position.y, transform.position.z);
            if(patrolArea[0].x < transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public override void FixedUpdateAddOn()
    {
        //follow patrol route, but dont go through objects
        Vector3 destination = Vector3.Normalize((patrolArea[patrolTarget] - transform.position) - new Vector3(0,0,(patrolArea[patrolTarget] - transform.position).z)) * 4f * Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
        //if reached, set next destination
        if(Vector3.Distance(patrolArea[patrolTarget], transform.position) <= 0.5f)
        {
            patrolTarget = (patrolTarget + 1)%(patrolArea.Length);
            
            if(patrolArea[patrolTarget].x < transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public override void OnTriggerEnter2DAddOn(Collider2D col)
    {
        //dont block on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            Vector3 colDir = Vector3.Normalize(col.transform.position - transform.position);
            Vector3 movDir = Vector3.Normalize(patrolArea[patrolTarget] - transform.position);
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
