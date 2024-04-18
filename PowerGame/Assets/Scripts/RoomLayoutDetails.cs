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
            currentRoom.LockDoors(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        numberOfEnemies--;
        if(numberOfEnemies == 0)
        {
            Debug.Log("All Killed");//TODO add animation
            currentRoom.LockDoors(false);
        }
    }

    public void SetRoom(Room room)
    {
        currentRoom = room;
    }
}
