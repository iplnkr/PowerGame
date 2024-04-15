using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorGenerator : MonoBehaviour
{

    private static readonly int mapMax = 9;//maximum size of map
    [SerializeField] private Room demoRoom;
    [SerializeField] private Image miniMap;
    [SerializeField] private Image demoUiRoom;
    private List<Room> roomList = new List<Room>();//list of created rooms
    private int[,] roomGrid = new int[mapMax, mapMax];//grid layout of room positions

    // Start is called before the first frame update
    void Start()
    {
        GenerateFloor(23);
    }

    public void ResetFloor()
    {
        //destroy all rooms
        while(roomList.Count > 0)
        {
            roomList[roomList.Count - 1].DestroyMinimap();
            Destroy(roomList[roomList.Count - 1].gameObject);
            roomList.RemoveAt(roomList.Count - 1);
        }
        roomList.Clear();
        roomGrid = new int[mapMax, mapMax];
    }

    public void GenerateFloor(int rooms)
    {
        //make first 
        Room first = Instantiate(demoRoom);
        first.gameObject.SetActive(true);
        first.StartRoom(1, new Vector2((mapMax + 1) / 2, (mapMax + 1) / 2));
        roomList.Add(first);
        roomGrid[(mapMax+1)/2, (mapMax+1)/2] = 1;
        //make first's minimap version
        Image minimapFirst = Instantiate(demoUiRoom);
        minimapFirst.transform.parent = miniMap.transform.GetChild(0);
        minimapFirst.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        minimapFirst.gameObject.SetActive(true);
        minimapFirst.GetComponent<RectTransform>().anchoredPosition  = new Vector2(0, 0);
        first.SetMinimap(minimapFirst);
        //Make all other rooms
        for(int i = 1; i < rooms; i++)
        {
            //try randomly looking for empty slots to put rooms in, by picking a random room and getting the direction of an empty slot
            int selectedRoomId = -1;
            int dire = -1;
            while(dire == -1)
            {
                selectedRoomId = Random.Range(0, roomList.Count);
                dire = roomList[selectedRoomId].SelectEmptyDirection();
            }
            //create the new room
            Vector2 prevRoomPos = roomList[selectedRoomId].GetPosition();
            Room newRoom = Instantiate(demoRoom);
            newRoom.gameObject.SetActive(true);
            newRoom.StartRoom(i + 1, new Vector2(Mathf.RoundToInt(prevRoomPos.x + -Mathf.Cos(Mathf.Deg2Rad * dire * 90)), Mathf.RoundToInt(prevRoomPos.y + Mathf.Sin(Mathf.Deg2Rad * dire * 90))));
            roomList.Add(newRoom);
            roomGrid[Mathf.RoundToInt(prevRoomPos.x + -Mathf.Cos(Mathf.Deg2Rad * dire * 90)), Mathf.RoundToInt(prevRoomPos.y + Mathf.Sin(Mathf.Deg2Rad * dire * 90))] = i + 1;
            //make room's minimap version
            Image minimapnew = Instantiate(demoUiRoom);
            minimapnew.transform.parent = miniMap.transform.GetChild(0);
            minimapnew.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            minimapnew.gameObject.SetActive(true);
            minimapnew.GetComponent<RectTransform>().anchoredPosition = new Vector2((newRoom.GetPosition().x - ((mapMax + 1) / 2)) * demoUiRoom.GetComponent<RectTransform>().sizeDelta.x, (newRoom.GetPosition().y - ((mapMax + 1) / 2)) * demoUiRoom.GetComponent<RectTransform>().sizeDelta.y);
            newRoom.SetMinimap(minimapnew);
            //make connections to new room
            if(newRoom.GetPosition().y != mapMax - 1)//if not at top edge
            {
                int testIndex = roomGrid[Mathf.RoundToInt(newRoom.GetPosition().x), Mathf.RoundToInt(newRoom.GetPosition().y+1)];
                if(testIndex > 0)//if a room is present above
                {
                    newRoom.SetUp(roomList[testIndex-1]);
                    roomList[testIndex-1].SetDown(newRoom);
                }
            }
            if(newRoom.GetPosition().x != mapMax - 1)//if not at right edge
            {
                int testIndex = roomGrid[Mathf.RoundToInt(newRoom.GetPosition().x+1), Mathf.RoundToInt(newRoom.GetPosition().y)];
                if(testIndex > 0)//if a room is present to the right
                {
                    newRoom.SetRight(roomList[testIndex-1]);
                    roomList[testIndex-1].SetLeft(newRoom);
                }
            }
            if(newRoom.GetPosition().y != 0)//if not at bottom edge
            {
                int testIndex = roomGrid[Mathf.RoundToInt(newRoom.GetPosition().x), Mathf.RoundToInt(newRoom.GetPosition().y-1)];
                if(testIndex > 0)//if a room is present below
                {
                    newRoom.SetDown(roomList[testIndex-1]);
                    roomList[testIndex-1].SetUp(newRoom);
                }
            }
            if(newRoom.GetPosition().x != 0)//if not at left edge
            {
                int testIndex = roomGrid[Mathf.RoundToInt(newRoom.GetPosition().x-1), Mathf.RoundToInt(newRoom.GetPosition().y)];
                if(testIndex > 0)//if a room is present to the left
                {
                    newRoom.SetLeft(roomList[testIndex-1]);
                    roomList[testIndex-1].SetRight(newRoom);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
