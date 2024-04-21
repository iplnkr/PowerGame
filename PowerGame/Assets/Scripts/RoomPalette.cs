using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPalette : MonoBehaviour
{
    [SerializeField] private GameObject[] basicRoomLayouts;//all stored basic room layouts
    [SerializeField] private GameObject[] exitRoomLayouts;//all stored exit room layouts

    public GameObject GetRandomBasicRoomLayout()
    {
        return basicRoomLayouts[Random.Range(0, basicRoomLayouts.Length)];
    }

    public GameObject GetExitRoomLayout(int index)
    {
        return exitRoomLayouts[index];
    }
}
