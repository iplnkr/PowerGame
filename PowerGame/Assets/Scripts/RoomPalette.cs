using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPalette : MonoBehaviour
{
    [SerializeField] private GameObject[] basicRoomLayouts;//all stored basic room layouts

    public GameObject GetRandomBasicRoomLayout()
    {
        return basicRoomLayouts[Random.Range(0, basicRoomLayouts.Length)];
    }
}
