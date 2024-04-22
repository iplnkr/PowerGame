using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPalette : MonoBehaviour
{
    [SerializeField] private GameObject[] basicRoomLayouts;//all stored basic room layouts
    [SerializeField] private GameObject[] mediumRoomLayouts;//all stored medium room layouts
    [SerializeField] private GameObject[] hardRoomLayouts;//all stored hard room layouts
    [SerializeField] private GameObject[] exitRoomLayouts;//all stored exit room layouts
    [SerializeField] private GameObject[] specialRoomLayouts;//all stored special room layouts

    public GameObject GetRandomBasicRoomLayout()
    {
        return basicRoomLayouts[Random.Range(0, basicRoomLayouts.Length)];
    }

    public GameObject GetRandomMediumRoomLayout()
    {
        return basicRoomLayouts[Random.Range(0, mediumRoomLayouts.Length)];
    }

    public GameObject GetRandomHardRoomLayout()
    {
        return basicRoomLayouts[Random.Range(0, hardRoomLayouts.Length)];
    }

    public GameObject GetExitRoomLayout(int index)
    {
        return exitRoomLayouts[index];
    }

    public GameObject GetSpecialRoomLayout(int index)
    {
        return specialRoomLayouts[index];
    }
}
