using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : Enemy
{
    [SerializeField] private GameObject bulletDemo;
     private PlayerMovement playerObj;
    private float shootcooldown = 1f;
    private int shotCounter = -1;
    [SerializeField] private int delay = 0;

    //patrol variables
    [SerializeField] private Vector3[] patrolArea;
    private int patrolTarget = 0;
    private int xMult = 1;
    private int yMult = 1;
    private bool angry = false;
    private float movSpeed = 2.5f;


    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
        //set up patrol
        //if the patrol area was not set, default to horizontal patrol
        if(patrolArea.Length == 0)
        {
            patrolArea = new Vector3[2];
            patrolArea[0] = new Vector3(transform.position.x - transform.localPosition.x + 7f, transform.position.y, transform.position.z);
            patrolArea[1] = new Vector3(transform.position.x - transform.localPosition.x - 7f, transform.position.y, transform.position.z);
        }
        else
        {
            //adjust for local position
            float conversionDiffx = transform.position.x - transform.localPosition.x;
            float conversionDiffy = transform.position.y - transform.localPosition.y;
            for(int i = 0; i < patrolArea.Length; i++)
            {
                patrolArea[i] = new Vector3(conversionDiffx + patrolArea[i].x, conversionDiffy + patrolArea[i].y, transform.position.z);
            }
        }
    }

    public override void FixedUpdateAddOn()
    {
        //check if should become angry
        if(!angry)
        {
            if(FindObjectsOfType<BulletHell>().Length < 2)
            {
                angry = true;
                movSpeed = movSpeed * 2;
            }
        }

        //do shooting
        shootcooldown = shootcooldown - Time.fixedDeltaTime;
        if(shootcooldown <= 0)
        {
            shootcooldown = 0.3f;
            shotCounter = (shotCounter + 1) % 20;
            if((shotCounter == (0 + delay) % 20) || (shotCounter == (2 + delay) % 20))
            {                
                ShootBullets();
            }
            if(shotCounter == (1 + delay) % 20)
            {                
                ShootAntiBullets();
            }
            //if other copy dead, shoot twice
            if(angry)
            {
                if((shotCounter == (10 + delay) % 20) || (shotCounter == (12 + delay) % 20))
                {                
                    ShootBullets();
                }
                if(shotCounter == (11 + delay) % 20)
                {                
                    ShootAntiBullets();
                }
            }
        }
        
        //do patrolling
        //follow patrol route, but dont go through objects
        Vector3 destination = Vector3.Normalize((patrolArea[patrolTarget] - transform.position) - new Vector3(0,0,(patrolArea[patrolTarget] - transform.position).z)) * movSpeed * Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
        transform.right = -destination;
        //if reached, set next destination
        if(Vector3.Distance(patrolArea[patrolTarget], transform.position) <= 0.1f)
        {
            patrolTarget = (patrolTarget + 1)%(patrolArea.Length);
        }
    }

    //shoot bullets in first pattern
    public void ShootBullets()
    {
        FireBullet(0);
        FireBullet(45);
        FireBullet(-45);
    }

    //shoot bullets in second pattern
    public void ShootAntiBullets()
    {
        FireBullet(22.5f);
        FireBullet(-22.5f);
    }

    //fire a single bullet
    private void FireBullet(float ang)
    {
        Quaternion angle = Quaternion.Euler(0, 0, ang);
        //shoot a bullet towards the player
        Vector3 destination = Vector3.Normalize((playerObj.transform.position - transform.position) - new Vector3(0,0,(playerObj.transform.position - transform.position).z)) * 0.05f;
        destination = angle * destination;
        GameObject curBullet = Instantiate(bulletDemo);
        curBullet.transform.right = destination;
        curBullet.transform.position = transform.position + 0.5f*destination;
        curBullet.SetActive(true);
        curBullet.GetComponent<Bullet>().Fire(); 
    }

    //patrol blocking by walls-----------------------------------------

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
