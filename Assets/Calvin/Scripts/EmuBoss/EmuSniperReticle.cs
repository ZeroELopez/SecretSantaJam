using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EmuSniperReticle : MonoBehaviour
{
    [SerializeField]
    private EmuSniper sniperParent;

    /// <summary>
    /// Handle when the Player enters the Circle Collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if Player has entered the collider
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            ///Ignore the player if they are already stunned.
            if (player.IsStunned == false)
            {
                //inform Sniper Parent that the Player has entered the collider.
                //If so, kick start the coroutine.
                sniperParent.TakeAim();
            }
        }
    }
}
