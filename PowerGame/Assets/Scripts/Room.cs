using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    private static readonly int mapMax = 9;
    private Room up;
    private Room right;
    private Room down;
    private Room left;
    private int idNumber; //proper ids start at 1
    private Vector2 position;
    private Image minimapSelf;
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;
    [SerializeField] private GameObject door3;
    [SerializeField] private GameObject door4;

    //set starting values for a room
    public void StartRoom (int id, Vector2 pos)
    {
        SetId(id);
        SetPosition(pos);
    }

    //return an int representing a direction (u,r,d,l = 1,2,3,4) of a random empty neighbour(to find a place to put a new room)
    public int SelectEmptyDirection()
    {
        //create list of valid directions
        List<int> directions = new List<int>();
        if((up == null) && (position.y != mapMax - 1))// if no connection and not on map edge
        {
            directions.Add(1);
        }
        if((right == null) && (position.x != mapMax - 1))
        {
            directions.Add(2);
        }
        if((down == null) && (position.y != 0))
        {
            directions.Add(3);
        }
        if((left == null) && (position.x != 0))
        {
            directions.Add(4);
        }
        //return -1 if not possible
        if(directions.Count == 0)
        {
            return -1;
        }
        else
        {
            return directions[Random.Range(0,directions.Count)];
        }
    }

    public void DestroyMinimap()
    {
        Destroy(minimapSelf.gameObject);
    }

    //destroy all doors without something on other side
    public void DestroyUselessDoors()
    {
        if(up == null)
        {
            Destroy(door1);
        }
        if(right == null)
        {
            Destroy(door2);
        }
        if(down == null)
        {
            Destroy(door3);
        }
        if(left == null)
        {
            Destroy(door4);
        }
    }

    //lock/unlock all possible doors
    public void LockDoors(bool locking)
    {
        if(up != null)
        {
            door1.GetComponent<Door>().LockDoor(locking);
        }
        if(right != null)
        {
            door2.GetComponent<Door>().LockDoor(locking);
        }
        if(down != null)
        {
            door3.GetComponent<Door>().LockDoor(locking);
        }
        if(left != null)
        {
            door4.GetComponent<Door>().LockDoor(locking);
        }
    }

    //---------------------------------------------------general get and set functions

    public void SetUp(Room u)
    {
        up = u;
    }

    public void SetRight(Room r)
    {
        right = r;
    }

    public void SetDown(Room d)
    {
        down = d;
    }

    public void SetLeft(Room l)
    {
        left = l;
    }

    public void SetId(int id)
    {
        idNumber = id;
    }

    public void SetPosition(Vector2 pos)
    {
        position = pos;
        transform.position = new Vector3((pos.x - ((mapMax + 1) / 2)) * 16, (pos.y - ((mapMax + 1) / 2)) * 10, 0);
    }

    public void SetMinimap(Image map)
    {
        minimapSelf = map;
    }

    public Room GetUp()
    {
        return up;
    }

    public Room GetRight()
    {
        return right;
    }

    public Room GetDown()
    {
        return down;
    }

    public Room GetLeft()
    {
        return left;
    }

    public int GetId()
    {
        return idNumber;
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public Image GetMinimap()
    {
        return minimapSelf;
    }
}
