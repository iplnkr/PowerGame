using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    private static readonly int mapMax = 9;//maximum size of map
    [SerializeField] private Room demoRoom;
    [SerializeField] private GameObject miniMapPanel;
    [SerializeField] private Image demoUiRoom;
    private List<Room> roomList = new List<Room>();//list of created rooms
    private int[,] roomGrid = new int[mapMax, mapMax];//grid layout of room positions
    private Vector2 currentRoom = new Vector2((mapMax + 1) / 2, (mapMax + 1) / 2);
    [SerializeField] private Camera mainCam;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Color fullyMapped;
    [SerializeField] private Color semiMapped;

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

    //Generate the layout of the level by making rooms
    public void GenerateFloor(int rooms)
    {
        //make first 
        Room first = Instantiate(demoRoom);
        first.gameObject.SetActive(true);
        first.transform.parent = this.transform;
        first.StartRoom(1, new Vector2((mapMax + 1) / 2, (mapMax + 1) / 2));
        roomList.Add(first);
        roomGrid[(mapMax+1)/2, (mapMax+1)/2] = 1;
        //make first's minimap version
        Image minimapFirst = Instantiate(demoUiRoom);
        minimapFirst.transform.parent = miniMapPanel.transform;
        minimapFirst.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
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
            newRoom.transform.parent = this.transform;
            newRoom.StartRoom(i + 1, new Vector2(Mathf.RoundToInt(prevRoomPos.x + -Mathf.Cos(Mathf.Deg2Rad * dire * 90)), Mathf.RoundToInt(prevRoomPos.y + Mathf.Sin(Mathf.Deg2Rad * dire * 90))));
            roomList.Add(newRoom);
            roomGrid[Mathf.RoundToInt(prevRoomPos.x + -Mathf.Cos(Mathf.Deg2Rad * dire * 90)), Mathf.RoundToInt(prevRoomPos.y + Mathf.Sin(Mathf.Deg2Rad * dire * 90))] = i + 1;
            //make room's minimap version
            Image minimapnew = Instantiate(demoUiRoom);
            minimapnew.transform.parent = miniMapPanel.transform;
            minimapnew.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            //minimapnew.gameObject.SetActive(true);
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
        //wipe useless doors
        for(int i = 0; i < roomList.Count; i++)
        {
            roomList[i].DestroyUselessDoors();
        }
        //set up minimap first room
        MapNewTile(first);
    }

    //Map the current square and semi-map the adjacent squares
    private void MapNewTile(Room roomToMap)
    {
        MakeNearbyActive(roomToMap);
        //if room not already mapped
        if(!roomToMap.GetMinimap().GetComponent<Image>().color.Equals(fullyMapped))
        {
            //set current to fully mapped
            roomToMap.GetMinimap().gameObject.SetActive(true);
            roomToMap.GetMinimap().GetComponent<Image>().color = fullyMapped;//new Color(roomToMap.GetMinimap().GetComponent<Image>().color.r, roomToMap.GetMinimap().GetComponent<Image>().color.g, roomToMap.GetMinimap().GetComponent<Image>().color.b, 1);
            //set non-fully mapped adjacent's to be semi-mapped
            if(roomToMap.GetUp() != null)
            {
                if(!roomToMap.GetUp().GetMinimap().GetComponent<Image>().color.Equals(fullyMapped))
                {
                    roomToMap.GetUp().GetMinimap().gameObject.SetActive(true);
                    roomToMap.GetUp().GetMinimap().GetComponent<Image>().color = semiMapped;//new Color(roomToMap.GetUp().GetMinimap().GetComponent<Image>().color.r, roomToMap.GetUp().GetMinimap().GetComponent<Image>().color.g, roomToMap.GetUp().GetMinimap().GetComponent<Image>().color.b, 0.5f);
                }
            }
            if(roomToMap.GetRight() != null)
            {
                if(!roomToMap.GetRight().GetMinimap().GetComponent<Image>().color.Equals(fullyMapped))
                {                    
                    roomToMap.GetRight().GetMinimap().gameObject.SetActive(true);
                    roomToMap.GetRight().GetMinimap().GetComponent<Image>().color = semiMapped;//new Color(roomToMap.GetRight().GetMinimap().GetComponent<Image>().color.r, roomToMap.GetRight().GetMinimap().GetComponent<Image>().color.g, roomToMap.GetRight().GetMinimap().GetComponent<Image>().color.b, 0.5f);
                }
            }
            if(roomToMap.GetDown() != null)
            {
                if(!roomToMap.GetDown().GetMinimap().GetComponent<Image>().color.Equals(fullyMapped))
                {
                    roomToMap.GetDown().GetMinimap().gameObject.SetActive(true);
                    roomToMap.GetDown().GetMinimap().GetComponent<Image>().color = semiMapped;//new Color(roomToMap.GetDown().GetMinimap().GetComponent<Image>().color.r, roomToMap.GetDown().GetMinimap().GetComponent<Image>().color.g, roomToMap.GetDown().GetMinimap().GetComponent<Image>().color.b, 0.5f);
                }
            }
            if(roomToMap.GetLeft() != null)
            {
                if(!roomToMap.GetLeft().GetMinimap().GetComponent<Image>().color.Equals(fullyMapped))
                {
                    roomToMap.GetLeft().GetMinimap().gameObject.SetActive(true);
                    roomToMap.GetLeft().GetMinimap().GetComponent<Image>().color = semiMapped;//new Color(roomToMap.GetLeft().GetMinimap().GetComponent<Image>().color.r, roomToMap.GetLeft().GetMinimap().GetComponent<Image>().color.g, roomToMap.GetLeft().GetMinimap().GetComponent<Image>().color.b, 0.5f);
                }
            }
        }
    }

    //make only the nearby rooms active
    private void MakeNearbyActive(Room center)
    {
        //make all rooms inactive
        for(int i = 0; i < roomList.Count; i++)
        {
            roomList[i].gameObject.SetActive(false);
        }
        //make center active
        center.gameObject.SetActive(true);
        //neighbours too
        if(center.GetUp() != null)
        {
            center.GetUp().gameObject.SetActive(true);
        }
        if(center.GetRight() != null)
        {
            center.GetRight().gameObject.SetActive(true);
        }
        if(center.GetDown() != null)
        {
            center.GetDown().gameObject.SetActive(true);
        }
        if(center.GetLeft() != null)
        {
            center.GetLeft().gameObject.SetActive(true);
        }

    }

    //Move the camera and player to a new room
    public void MoveRoom(int direction)
    {
        StartCoroutine("MovePlayer", direction);
        StartCoroutine("MoveCamera", direction);
        currentRoom = new Vector2(Mathf.RoundToInt(currentRoom.x + -Mathf.Cos(Mathf.Deg2Rad * direction * 90)), Mathf.RoundToInt(currentRoom.y + Mathf.Sin(Mathf.Deg2Rad * direction * 90)));
        MapNewTile(roomList[roomGrid[(int)currentRoom.x,(int)currentRoom.y] - 1]);
    }

    private IEnumerator MoveCamera(int direction)
    {
        Vector2 uiDest = new Vector2((-roomList[roomGrid[Mathf.RoundToInt(currentRoom.x + -Mathf.Cos(Mathf.Deg2Rad * direction * 90)), Mathf.RoundToInt(currentRoom.y + Mathf.Sin(Mathf.Deg2Rad * direction * 90))] - 1].GetPosition().x + ((mapMax + 1) / 2)) * demoUiRoom.GetComponent<RectTransform>().sizeDelta.x, (-roomList[roomGrid[Mathf.RoundToInt(currentRoom.x + -Mathf.Cos(Mathf.Deg2Rad * direction * 90)), Mathf.RoundToInt(currentRoom.y + Mathf.Sin(Mathf.Deg2Rad * direction * 90))] - 1].GetPosition().y + ((mapMax + 1) / 2)) * demoUiRoom.GetComponent<RectTransform>().sizeDelta.y);
        Vector3 destPos = new Vector3(16*Mathf.RoundToInt(currentRoom.x + -Mathf.Cos(Mathf.Deg2Rad * direction * 90) - ((mapMax + 1) / 2)), 10*Mathf.RoundToInt(currentRoom.y + Mathf.Sin(Mathf.Deg2Rad * direction * 90) - ((mapMax + 1) / 2)), -10);
        for(int i = 0; i < 65; i++)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, destPos, 0.075f);
            miniMapPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(miniMapPanel.GetComponent<RectTransform>().anchoredPosition, uiDest, 0.075f);
            yield return new WaitForSeconds(0.01f);
        }
        mainCam.transform.position = destPos;
        miniMapPanel.GetComponent<RectTransform>().anchoredPosition = uiDest;
    }

    private IEnumerator MovePlayer(int direction)
    {
        Vector3 destPos = new Vector3((player.transform.position.x + 3.015f*-Mathf.Cos(Mathf.Deg2Rad * direction * 90)), (player.transform.position.y + 3.015f*Mathf.Sin(Mathf.Deg2Rad * direction * 90)), player.transform.position.z);
        player.GetComponent<BoxCollider2D>().enabled = false;
        for(int i = 0; i < 75; i++)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, destPos, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
        player.GetComponent<BoxCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}