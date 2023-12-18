using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementAnimator : MonoBehaviour
{
    Animator thisAnimator;
    BoxCollider2D[] thisCollider2D;
    [SerializeField] int uppderBodyLayer = 0;
    [SerializeField] int lowerBodyLayer = 1;

    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        thisCollider2D = GetComponents<BoxCollider2D>();
    }


    Vector3 prevPos;
    PhysicsState state;
    // Update is called once per frame
    void Update()
    {
        if (transform.position == prevPos)
            return;

        Vector3 dir = (transform.position - prevPos).normalized;
        state = GetState();

        switch (state)
        {
            case PhysicsState.isFalling:
                AirState(dir);
                break;
            case PhysicsState.onGround:
                GroundState(dir);
                break;
        }
        Vector3 scale = Vector3.one;
        scale.x = dir.x < 0 ? 1 : -1;

        transform.localScale = scale;
        prevPos = transform.position;
    }
    void AirState(Vector2 dir)
    {
        if (dir.y > 0)
            thisAnimator.Play("JumpUp", lowerBodyLayer);
        else
            thisAnimator.Play("JumpDown", lowerBodyLayer);
    }
    void GroundState(Vector2 dir)
    {
        if (dir.x != 0)
            thisAnimator.Play("Running", lowerBodyLayer);
        else
            thisAnimator.Play("Standing", lowerBodyLayer);
    }

    PhysicsState GetState()
    {
        //Creates the collider array that will recieve all the overlaping colliders
        Collider2D[] allCollisions = new Collider2D[10];
        //Filter out triggers and only use solid colliders
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        //Reset on ground every frame. Player is not on ground until proven so
        PhysicsState state  = PhysicsState.isFalling;

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

            foreach (Collider2D t in allCollisions)
                if (t != null && !t.gameObject.GetComponent<PlayerMovement>())
                {
                    ignore = false;
                }

            if (ignore)
                continue;

            //Player is touching an object. The naming is misleading. As this will be true even if just touching a wall
            //Knew that the name was misleading. Adding onWall variable now
            //state = wallDirection == 0 ? PhysicsState.onGround : PhysicsState.onWall;
            state = PhysicsState.onGround;
        }
        return state;
    }
}
