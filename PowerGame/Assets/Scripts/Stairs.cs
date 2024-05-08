using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stairs : MonoBehaviour
{
    [SerializeField] LevelController level;
    [SerializeField] private PlayerMovement player;
    [SerializeField] int levelToLoad;
    [SerializeField] private Image fadeRing;
    [SerializeField] private Image fadeCover;
    [SerializeField] private Text levelName;
    private bool used = false;//stop stairs from triggering more than once

    //dialogue
    [SerializeField] private string[] dialogue;//if there is dialogue between transitions
    [SerializeField] private Image dialogueBG;
    [SerializeField] private Text dialogueText;
    [SerializeField] private RoomService roomServiceGuy;
    private bool trySkip = false;
    private bool tryfullSkip = false;

    //audio
    [SerializeField] private AudioSource fadeInSound;
    [SerializeField] private AudioSource fadeOutSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource callSound;

    void Update()
    {
        if(Input.anyKeyDown)
        {
            trySkip = true;
        }
        if(!Input.anyKey)
        {
            trySkip = false;
        }
        //escape dialogue
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            tryfullSkip = true;
        }
    }

    void OnDisable()
    {        
        //if object disabled before text fades out, vanish text
        if(levelName != null)
        {
            levelName.color = new Color(1,1,1,0);
        }
    }

    public void PlayStartAnim()
    {
        StartCoroutine("FadeInAtStartAnim");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            if(!used)
            {
                used = true;
                //add dialogue change for alternate ending
                if(levelToLoad == -3)
                {
                    //check ending
                    if(roomServiceGuy != null)
                    {
                        if(roomServiceGuy.ShouldGoEvil())
                        {
                            player.ActivateEvil();
                            //make room service unsummonable
                            roomServiceGuy.GoEvil();
                            //add new dialogue
                            string[] dialogueAddOn = {"But you won't be getting much further I'm afraid...", "I will stop you...", "Because you don't deserve to be considered a hero...", "A real hero wouldn't tip so poorly..."};
                            //create merged array
                            string[] newDialogue = new string[dialogueAddOn.Length + dialogue.Length];
                            for(int i = 0; i < dialogue.Length; i++)
                            {
                                newDialogue[i] = dialogue[i];
                            }
                            for(int i = 0; i < dialogueAddOn.Length; i++)
                            {
                                newDialogue[dialogue.Length + i] = dialogueAddOn[i];
                            }
                            dialogue = newDialogue;
                        }
                    }
                }
                //do fade circle animation
                StartCoroutine("FadeOutAndInAnim");
            }
        }
    }

    IEnumerator FadeOutAndInAnim()
    {        
        //disable player movement
        player.SetMovementCooldown(100000);
        //fade out
        if(fadeOutSound != null)
        {
            fadeOutSound.Play();
        }
        fadeRing.gameObject.SetActive(true);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * 30;
        while(fadeRing.GetComponent<RectTransform>().localScale.x > 1)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x - 0.5f);
            //also move player towards stairs centre
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(transform.position.x, transform.position.y, -1), 0.05f);
            //yield return new WaitForSeconds(0.0005f);
            yield return new WaitForFixedUpdate();
        }
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        fadeCover.gameObject.SetActive(true);
        //yield return new WaitForSeconds(0.0005f);
        yield return new WaitForFixedUpdate();
        //detach this object so it isnt destroyed on loading
        transform.parent = null;
        //load next level
        level.ResetFloor();
        level.GenerateFloorForLevel(levelToLoad);

        //if there is dialogue, go through that first
        if(dialogue.Length > 0)
        {
            //play call sound
            if(callSound != null)
            {
                callSound.Play();
            }
            fadeCover.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            dialogueText.text = "";
            dialogueBG.gameObject.SetActive(true);
            //slide in dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 25);
                //yield return new WaitForSeconds(0.0005f);
                yield return new WaitForFixedUpdate();
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            yield return new WaitForSecondsRealtime(0.2f);
            //go through dialogue
            int pointer = 0;
            while(pointer < dialogue.Length)
            {
                //type out the message
                string toType = dialogue[pointer];
                string written = "";
                while(!written.Equals(toType))//while untyped
                {
                    written = toType.Substring(0, written.Length + 1);
                    dialogueText.text = written;
                    if(clickSound != null)
                    {
                        clickSound.Play();
                    }
                    if((trySkip) || (tryfullSkip))//speed up if key pressed
                    {
                        //yield return new WaitForSeconds(0.005f);
                        trySkip = false;
                        yield return new WaitForFixedUpdate();
                        break;
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.05f);
                    }
                    yield return null;
                }
                //see if full skipping
                if(tryfullSkip)
                {
                    tryfullSkip = false;
                    break;
                }
                dialogueText.text = dialogue[pointer];
                //wait until user input for next message
                while(true)
                {
                    if(Input.anyKeyDown)
                    {
                        pointer++;
                        trySkip = false;
                        //yield return new WaitForSeconds(0.0005f);
                        yield return new WaitForFixedUpdate();
                        break;
                    }
                    yield return null;
                }
            }
            //slide out dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y > -300)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 25);
                //yield return new WaitForSeconds(0.0005f);
                yield return new WaitForFixedUpdate();
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            dialogueBG.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        //fade in
        if(fadeInSound != null)
        {
            fadeInSound.Play();
        }
        fadeCover.gameObject.SetActive(false);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        while(fadeRing.GetComponent<RectTransform>().localScale.x < 30)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x + 0.5f);
            //yield return new WaitForSeconds(0.0005f);
            yield return new WaitForFixedUpdate();
        }
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * 30;
        fadeRing.gameObject.SetActive(false);
        transform.position = Vector3.one * 50;//move stairs far enough away not be be accidentally retriggered
        //re-enable player movement
        player.SetMovementCooldown(0.1f);
        //show level name
        levelName.color = new Color(1,1,1,0);
        float colAlph = 0;
        while(colAlph < 0.8f)
        {
            levelName.color = levelName.color + new Color(0,0,0, 0.025f);
            colAlph += 0.025f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForFixedUpdate();
        }
        levelName.color = new Color(1,1,1,0.8f);
        colAlph = 0.8f;
        yield return new WaitForSeconds(1.5f);
        while(colAlph > 0)
        {
            levelName.color = levelName.color - new Color(0,0,0, 0.025f);
            colAlph -= 0.025f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForFixedUpdate();
        }
        levelName.color = new Color(1,1,1,0);
        //destroy this object
        Destroy(gameObject);
    }

    IEnumerator FadeInAtStartAnim()
    {  
        //disable player movement
        player.SetMovementCooldown(100000);

        //if there is dialogue, go through that first
        if(dialogue.Length > 0)
        {
            //play call sound
            if(callSound != null)
            {
                callSound.Play();
            }
            fadeCover.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            dialogueText.text = "";
            dialogueBG.gameObject.SetActive(true);
            //slide in dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 25);
                //yield return new WaitForSeconds(0.0005f);
                yield return new WaitForFixedUpdate();
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            yield return new WaitForSecondsRealtime(0.2f);
            //go through dialogue
            int pointer = 0;
            while(pointer < dialogue.Length)
            {
                //type out the message
                string toType = dialogue[pointer];
                string written = "";
                while(!written.Equals(toType))//while untyped
                {
                    written = toType.Substring(0, written.Length + 1);
                    dialogueText.text = written;
                    if(clickSound != null)
                    {
                        clickSound.Play();
                    }
                    if((trySkip) || (tryfullSkip))//speed up if key pressed
                    {
                        //yield return new WaitForSeconds(0.005f);
                        trySkip = false;
                        yield return new WaitForFixedUpdate();
                        break;
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.05f);
                    }
                    yield return null;
                }
                //see if full skipping
                if(tryfullSkip)
                {
                    tryfullSkip = false;
                    break;
                }
                dialogueText.text = dialogue[pointer];
                //wait until user input for next message
                while(true)
                {
                    if(Input.anyKeyDown)
                    {
                        pointer++;
                        trySkip = false;
                        //yield return new WaitForSeconds(0.0005f);
                        yield return new WaitForFixedUpdate();
                        break;
                    }
                    yield return null;
                }
            }
            //slide out dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y > -300)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 25);
                //yield return new WaitForSeconds(0.0005f);
                yield return new WaitForFixedUpdate();
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            dialogueBG.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        //fade in
        if(fadeInSound != null)
        {
            fadeInSound.Play();
        }
        fadeCover.gameObject.SetActive(false);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        while(fadeRing.GetComponent<RectTransform>().localScale.x < 30)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x + 0.5f);
            //yield return new WaitForSeconds(0.0005f);
            yield return new WaitForFixedUpdate();
        }
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * 30;
        fadeRing.gameObject.SetActive(false);
        transform.position = Vector3.one * 50;//move stairs far enough away not be be accidentally retriggered
        //re-enable player movement
        player.SetMovementCooldown(0.1f);
        //show level name
        levelName.color = new Color(1,1,1,0);
        float colAlph = 0;
        while(colAlph < 0.8f)
        {
            levelName.color = levelName.color + new Color(0,0,0, 0.025f);
            colAlph += 0.025f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForFixedUpdate();
        }
        levelName.color = new Color(1,1,1,0.8f);
        colAlph = 0.8f;
        yield return new WaitForSeconds(1.5f);
        while(colAlph > 0)
        {
            levelName.color = levelName.color - new Color(0,0,0, 0.025f);
            colAlph -= 0.025f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForFixedUpdate();
        }
        levelName.color = new Color(1,1,1,0);
    }
}
