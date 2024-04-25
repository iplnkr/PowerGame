using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodentKing : Enemy
{
    private PlayerMovement playerObj;
    private float cooldown = 0;
    private int xMult = 1;
    private int yMult = 1;
    
    private float spawningcooldown = 1f;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject doomService;
    private int spawnCounter = 0;
    private float speed = 1f;

    public override void StartAddOn()
    {
        playerObj = FindObjectOfType<PlayerMovement>();
        //if player on evil room service route, activate room service
        if(playerObj.GetEvil())
        {
            doomService.SetActive(true);
        }
    }

    public override void FixedUpdateAddOn()
    {
        cooldown = cooldown - Time.fixedDeltaTime;
        spawningcooldown = spawningcooldown - Time.fixedDeltaTime;
        if(cooldown <= 0)
        {
            //how to move when under torch
            if(toBurn)
            {
                //slow movement under torch, but faster spawns
                speed = 0.25f;
                spawningcooldown = spawningcooldown - Time.fixedDeltaTime;
            }
            else if(toHeal)//how to move when healing
            {
                speed = 1f;
                spawningcooldown = spawningcooldown + (Time.fixedDeltaTime / 3f);//slow down spawn to 2/3 speed of normal rate
            }
            else
            {
                speed = 1f;
            }
            //follow player, but dont go through objects
            Vector3 destination = Vector3.Normalize((playerObj.transform.position - transform.position) - new Vector3(0,0,(playerObj.transform.position - transform.position).z)) * speed * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x + destination.x, transform.position.y + destination.y, transform.position.z);
        }
        if(toDamagePlayer)
        {
            cooldown = 1.5f;
        }
        //spawn minions (spawn pattern: rat x 4, bat x 1)
        if(spawningcooldown <= 0)
        {
            spawningcooldown = 4f;
            spawnCounter = (spawnCounter + 1) % 5;
            GameObject newMinion; 
            if(spawnCounter == 0)
            {
                newMinion = Instantiate(bat);
            }
            else
            {
                newMinion = Instantiate(rat);
            }
            newMinion.transform.parent = transform.parent;
            newMinion.transform.position = transform.position;
            newMinion.SetActive(true);
        }
    }
    
}
