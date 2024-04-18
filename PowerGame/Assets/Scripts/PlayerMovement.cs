using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private static float movementCooldown;//cooldown for movement after passing a door
    [SerializeField] private float movementSpeed = 6;
    private float speedX, speedY;
    private Rigidbody2D rb;

    //torch movement
    [SerializeField] private GameObject torch;
    [SerializeField] private GameObject torchLight;
    [SerializeField] private Text batteryText;
    private float batteryMax = 100;
    private float batteryPercent = 100;
    private float batteryDecayRate = 50;
    private float batteryChargeRate = 25;

    //hp
    [SerializeField] private Text healthText;
    private float healthMax = 3;
    private float healthCurrent = 3;
    private bool isImmune = false;
    private float immuneCooldown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthCurrent = healthMax;
        batteryPercent = batteryMax;
        healthText.text = "" + healthCurrent;
        batteryText.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        //if player not crossing rooms
        if(movementCooldown <= 0)
        {
            //player movement
            speedX = Input.GetAxisRaw("Horizontal");
            speedY = Input.GetAxisRaw("Vertical");
            rb.velocity = movementSpeed * new Vector2(speedX, speedY).normalized;
            //torch controls
            if(Input.GetKey(KeyCode.Mouse0))
            {
                batteryPercent = batteryPercent - (batteryDecayRate * Time.deltaTime);
                if(batteryPercent < 0)
                {
                    batteryPercent = 0;
                    torchLight.SetActive(false);
                }
                else
                {
                    torchLight.SetActive(true);
                }
            }
            else
            {
                torchLight.SetActive(false);
                batteryPercent = batteryPercent + (batteryChargeRate * Time.deltaTime);
                if(batteryPercent > batteryMax)
                {
                    batteryPercent = batteryMax;
                }
            }
        }
        else
        {
            torchLight.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        //put movement on cooldown after a door used
        if(movementCooldown > 0)
        {
            movementCooldown = movementCooldown - Time.fixedDeltaTime;
            rb.velocity = new Vector3(0,0,0);
        }
        //torch point at mouse
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        torch.transform.right = mouseWorldPosition - transform.position;
        //battery visuals
        batteryText.text = "" + Mathf.RoundToInt((batteryPercent / batteryMax) * 100);
        //immunity cooldown
        immuneCooldown = immuneCooldown - Time.fixedDeltaTime;
        if(immuneCooldown <= 0)
        {
            isImmune = false;
        }
    }

    //when loading new room, time to wait before enabling movement
    public void SetMovementCooldown(float tim)
    {
        movementCooldown = tim;
    }


    public void TakeDamage()
    {
        if(!isImmune)
        {
            healthCurrent = healthCurrent - 1;
            healthText.text = "" + healthCurrent;
            if(healthCurrent <= 0)
            {
                Debug.Log("Death");//TODO add proper death
            }
            isImmune = true;
            immuneCooldown = 1;
        }
    }
}
