using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomService : Enemy
{
    private PlayerMovement playerObj;
    private GameObject targetEnemy;//enemy chasing after
    private int xMult = 1;
    private int yMult = 1;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
    }

    public override void FixedUpdateAddOn()
    {
        //find all enemies
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        if(allEnemies.Length <= 1)
        {
            //follow player, but dont go through objects
            Vector3 destination = Vector3.Normalize((playerObj.transform.position - transform.position) - new Vector3(0,0,(playerObj.transform.position - transform.position).z)) * 2f * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
        }
        else
        {
            //follow enemies to heal them
            //find enemy to chase
            targetEnemy = FindLowest(allEnemies);
            //move towards closest enemy
            Vector3 destination = Vector3.Normalize((targetEnemy.transform.position - transform.position) - new Vector3(0,0,(targetEnemy.transform.position - transform.position).z)) * 5f * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
        }
    }

    //don't collide into enemies
    public override void OnTriggerEnter2DAddOn(Collider2D col)
    {
        if(targetEnemy != null)
        {
            //dont block on collision with torch and collectables
            if((!col.isTrigger) || (col.GetComponent<Enemy>() != null))
            {
                Vector3 colDir = Vector3.Normalize(col.transform.position - transform.position);
                Vector3 movDir = Vector3.Normalize(targetEnemy.transform.position - transform.position);
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
    }

    public override void OnTriggerExit2DAddOn(Collider2D col)
    {
        //dont block on collision with torch and collectables
        if((!col.isTrigger) || (col.GetComponent<Enemy>() != null))
        {
            xMult = 1;
            yMult = 1;
        }
    }

    //find closest, assuming at least 1 exists
    private GameObject FindClosest(Enemy[] arrayOfAll)
    {
        if(arrayOfAll.Length > 1)
        {
            Enemy closest = arrayOfAll[0];
            float dist = Vector3.Distance(transform.position, closest.transform.position);
            //exclude self
            if(arrayOfAll[0].Equals(gameObject.GetComponent<Enemy>()))
            {
                closest = arrayOfAll[1];
                dist = Vector3.Distance(transform.position, closest.transform.position);
            }
            for(int i = 1; i < arrayOfAll.Length; i++)
            {
                //exclude self
                if(!arrayOfAll[i].Equals(gameObject.GetComponent<Enemy>()))
                {
                    float newdist = Vector3.Distance(transform.position, arrayOfAll[i].transform.position);
                    if(newdist < dist)
                    {
                        closest = arrayOfAll[i];
                        dist = newdist;
                    }
                }
            }
            return closest.gameObject;
        }
        return null;
    }

    //find lowest in health, assuming at least 1 exists
    private GameObject FindLowest(Enemy[] arrayOfAll)
    {
        if(arrayOfAll.Length > 1)
        {
            Enemy lowest = arrayOfAll[0];
            float ratio = lowest.GetHealthPercent();
            //exclude self
            if(arrayOfAll[0].Equals(gameObject.GetComponent<Enemy>()))
            {
                lowest = arrayOfAll[1];
                ratio = lowest.GetHealthPercent();
            }
            for(int i = 1; i < arrayOfAll.Length; i++)
            {
                //exclude self
                if(!arrayOfAll[i].Equals(gameObject.GetComponent<Enemy>()))
                {
                    float newratio = arrayOfAll[i].GetHealthPercent();
                    if(newratio < ratio)
                    {
                        lowest = arrayOfAll[i];
                        ratio = newratio;
                    }
                }
            }
            return lowest.gameObject;
        }
        return null;
    }
}
