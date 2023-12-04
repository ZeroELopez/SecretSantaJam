using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LowerbodyScript : MonoBehaviour
{
    BoxCollider2D[] thisCollider2D;

    public static bool onGround;
    public static int dir;

    public static float width;
    public static float height;
    // Start is called before the first frame update
    void Awake()
    {
        //Get All Colliders the object has
        thisCollider2D = GetComponents<BoxCollider2D>();
        //Currently there are two colliders
        //first checks for objects below player
        //second checks for objects beside player (for wall jumping and climbing)

        //Just make sure they are triggers to not cause any problems later
        foreach(BoxCollider2D c in thisCollider2D)
        c.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Creates the collider array that will recieve all the overlaping colliders
        Collider2D[] allCollisions = new Collider2D[10];
        //Filter out triggers and only use solid colliders
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        //Reset on ground every frame. Player is not on ground until proven so
        onGround = false;

        foreach (BoxCollider2D c in thisCollider2D)
        {
            //Okay so I wanted the size of the colliders to be a variable playtesters and designers could edit.
            //width and height are connected to the PlayerMovement.lowerBodySize. 
            c.size = new Vector2(c.size.x > .1f ? width : .1f,
                c.size.y > .1f ? height : .1f);

            //Get all overlaping colliders
            if (c.OverlapCollider(filter, allCollisions) == 0)
                continue;
             
            //Will ignore overlapping collider if it is the player
            bool ignore = true;

            foreach (BoxCollider2D t in allCollisions)
                if (t != null && !t.gameObject.GetComponent<PlayerMovement>())
                {
                    dir = c.size.x >= 1 ? t.ClosestPoint(transform.position).x > transform.position.x ? 1 : -1 : 0;
                    ignore = false;
                }

            if (ignore)
                continue;
            
            //Player is touching an object. The naming is misleading. As this will be true even if just touching a wall
            onGround = true;

            return;
        }

    }
}
