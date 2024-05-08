using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeeJay : MonoBehaviour
{
    
    [SerializeField] private AudioSource levelMusicCalm;
    [SerializeField] private AudioSource levelMusicCombat;
    [SerializeField] private AudioSource winMusic;
    [SerializeField] private AudioSource shutDownSound;
    [SerializeField] private bool fadeIn = true;

    // Start is called before the first frame update
    void Start()
    {
        levelMusicCalm.mute = false;
        if((levelMusicCombat != null) && (winMusic != null))
        {
            levelMusicCombat.mute = true;            
            winMusic.mute = true;
        }
        levelMusicCalm.Play();
        if((levelMusicCombat != null) && (winMusic != null))
        {
            levelMusicCombat.Play();
            winMusic.Play();
        }
        if(fadeIn)
        {
            StartCoroutine("FadeIn");
        }
    }

    void Update()
    {
        //mute/unmute combo
        if ((Input.GetKey("left shift")) || (Input.GetKey("right shift")))
        {
            if(Input.GetKeyDown("m"))
            {
                AudioListener.volume = 1 - AudioListener.volume;
            }
        }
    }

    private IEnumerator FadeIn()
    {
        levelMusicCalm.volume = 0;
        while(levelMusicCalm.volume < 1)
        {
            levelMusicCalm.volume = levelMusicCalm.volume + 0.0035f;
            yield return new WaitForFixedUpdate();
        }
        levelMusicCalm.volume = 1;
    }
    
    public void EnterCombat()
    {
        levelMusicCalm.mute = true;
        levelMusicCombat.mute = false;
        winMusic.mute = true;
    }

    public void ExitCombat()
    {
        levelMusicCalm.mute = false;
        levelMusicCombat.mute = true;
        winMusic.mute = true;
    }

    public void WinMusic()
    {
        levelMusicCalm.mute = true;
        levelMusicCombat.mute = true;
        winMusic.mute = false;
    }

    public void PlayShutdown()
    {
        if(shutDownSound != null)
        {
            levelMusicCalm.mute = true;
            if((levelMusicCombat != null) && (winMusic != null))
            {
                levelMusicCombat.mute = true;
                winMusic.mute = true;
            }
            shutDownSound.Play();
        }
    }
}
