using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private int collectableId = 0;//0 = default, 1 = coin, 2 = heart, 3 = torchDuration
    [SerializeField] private int cost = 0;
    [SerializeField] private AudioSource collectSound;

    void Start()
    {
        //player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col != null)
        {
            if(col.GetComponent<PlayerMovement>() != null)
            {
                player = col.GetComponent<PlayerMovement>();
                if(player.CanLoseGold(cost))
                {
                    player.LoseGold(cost);
                    //play collect sound
                    if(collectSound != null)
                    {
                        //create a temporary object to play sound
                        GameObject audioHolder = new GameObject();
                        audioHolder.transform.position = transform.position;
                        audioHolder.transform.parent = null;
                        audioHolder.AddComponent<AudioSource>();
                        AudioSource audSou = audioHolder.GetComponent<AudioSource>();
                        audSou.clip = collectSound.clip;
                        audSou.playOnAwake = false;
                        audSou.volume = 0.25f;
                        audSou.Play();
                        Destroy(audioHolder, 5);
                    }
                    Collect();
                    Destroy(gameObject);
                }
            }
        }
    }

    //do collect functionality
    public virtual void Collect()
    {
        if(player != null)
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
}
