using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            Collect();
            Destroy(gameObject);
        }
    }

    public virtual void Collect()
    {

    }
}
