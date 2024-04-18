using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool firing = false;
    private float lifetime = 5;
    [SerializeField] private float speed = 5;

    //shoot the bullet
    public void Fire()
    {
        firing = true;
    }

    void FixedUpdate()
    {
        if(firing)
        {
            lifetime = lifetime - Time.fixedDeltaTime;
            transform.position = transform.position + (transform.right * speed * Time.fixedDeltaTime);
        }
        if(lifetime < 0)
        {
            ExplodeBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if hit player, deal damage
        if(col.GetComponent<PlayerMovement>() != null)
        {
            col.GetComponent<PlayerMovement>().TakeDamage();
        }
        //dont destroy on collision with torch, enemies and collectables
        if(!col.isTrigger)
        {
            ExplodeBullet();
        }
    }

    private void ExplodeBullet()
    {
        Destroy(gameObject);
    }
}
