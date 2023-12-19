using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The non lethal flunkies of the Kangaroo boss.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class KangarooMook : MonoBehaviour
{
    //Design: When the player enters the trigger, teleport boss to the mook's location, boss then attacks.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Process if collision is Player
        //If so, Send an event.
        if (collision.GetComponent<PlayerMovement>())
        {
            Debug.Log("Send Kangaroo");
            //Post Event
            EventHub.Instance.PostEvent<TransportKangarooBoss>(new TransportKangarooBoss { newLocation = transform.position }) ;
        }
    }
}
