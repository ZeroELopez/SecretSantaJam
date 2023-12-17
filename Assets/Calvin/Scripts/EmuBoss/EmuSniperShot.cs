using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EmuSniperShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check what entered the hitbox
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        //If it's a player then put Player in the Stun State temporarily.
        if (player != null)
        {
            //StunPlayer returns whether or not it stuns the player
            player.StunPlayer();
        }
    }
}
