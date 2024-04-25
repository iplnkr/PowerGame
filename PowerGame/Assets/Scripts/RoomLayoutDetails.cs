using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutDetails : MonoBehaviour
{
    //how many enemies are in the room
    public int numberOfEnemies = 1;
    private Room currentRoom;
    // Start is called before the first frame update
    void Start()
    {
        numberOfEnemies = FindObjectsOfType<Enemy>().Length;
        if(numberOfEnemies == 0)
        {
            if(currentRoom != null)
            {
                currentRoom.LockDoors(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //force open all doors (in case of some softlock bug appearing)
        if((Input.GetKey("1")) && (Input.GetKey("0")) && (Input.GetKey("z")) && (Input.GetKey("m")))
        {
            if(currentRoom != null)
            {
                currentRoom.LockDoors(false);
            }
        }
    }

    public void Death()
    {
        DoubleCheck();
        if(numberOfEnemies > 0)
        {
            //check again in a second to avoid bug of when multiple enemies killed simulataneously
            Invoke("DoubleCheck", 1);
        }
    }

    private void DoubleCheck()
    {
        numberOfEnemies = FindObjectsOfType<Enemy>().Length;
        if(numberOfEnemies <= 0)
        {
            numberOfEnemies = 0;
            currentRoom.LockDoors(false);
        }
    }

    public void SetRoom(Room room)
    {
        currentRoom = room;
    }
}
