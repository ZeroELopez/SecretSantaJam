using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Home : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetHome(GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if Player has entered the collider
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        //Check if player is Valid AND GameManager's State is escape.
        if (player != null && GameManager.Instance.state == GameState.Escape)
        {
            Debug.Log("GameWon");
            EventHub.Instance.PostEvent(new onGameWon());
        }
    }
}
