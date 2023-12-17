using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;


//this script sends player back to a certain point 
//if touched. it is for pitfalls
public class SendPlayerBackToSpawn : MonoBehaviour, ISubscribable<onEscapeMode>
{
    BoxCollider2D[] thisCollider2D;

    PlayerMovement player;
    Vector3 spawn;
    // Start is called before the first frame update
    void Start()
    {
        Subscribe();

        player = GameObject.FindObjectOfType<PlayerMovement>();
        thisCollider2D = GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D c in thisCollider2D)
            c.isTrigger = true;

        SetSpawn();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        //filter.useTriggers = false;
        thisCollider2D[0].OverlapCollider(filter, allCollisions);

        foreach (BoxCollider2D c in allCollisions)
        {
            if (c == null)
                return;

            if (c.gameObject.GetComponent<PlayerMovement>())
            {
                c.gameObject.transform.position = spawn;
                c.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

    }

    public void SetSpawn() => spawn = player.transform.position;

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onEscapeMode>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onEscapeMode>(this);
    }

    public void HandleEvent(onEscapeMode evt)
    {
        SetSpawn();
    }
}
