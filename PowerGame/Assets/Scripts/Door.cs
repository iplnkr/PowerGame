using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    private static float doorCooldown;//cooldown for all doors
    private bool isLeadingDoor = false;//only use one door to count the door cooldown
    private bool isOpen = true;
    [SerializeField] private int direction = 1;//u, r, d, l  =  1, 2, 3, 4
    [SerializeField] private LevelController floorGen;
    private float waitTime = 0.95f;
    
    //animations
    [SerializeField] private Animator animDark;
    [SerializeField] private Animator animLight;
    [SerializeField] private AudioSource doorCloseSound;
    [SerializeField] private AudioSource doorOpenSound;

    void FixedUpdate()
    {
        //put all doors on cooldown after a door used
        if(isLeadingDoor)
        {
            doorCooldown = doorCooldown - Time.fixedDeltaTime;
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

    public void LockDoor(bool locking)
    {
        isOpen = !locking;
        //do door animations
        if(locking)
        {
            animDark.Play("Base Layer._Door_Dark_Shut");
            animLight.Play("Base Layer._Door_Light_Shut");
        }
        else
        {
            animDark.Play("Base Layer.DoorDark");
            animLight.Play("Base Layer.DoorLight");
        }
    }

    //play door shutting sound
    public void ShutDoorSound()
    {
        if(doorCloseSound != null)
        {
            doorCloseSound.Play();
            DeeJay dj = FindObjectOfType<DeeJay>();
            if(dj != null)
            {
                dj.EnterCombat();
            }
        }
    }

    //play door opening sound
    public void OpenDoorSound()
    {
        if(doorOpenSound != null)
        {
            doorOpenSound.Play();
            DeeJay dj = FindObjectOfType<DeeJay>();
            if(dj != null)
            {
                dj.ExitCombat();
            }
        }
    }
}
