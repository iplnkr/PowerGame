using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    [SerializeField] private GameObject eyeball1;
    [SerializeField] private GameObject eyeball2;
    //
    [SerializeField] private GameObject pivot1;
    [SerializeField] private GameObject pivot2;
    //
    [SerializeField] private GameObject iris1;
    [SerializeField] private GameObject iris2;

    void FixedUpdate()
    {
        //face self and pivots at mouse
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        transform.up = -mouseWorldPosition + transform.position;
        pivot1.transform.up = -mouseWorldPosition + pivot1.transform.position;
        pivot2.transform.up = -mouseWorldPosition + pivot2.transform.position;        
        //basic bounce animation
        if(Vector3.Distance(transform.position, mouseWorldPosition) >= 0.4f)
        {
            //transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.05f * Mathf.Sin(Time.timeSinceLevelLoad * 8), transform.position.z);
            eyeball1.transform.position = new Vector3(eyeball1.transform.parent.position.x, 0.1f + eyeball1.transform.parent.position.y + 0.05f * Mathf.Sin(Time.timeSinceLevelLoad * 8), eyeball1.transform.position.z);
            eyeball2.transform.position = new Vector3(eyeball2.transform.parent.position.x, 0.1f + eyeball2.transform.parent.position.y + 0.05f * Mathf.Sin(Time.timeSinceLevelLoad * 8 + 0.5f), eyeball2.transform.position.z);
        }
        //fix pivot's z pos
        pivot1.transform.position = new Vector3(pivot1.transform.position.x, pivot1.transform.position.y, eyeball1.transform.position.z - 0.005f);
        pivot2.transform.position = new Vector3(pivot2.transform.position.x, pivot2.transform.position.y, eyeball2.transform.position.z - 0.005f);
        //keep eyeballs stable and on correct overlap order
        eyeball1.transform.up = Vector3.up;
        eyeball2.transform.up = Vector3.up;
        if(eyeball1.transform.position.y < eyeball2.transform.position.y)
        {
            eyeball1.GetComponent<SpriteRenderer>().sortingOrder = 2;
            eyeball1.GetComponent<SpriteMask>().frontSortingOrder = 3;
            eyeball1.GetComponent<SpriteMask>().backSortingOrder = 2;
            iris1.GetComponent<SpriteRenderer>().sortingOrder = 2;
            eyeball2.GetComponent<SpriteRenderer>().sortingOrder = 0;
            eyeball2.GetComponent<SpriteMask>().frontSortingOrder = 1;
            eyeball2.GetComponent<SpriteMask>().backSortingOrder = 0;
            iris2.GetComponent<SpriteRenderer>().sortingOrder = 0;
            eyeball1.transform.localScale = Vector3.one;
            eyeball2.transform.localScale = Vector3.one * 0.9f;
        }
        else
        {
            eyeball2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            eyeball2.GetComponent<SpriteMask>().frontSortingOrder = 3;
            eyeball2.GetComponent<SpriteMask>().backSortingOrder = 2;
            iris2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            eyeball1.GetComponent<SpriteRenderer>().sortingOrder = 0;
            eyeball1.GetComponent<SpriteMask>().frontSortingOrder = 1;
            eyeball1.GetComponent<SpriteMask>().backSortingOrder = 0;
            iris1.GetComponent<SpriteRenderer>().sortingOrder = 0;
            eyeball1.transform.localScale = Vector3.one * 0.9f;
            eyeball2.transform.localScale = Vector3.one;
        }
        //keep irises stable        
        iris1.transform.up = Vector3.up;
        iris2.transform.up = Vector3.up;
    }
}
