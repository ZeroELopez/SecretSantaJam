using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KangarooAttackBox : MonoBehaviour
{
    /// <summary>
    /// Direction in which the player should be pushed. This should be set in the parent object (KangarooBoss)
    /// </summary>
    [HideInInspector]
    public Vector2 attackForce;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Process if collision is Player
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player is not null)
        {
            player.StunPlayer();
            player.PushPlayer(attackForce);
        }
    }
}
