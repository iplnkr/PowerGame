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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                //todo add dialogue change for alternate ending
                if(levelToLoad == -3)
                {
                    //TODO check ending and add dialogue
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
        fadeRing.gameObject.SetActive(true);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * 30;
        while(fadeRing.GetComponent<RectTransform>().localScale.x > 1)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x - 0.1f);
            //also move player towards stairs centre
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(transform.position.x, transform.position.y, -1), 0.01f);
            yield return new WaitForSeconds(0.0005f);
        }
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        fadeCover.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.0005f);
        //detach this object so it isnt destroyed on loading
        transform.parent = null;
        //load next level
        level.ResetFloor();
        level.GenerateFloorForLevel(levelToLoad);

        //if there is dialogue, go through that first
        if(dialogue.Length > 0)
        {
            fadeCover.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            dialogueText.text = "";
            dialogueBG.gameObject.SetActive(true);
            //slide in dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 5);
                yield return new WaitForSeconds(0.0005f);
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            yield return new WaitForSeconds(0.2f);
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
                    if(Input.anyKey)//speed up if key pressed
                    {
                        yield return new WaitForSeconds(0.005f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.05f);
                    }
                    //skip typing
                    if(Input.anyKeyDown)
                    {
                        break;
                    }
                    yield return null;
                }
                dialogueText.text = dialogue[pointer];
                //wait until user input for next message
                while(true)
                {
                    if(Input.anyKeyDown)
                    {
                        pointer++;
                        yield return new WaitForSeconds(0.0005f);
                        break;
                    }
                    yield return null;
                }
            }
            //slide out dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y > -300)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 5);
                yield return new WaitForSeconds(0.0005f);
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            dialogueBG.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        //fade in
        fadeCover.gameObject.SetActive(false);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        while(fadeRing.GetComponent<RectTransform>().localScale.x < 30)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x + 0.1f);
            yield return new WaitForSeconds(0.0005f);
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
            yield return new WaitForSeconds(0.05f);
        }
        levelName.color = new Color(1,1,1,0.8f);
        colAlph = 0.8f;
        yield return new WaitForSeconds(1.5f);
        while(colAlph > 0)
        {
            levelName.color = levelName.color - new Color(0,0,0, 0.025f);
            colAlph -= 0.025f;
            yield return new WaitForSeconds(0.05f);
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
            fadeCover.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            dialogueText.text = "";
            dialogueBG.gameObject.SetActive(true);
            //slide in dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 5);
                yield return new WaitForSeconds(0.0005f);
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            yield return new WaitForSeconds(0.2f);
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
                    if(Input.anyKey)//speed up if key pressed
                    {
                        yield return new WaitForSeconds(0.005f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.05f);
                    }
                    //skip typing
                    if(Input.anyKeyDown)
                    {
                        break;
                    }
                    yield return null;
                }
                dialogueText.text = dialogue[pointer];
                //wait until user input for next message
                while(true)
                {
                    if(Input.anyKeyDown)
                    {
                        pointer++;
                        yield return new WaitForSeconds(0.0005f);
                        break;
                    }
                    yield return null;
                }
            }
            //slide out dialogue box
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            while(dialogueBG.GetComponent<RectTransform>().anchoredPosition.y > -300)
            {
                dialogueBG.GetComponent<RectTransform>().anchoredPosition = dialogueBG.GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 5);
                yield return new WaitForSeconds(0.0005f);
            }
            dialogueBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
            dialogueBG.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        //fade in
        fadeCover.gameObject.SetActive(false);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        while(fadeRing.GetComponent<RectTransform>().localScale.x < 30)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x + 0.1f);
            yield return new WaitForSeconds(0.0005f);
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
            yield return new WaitForSeconds(0.05f);
        }
        levelName.color = new Color(1,1,1,0.8f);
        colAlph = 0.8f;
        yield return new WaitForSeconds(1.5f);
        while(colAlph > 0)
        {
            levelName.color = levelName.color - new Color(0,0,0, 0.025f);
            colAlph -= 0.025f;
            yield return new WaitForSeconds(0.05f);
        }
        levelName.color = new Color(1,1,1,0);
    }
}
