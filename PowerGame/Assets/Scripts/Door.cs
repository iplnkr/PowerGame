using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    private static float doorCooldown;//cooldown for all doors
    private bool isLeadingDoor = false;//only use one door to count the door cooldown
    private bool isOpen = true;
    [SerializeField] private int direction = 1;//u, r, d, l  =  1, 2, 3, 4
    [SerializeField] private FloorGenerator floorGen;
    private float waitTime = 0.95f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //put all doors on cooldown after a door used
        if(isLeadingDoor)
        {
            doorCooldown = doorCooldown - Time.fixedDeltaTime;
            //Debug.Log(doorCooldown);
            if(doorCooldown <= 0)
            {
                isLeadingDoor = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            //if the door is open and doors are not on cooldown
            if(isOpen)
            {
                if(doorCooldown <= 0)
                {
                    //if heading through door
                    if(((Input.GetAxisRaw("Vertical") == Mathf.Sin(Mathf.Deg2Rad * direction * 90)) && (Input.GetAxisRaw("Vertical") != 0)) || ((Input.GetAxisRaw("Horizontal") == -Mathf.Cos(Mathf.Deg2Rad * direction * 90)) && (Input.GetAxisRaw("Horizontal") != 0)))
                    {
                        doorCooldown = waitTime;
                        isLeadingDoor = true;
                        col.GetComponent<PlayerMovement>().SetMovementCooldown(waitTime);
                        floorGen.MoveRoom(direction);
                    }
                }
            }
        }
    }
}
