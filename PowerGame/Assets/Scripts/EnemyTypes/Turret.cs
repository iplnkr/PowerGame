using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [SerializeField] private GameObject bulletDemo;
     private PlayerMovement playerObj;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
    }

    public void ShootBullet()
    {
        //shoot a bullet towards the player
        Vector3 destination = Vector3.Normalize((playerObj.transform.position - transform.position) - new Vector3(0,0,(playerObj.transform.position - transform.position).z)) * 0.05f;
        GameObject curBullet = Instantiate(bulletDemo);
        curBullet.transform.right = destination;
        curBullet.transform.position = transform.position + 0.5f*destination + Vector3.up/2;
        curBullet.SetActive(true);
        curBullet.GetComponent<Bullet>().Fire();  
    }
    
}
