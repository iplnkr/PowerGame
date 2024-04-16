using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static float movementCooldown;//cooldown for movement after passing a door
    [SerializeField] private float movementSpeed = 6;
    private float speedX, speedY;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movementCooldown <= 0)
        {
            speedX = Input.GetAxisRaw("Horizontal");
            speedY = Input.GetAxisRaw("Vertical");
            rb.velocity = movementSpeed * new Vector2(speedX, speedY).normalized;
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
    }

    public void SetMovementCooldown(float tim)
    {
        movementCooldown = tim;
    }
}
