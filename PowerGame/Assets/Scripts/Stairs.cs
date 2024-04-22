using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stairs : MonoBehaviour
{
    [SerializeField] LevelController level;
    [SerializeField] private PlayerMovement player;
    [SerializeField] int levelToLoad;
    [SerializeField] Image fadeRing;
    [SerializeField] Image fadeCover;
    private bool used = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<PlayerMovement>() != null)
        {
            if(!used)
            {
                used = true;
                StartCoroutine("FadeOutAndInAnim");
            }
        }
    }

    IEnumerator FadeOutAndInAnim()
    {        
        //disable player movement
        player.SetMovementCooldown(10000);
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
        //re-enable player movement
        player.SetMovementCooldown(0.1f);
        //destroy this object
        Destroy(gameObject);
    }
}
