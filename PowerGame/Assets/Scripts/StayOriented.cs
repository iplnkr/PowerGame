using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOriented : MonoBehaviour
{
    private float xflip, yflip, zflip = 1;   
    
    //ensure this object stays upright
    void FixedUpdate()
    {
        xflip = Mathf.Sign(transform.lossyScale.x);
        yflip = Mathf.Sign(transform.lossyScale.y);
        zflip = Mathf.Sign(transform.lossyScale.z);
        transform.localScale = new Vector3(transform.localScale.x * xflip, transform.localScale.y * yflip, transform.localScale.z * zflip);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
