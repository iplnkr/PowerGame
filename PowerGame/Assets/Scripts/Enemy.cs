using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 10;
    protected float currentHealth;
    [SerializeField] private GameObject healthDisplay;
    protected bool toBurn = false;
    protected bool toDamagePlayer = false;
    private PlayerMovement playerX;
    [SerializeField] protected RoomLayoutDetails roomImIn;
    private bool startedMoving = false;
    [SerializeField] protected GameObject loot;
    [SerializeField] protected GameObject lightSelf;//the masked version of the character
    private bool smokeCopyMade = false;
    private bool dealsDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if(roomImIn == null)
        {
            roomImIn = FindObjectOfType<RoomLayoutDetails>();
        }
        StartAddOn();
        Invoke("StartActivity", 1.25f);
    }

    //enable movement after room loading
    private void StartActivity()
    {
        startedMoving = true;
    }

    //Allow Inheritors to add on to start by overriding this
    public virtual void StartAddOn()
    {

    }


    void FixedUpdate()
    {
        //take damage
        if(toBurn)
        {
            currentHealth = currentHealth - (4.5f * Time.fixedDeltaTime);
            if(currentHealth < 0)
            {
                currentHealth = 0;
                //drop a coin and destroy enemy
                if(loot != null)
                {
                    loot.transform.parent = transform.parent;
                    loot.transform.localScale = new Vector3(Mathf.Abs(loot.transform.localScale.x), Mathf.Abs(loot.transform.localScale.y), Mathf.Abs(loot.transform.localScale.z));
                    loot.SetActive(true);
                }
                if(roomImIn != null)
                {
                    roomImIn.Death();
                }
                //if has death animation, do it
                if(GetComponent<Animator>() != null)
                {
                    if((GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.SmokeAndDie"))) && (lightSelf.GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.Smoke"))))
                    {
                        GetComponent<SpriteRenderer>().color = new Color(0.3333333f, 0.3333333f, 0.3333333f, 1);
                        lightSelf.GetComponent<Animator>().Play("Base Layer.Smoke");
                        GetComponent<Animator>().Play("Base Layer.SmokeAndDie");
                    }
                }
                else//otherwise create a copy of the death animation to use
                {
                    //smoke for death animation of enemies that dont use an animator
                    //spare smoke backup is stored as first grandchild of palette
                    GameObject spareSmoke = FindObjectOfType<RoomPalette>().transform.GetChild(0).transform.GetChild(0).gameObject;
                    if(!smokeCopyMade)
                    {
                        smokeCopyMade = true;
                        GameObject deathSmoke = Instantiate(spareSmoke);
                        deathSmoke.transform.parent = transform;
                        deathSmoke.transform.localPosition = new Vector3(0, 0, -1);
                        deathSmoke.SetActive(true);
                        deathSmoke.transform.parent = null;
                        if(deathSmoke.GetComponent<Animator>() != null)
                        {
                            if((deathSmoke.GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.Smoke"))) && (deathSmoke.transform.GetChild(0).GetComponent<Animator>().HasState(0, Animator.StringToHash("Base Layer.Smoke"))))
                            {
                                deathSmoke.GetComponent<Animator>().Play("Base Layer.Smoke");
                                deathSmoke.transform.GetChild(0).GetComponent<Animator>().Play("Base Layer.Smoke");
                                SelfDestruct();
                            }
                        }
                    }
                }
                startedMoving = false;
                dealsDamage = false;
                //SelfDestruct();
            }
            healthDisplay.transform.parent.gameObject.SetActive(true);
            healthDisplay.SetActive(true);
            healthDisplay.transform.localScale = new Vector3(currentHealth/maxHealth, 1, 1);
            healthDisplay.transform.localPosition = new Vector3(-(1-(currentHealth/maxHealth))/2f, 0, 0);
        }
        //deal damage
        if(toDamagePlayer)
        {
            if(dealsDamage)
            {
                playerX.TakeDamage();
            }
        }
        //Allow Inheritors to add on to fixed update
        if(startedMoving)
        {
            FixedUpdateAddOn();
        }
    }

    public void SelfDestruct()
    {
        GameObject self = gameObject;
        Destroy(self);
    }

    //Allow Inheritors to add on to fixed update by overriding this
    public virtual void FixedUpdateAddOn()
    {

    }

    //set whether or not an enemy should take damage
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name.Equals("TorchLight"))
        {
            toBurn = true;
        }
        //if hit player, deal damage
        if(col.GetComponent<PlayerMovement>() != null)
        {
            playerX = col.GetComponent<PlayerMovement>();
            toDamagePlayer = true;
        }

        //Allow Inheritors to add on
        if(startedMoving)
        {
            OnTriggerEnter2DAddOn(col);
        }
    }

    //Allow Inheritors to add on to OnTriggerEnter2D by overriding this
    public virtual void OnTriggerEnter2DAddOn(Collider2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.name.Equals("TorchLight"))
        {
            toBurn = false;
        }
        if(col.GetComponent<PlayerMovement>() != null)
        {
            toDamagePlayer = false;
        }
        
        //Allow Inheritors to add on
        if(startedMoving)
        {
            OnTriggerExit2DAddOn(col);
        }
    }

    //Allow Inheritors to add on to OnTriggerExit2D by overriding this
    public virtual void OnTriggerExit2DAddOn(Collider2D col)
    {

    }
}
