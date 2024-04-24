using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBarVanish : MonoBehaviour
{
    //delete when no enemies detected in room
    void FixedUpdate()
    {
        if(FindObjectsOfType<Enemy>().Length == 0)
        {
            Destroy(gameObject);
        }
    }
}
