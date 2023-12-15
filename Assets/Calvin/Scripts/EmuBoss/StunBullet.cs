using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StunBullet : MonoBehaviour
{
    /// <summary>
    /// Direction of the bullet
    /// </summary>
    public Vector3 direction;

    /// <summary>
    /// How fast the bullet is traveling.
    /// </summary>
    public float speed;

    /// <summary>
    /// How long the bullet should travel before automatically despawning.
    /// </summary>
    [SerializeField]
    private float killTimer;

    /// <summary>
    /// Rigidbody 2d Cached reference
    /// </summary>
    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //Update position
        rigidbody.velocity = direction * speed;
    }

    private void Update()
    {
        ///Automatically desapwn the bullet if the bullet traveled a specific time.
        if (killTimer > 0)
        {
            killTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check what entered the Bullet's hitbox
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        //If it's a player then put Player in the Stun State temporarily.
        if (player != null)
        {
            //StunPlayer ireturns whether or not it stuns the player
            if(player.StunPlayer())
            {
                //If player was successfully stunned, Despawn.
                Destroy(this.gameObject);
            }
        }
    }
}