using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    [SerializeField] private GameObject globalLight;
    private bool used = false;
    [SerializeField] private AudioSource clickSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            if(!used)
            {
                used = true;
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                globalLight.SetActive(true);
                col.GetComponent<PlayerMovement>().WinGame();
                //play switch sound
                if(clickSound != null)
                {
                    clickSound.Play();
                }
                //play win music
                DeeJay dj = FindObjectOfType<DeeJay>();
                if(dj != null)
                {
                    dj.WinMusic();
                }
            }
        }
    }
}
