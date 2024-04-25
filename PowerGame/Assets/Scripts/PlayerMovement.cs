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
    [SerializeField] private Image batteryMask;
    [SerializeField] private Image batteryIcon;
    [SerializeField] private Image batteryMask2;
    [SerializeField] private Image batteryIcon2;
    [SerializeField] private Image batteryMask3;
    [SerializeField] private Image batteryIcon3;
    [SerializeField] private Image batteryMask4;
    [SerializeField] private Image batteryIcon4;
    [SerializeField] private Image batteryMask5;
    [SerializeField] private Image batteryIcon5;
    private float batteryMax = 100;
    private float batteryPercent = 100;
    private float batteryDecayRate = 50;
    private float batteryChargeRate = 35;

    //hp
    [SerializeField] private Text healthText;
    private float healthMax = 5;
    private float healthCurrent = 5;
    private bool isImmune = false;
    private float immuneCooldown;
    [SerializeField] private EyeMovement eyeMov;//used for invincibility animation
    [SerializeField] private Image fadeRing;//used for death screen
    [SerializeField] private Image fadeCoverDeath;//used for death screen
    [SerializeField] private Image winUI;//used for win screen

    //money
    [SerializeField] private Text moneyText;
    private float moneyCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthCurrent = healthMax;
        batteryPercent = batteryMax;
        moneyCurrent = 0;
        healthText.text = " " + healthCurrent;
        moneyText.text = " " + moneyCurrent;
        batteryMask.GetComponent<RectTransform>().localPosition = new Vector3(-8, 0, 0);
        batteryIcon.GetComponent<RectTransform>().localPosition = new Vector3(8, 0, 0);
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

            //cheat code for healing
            if((Input.GetKey(",")) && ((Input.GetKey("left shift")) || (Input.GetKey("right shift"))))
            {
                if(Input.GetKeyDown("3"))
                {
                    GainHP(1);
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
        batteryMask.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110))) + (batteryMask.GetComponent<RectTransform>().rect.size.x/2f), (batteryMask.GetComponent<RectTransform>().rect.size.y/2f), 0);
        batteryIcon.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110)))), 0, 0);
        batteryMask2.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102))) + (batteryMask2.GetComponent<RectTransform>().rect.size.x/2f), (batteryMask2.GetComponent<RectTransform>().rect.size.y/2f), 0);
        batteryIcon2.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102)))), 0, 0);
        batteryMask3.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102))) + (batteryMask3.GetComponent<RectTransform>().rect.size.x/2f), (batteryMask3.GetComponent<RectTransform>().rect.size.y/2f), 0);
        batteryIcon3.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102)))), 0, 0);
        batteryMask4.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102 - 102))) + (batteryMask3.GetComponent<RectTransform>().rect.size.x/2f), (batteryMask3.GetComponent<RectTransform>().rect.size.y/2f), 0);
        batteryIcon4.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102 - 102)))), 0, 0);
        batteryMask5.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102 - 102 - 102))) + (batteryMask3.GetComponent<RectTransform>().rect.size.x/2f), (batteryMask3.GetComponent<RectTransform>().rect.size.y/2f), 0);
        batteryIcon5.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(Mathf.Max(-110, Mathf.Min(-8, ((1.02f * batteryPercent) - 110 - 102 - 102 - 102 - 102)))), 0, 0);
        //immunity cooldown
        immuneCooldown = immuneCooldown - Time.fixedDeltaTime;
        if(immuneCooldown <= 0)
        {
            isImmune = false;
            torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.r, torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.g, torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.b, 1);            
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
            eyeMov.InvinceSet(false);        
        }
        //do immunity animation
        else
        {
            torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.r, torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.g, torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.b, 0.2f + 0.8f * Mathf.Sin(Time.timeSinceLevelLoad * 32));
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.2f + 0.8f * Mathf.Sin(Time.timeSinceLevelLoad * 32));
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
            healthText.text = " " + healthCurrent;
            if(healthCurrent <= 0)
            {
                //make immune for a long time (about a day irl?) to avoid retriggering
                immuneCooldown = 100000;
                isImmune = true;
                //destroy all nearby enemies
                Enemy[] ens = FindObjectsOfType<Enemy>();
                for(int i = ens.Length - 1; i >= 0; i--)
                {
                    Destroy(ens[i]);
                }
                //start fade out animation
                StartCoroutine("FadeDeathAnim");
            }
            else
            {
                isImmune = true;
                eyeMov.InvinceSet(true);
                immuneCooldown = 2;
            }
        }
    }

    IEnumerator FadeDeathAnim()
    {        
        GameObject camPos = FindObjectOfType<Camera>().gameObject;
        //disable player movement
        SetMovementCooldown(1000000);
        //fade out
        fadeRing.gameObject.SetActive(true);
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * 30;
        while(fadeRing.GetComponent<RectTransform>().localScale.x > 1)
        {
            fadeRing.GetComponent<RectTransform>().localScale = Vector3.one * (fadeRing.GetComponent<RectTransform>().localScale.x - 0.1f);
            transform.position = Vector3.Lerp(transform.position, new Vector3(camPos.transform.position.x, camPos.transform.position.y, -1), 0.01f);
            yield return new WaitForSeconds(0.0005f);
        }
        fadeRing.GetComponent<RectTransform>().localScale = Vector3.one;
        fadeCoverDeath.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.0005f);
    }

    //function for doing game win animation
    public void WinGame()
    {
        //disable collider
        GetComponent<Collider2D>().enabled = false;
        //destroy all nearby enemies
        Enemy[] ens = FindObjectsOfType<Enemy>();
        for(int i = ens.Length - 1; i >= 0; i--)
        {
            Destroy(ens[i]);
        }
        //start fade out animation
        StartCoroutine("WinAnim");
    }

    IEnumerator WinAnim()
    {        
        GameObject camPos = FindObjectOfType<Camera>().gameObject;
        Vector3 mid = new Vector3(camPos.transform.position.x, camPos.transform.position.y, -1);
        //disable player movement
        SetMovementCooldown(1000000);
        yield return new WaitForSeconds(0.5f);
        //player walks to mid
        while(Vector3.Distance(transform.position, mid) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, mid, 0.01f);
            yield return new WaitForSeconds(0.005f);
        }
        //player walks out
        mid = new Vector3(mid.x, mid.y - 15, -1);
        while(Vector3.Distance(transform.position, mid) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, mid, 0.01f);
            yield return new WaitForSeconds(0.005f);
        }
        //show end message
        winUI.gameObject.SetActive(true);
    }

    public void GainHP(int amt)
    {
        healthCurrent = healthCurrent + amt;
        healthText.text = " " + healthCurrent;
    }

    public void GainBattery()
    {
        batteryMax = batteryMax + 25;
    }

    public void GainGold(int amt)
    {
        moneyCurrent = moneyCurrent + amt;
        moneyText.text = " " + moneyCurrent;
    }

    public bool CanLoseGold(int amt)
    {
        return(amt <= moneyCurrent);
    }

    public void LoseGold(int amt)
    {
        moneyCurrent = moneyCurrent - amt;
        moneyText.text = " " + moneyCurrent;
    }
}
