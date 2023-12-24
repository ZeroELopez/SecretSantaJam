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

    public void SetSpawn()
    {
        //This function is as best possible to be called whenever the player is on the ground.
        //Presumably the player should be parented to a platform

        Transform platform = player.transform.parent;
        if(platform != null)
        {
            BoxCollider2D platformBox = platform.GetComponent<BoxCollider2D>();
            if (platformBox != null)
            {
                spawn.x = platform.transform.position.x + platformBox.size.x + platformBox.offset.x;
                spawn.y = platform.transform.position.y + ((platformBox.size.y * transform.lossyScale.y) / 2) + platformBox.offset.y;

                RaycastHit2D[] results = new RaycastHit2D[5];
                    
                Physics2D.Raycast(spawn, Vector2.down, new ContactFilter2D(),results, platformBox.size.y * transform.lossyScale.y);
                
                foreach(RaycastHit2D hit in results)
                    if (hit.collider == platformBox)
                        spawn = hit.point;



                spawn.z = 0;
            }
            else
            {
                //Debug.LogError("Box Collider not found on platform."); 

                //Fall back solution
                spawn = player.transform.position;
            }
        }
        else
        {
            //Debug.LogError("Player is not parented to a platform!"); 
            
            //Fall back
            spawn = player.transform.position;
        }


        //
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onEscapeMode>(this);
        LowerbodyScript.onLand += SetSpawn;
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onEscapeMode>(this);
        LowerbodyScript.onLand -= SetSpawn;
    }

    public void HandleEvent(onEscapeMode evt)
    {
        SetSpawn();
    }
}
