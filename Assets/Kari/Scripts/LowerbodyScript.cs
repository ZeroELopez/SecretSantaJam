using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kari.SoundManagement;

public enum PhysicsState
{
    isFalling, onGround, onWall
}

[RequireComponent(typeof(BoxCollider2D))]
public class LowerbodyScript : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

    BoxCollider2D[] thisCollider2D;

    public static PhysicsState state;

    public static int wallDirection;
    public static string floorType;
    //public static float width;
    //public static float height;
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
    Vector3 prevPos;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position == prevPos)
            return;

        //Creates the collider array that will recieve all the overlaping colliders
        Collider2D[] allCollisions = new Collider2D[10];
        //Filter out triggers and only use solid colliders
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        //Reset on ground every frame. Player is not on ground until proven so
        state = PhysicsState.isFalling;

        foreach (BoxCollider2D c in thisCollider2D)
        {
            //Okay so I wanted the size of the colliders to be a variable playtesters and designers could edit.
            //width and height are connected to the PlayerMovement.lowerBodySize. 
            //c.size = new Vector2(c.size.x > c.size.y ? width : c.size.x,
            //    c.size.y > c.size.x ? height : c.size.y);

            //Get all overlaping colliders
            if (c.OverlapCollider(filter, allCollisions) == 0)
                continue;
             
            //Will ignore overlapping collider if it is the player
            bool ignore = true;

            foreach (BoxCollider2D t in allCollisions)
                if (t != null && !t.gameObject.GetComponent<PlayerMovement>())
                {
                    //If the box collider's width is large then it's an upper body and needs to find what direction is the wall.
                    wallDirection = c.size.x >= 1 ? t.ClosestPoint(transform.position).x > transform.position.x ? 1 : -1 : 0;
                    //Parent player to platform if it is grounded
                    if (wallDirection == 0)
                    {
                        floorType = t.tag;
                        if (player.transform.parent == null)
                            AudioManager.PlaySound("Landon" + floorType,GetComponent<AudioSource>(),"LandonRock");
                    }
                    player.transform.parent = t.transform;

                    ignore = false;
                }

            if (ignore)
                continue;

            //Player is touching an object. The naming is misleading. As this will be true even if just touching a wall
            //Knew that the name was misleading. Adding onWall variable now
            state = wallDirection == 0? PhysicsState.onGround : PhysicsState.onWall;

            prevPos = transform.position;
            return;
        }

        if (state == PhysicsState.isFalling)
            player.transform.parent = null;

        prevPos = transform.position;
    }
}
