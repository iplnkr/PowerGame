using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private int collectableId = 0;//0 = default, 1 = coin, 2 = heart, 3 = torchDuration

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            Collect();
            Destroy(gameObject);
        }
    }

    //do collect functionality
    public virtual void Collect()
    {
        switch (collectableId)
        {
            case 1:
                player.GainGold(1);
                break;
            case 2:
                player.GainHP(1);
                break;
            case 3:
                player.GainBattery();
                break;    
        }
    }
}
