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
                loot.transform.parent = transform.parent;
                loot.transform.localScale = new Vector3(Mathf.Abs(loot.transform.localScale.x), Mathf.Abs(loot.transform.localScale.y), Mathf.Abs(loot.transform.localScale.z));
                loot.SetActive(true);
                if(roomImIn != null)
                {
                    roomImIn.Death();
                }
                Destroy(gameObject);
            }
            healthDisplay.transform.parent.gameObject.SetActive(true);
            healthDisplay.SetActive(true);
            healthDisplay.transform.localScale = new Vector3(currentHealth/maxHealth, 1, 1);
            healthDisplay.transform.localPosition = new Vector3(-(1-(currentHealth/maxHealth))/2f, 0, 0);
        }
        //deal damage
        if(toDamagePlayer)
        {
            playerX.TakeDamage();
        }
        //Allow Inheritors to add on to fixed update
        if(startedMoving)
        {
            FixedUpdateAddOn();
        }
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
