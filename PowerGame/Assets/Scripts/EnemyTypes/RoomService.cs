using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomService : MonoBehaviour
{
    private GameObject targetEnemy;
    private int xMult = 1;
    private int yMult = 1;
    [SerializeField] private GameObject tipJar;
    [SerializeField] private GameObject tipCoin;
    [SerializeField] private GameObject lanternLight;
    private bool tippingMode = false;
    private bool canSummon = true;
    private bool entering = false;
    private bool smokeCopyMade = false;
    private Vector3 spawnLocation;
    private GameObject lifeSmoke;
    private bool summonedInRoomAlready = false;

    //tip checking
    private float neededTips = 0;
    private float givenTips = 0;
    private bool isEvil = false;

    // Start is called before the first frame update
    void Start()
    {
        targetEnemy = gameObject;
    }

    void FixedUpdate()
    {
        if(entering)
        {
            //do only once
            if(!smokeCopyMade)
            {
                //do entrance smoke animation
                spawnLocation = FindObjectOfType<PlayerMovement>().transform.position;
                //spare smoke backup is stored as first grandchild of palette
                GameObject spareSmoke = FindObjectOfType<RoomPalette>().transform.GetChild(0).transform.GetChild(0).gameObject;

                smokeCopyMade = true;
                lifeSmoke = Instantiate(spareSmoke);
                lifeSmoke.transform.localScale = Vector3.one * 1.65f;
                lifeSmoke.transform.position = spawnLocation + new Vector3(0, -0.4f, -2.2f);
                lifeSmoke.SetActive(true);
                lifeSmoke.transform.parent = null;
                if(lifeSmoke.GetComponent<Animator>() != null)
                {
                    if((lifeSmoke.GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.Smoke"))) && (lifeSmoke.transform.GetChild(0).GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.Smoke"))))
                    {
                        lifeSmoke.GetComponent<Animator>().Play("Base Layer.Smoke");
                        lifeSmoke.transform.GetChild(0).GetComponent<Animator>().Play("Base Layer.Smoke");
                    }
                }

                //make appear from smoke
                Invoke("MakeVisible", 0.25f);
            }
            //if smoke done
            if((smokeCopyMade) && (lifeSmoke == null))
            {
                lanternLight.SetActive(true);
                entering = false;
                xMult = 1;
                yMult = 1;
            }
        }
        else
        {
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            if(allEnemies.Length == 0)
            {
                //when no enemies show tip jar
                tipJar.SetActive(true);
                tippingMode = true;
                //if within range, show coin, else dont
                if(Vector3.Distance(transform.position, FindObjectOfType<PlayerMovement>().transform.position) <= 2)
                {
                    tipCoin.SetActive(true);
                }
                else
                {
                    tipCoin.SetActive(false);
                }
            }
            else
            {
                tipJar.SetActive(false);
                tipCoin.SetActive(false);
                //find closest enemy
                targetEnemy = FindClosest(allEnemies);
                //move towards closest enemy
                Vector3 destination = Vector3.Normalize((targetEnemy.transform.position - transform.position) - new Vector3(0,0,(targetEnemy.transform.position - transform.position).z)) * 5f * Time.fixedDeltaTime;
                transform.position = new Vector3(transform.position.x + destination.x * xMult, transform.position.y + destination.y * yMult, transform.position.z);
            }
        }
    }

    private void MakeVisible()
    {
        transform.position = spawnLocation;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //dont do if being summoned
        if(!entering)
        {
            if(targetEnemy != null)
            {
                //dont block on collision with torch, player and collectables
                if(((!col.isTrigger) && (col.GetComponent<PlayerMovement>() == null)) || (col.GetComponent<Enemy>() != null))
                {
                    Vector3 colDir = Vector3.Normalize(col.transform.position - transform.position);
                    Vector3 movDir = Vector3.Normalize(targetEnemy.transform.position - transform.position);
                    Vector3 angleDif = Vector3.Normalize(colDir - movDir);
                    //block direction based on collision
                    if(Mathf.Abs(angleDif.x) <= Mathf.Abs(angleDif.y))
                    {
                        xMult = 0;
                    }
                    if(Mathf.Abs(angleDif.y) <= Mathf.Abs(angleDif.x))
                    {
                        yMult = 0;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //dont do if being summoned
        if(!entering)
        {
            //dont block on collision with torch, player and collectables
            if(((!col.isTrigger) && (col.GetComponent<PlayerMovement>() == null)) || (col.GetComponent<Enemy>() != null))
            {
                xMult = 1;
                yMult = 1;
            }
        }
    }

    //find closest, assuming at least 1 exists
    private GameObject FindClosest(Enemy[] arrayOfAll)
    {
        if(arrayOfAll.Length > 0)
        {
            Enemy closest = arrayOfAll[0];
            float dist = Vector3.Distance(transform.position, closest.transform.position);
            for(int i = 1; i < arrayOfAll.Length; i++)
            {
                float newdist = Vector3.Distance(transform.position, arrayOfAll[i].transform.position);
                if(newdist < dist)
                {
                    closest = arrayOfAll[i];
                    dist = newdist;
                }
            }
            return closest.gameObject;
        }
        return null;
    }

    public bool CanSummon()
    {
        return canSummon;
    }

    public void SummonMe()
    {
        if(!isEvil)//cant summon if evil
        {
            if(!summonedInRoomAlready)
            {
                summonedInRoomAlready = true;
                smokeCopyMade = false;
                entering = true;
                lanternLight.SetActive(false);
                transform.position = new Vector3(5000, 5000, transform.position.z);
                neededTips += 2;//increase needed tip each time summoned
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public bool CanTip()
    {
        return tippingMode;
    }

    public void TipMe()
    {
        StartCoroutine("TipAnim");
    }

    private IEnumerator TipAnim()
    {
        tipCoin.transform.localPosition = new Vector3(0, 3f, 0.5f);
        for(int i = 0; i < 200; i++)
        {
            tipCoin.SetActive(true);
            tipCoin.transform.position = Vector3.Lerp(tipCoin.transform.position, tipJar.transform.position, 0.05f);
            yield return new WaitForSeconds(0.001f);
        }
        givenTips++;
        yield return new WaitForSeconds(0.5f);
        tipCoin.transform.localPosition = new Vector3(0, 3f, 0.5f);
    }

    //reset use and disappear
    public void Vanish()
    {
        summonedInRoomAlready = false;
        gameObject.SetActive(false);
    }

    public bool ShouldGoEvil()
    {
        return (givenTips < neededTips);
    }

    public void GoEvil()
    {
        isEvil = true;
    }
}
