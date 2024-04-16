using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;
    [SerializeField] private GameObject healthDisplay;
    private bool toBurn = false;
    private bool toDamagePlayer = false;
    private PlayerMovement playerX;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //take damage
        if(toBurn)
        {
            currentHealth = currentHealth - (4 * Time.fixedDeltaTime);
            if(currentHealth < 0)
            {
                currentHealth = 0;
                //TODO add drops
                Debug.Log("Drop Loot");
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
    }
}
