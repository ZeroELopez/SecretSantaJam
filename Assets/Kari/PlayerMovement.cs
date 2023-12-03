using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D thisRigidbody;

    [Tooltip("normal movement speed")]
    [SerializeField] float walkSpeed;
    [Tooltip("The horizontal maximum speed the player can travel when not dashing")]
    [SerializeField] float maxWalkSpeed;
    [Tooltip("the variable that slows to a stop")]
    [SerializeField] float slowdown;

    [Tooltip("The strength of the jump")]
    [SerializeField] float jumpStrength;
    [Tooltip("The gravity scale when player is jumping")]
    [SerializeField] float upwardsGravity;
    [Tooltip("The gravity scale when player is falling")]
    [SerializeField] float downwardsGravity;

    [Tooltip("Dash force")]
    [SerializeField] float dashSpeed;
    [Tooltip("Max speed when player is dashing")]
    [SerializeField] float maxDashSpeed;
    [Tooltip("The amount of time spent dashing")]
    [SerializeField] float dashTiming;
    [Tooltip("READ ONLY! For reference of how long player has been dashing and the cool down")]
    [SerializeField] float showDashTime;

    [Tooltip("The size of the collider that judges if the character is grounded or against a wall. 1 is the normal size of the player")]
    [SerializeField] Vector2 lowerBodySize = new Vector2(1.2f,0);

    [Tooltip("The strength of the wall jump")]
    [SerializeField] float wallJumpHorizontalForce;
    [Tooltip("The strength of the wall jump")]
    [SerializeField] float wallJumpVerticalForce;

    float dTime = 0;

    bool _dashing;

    public static bool still;
    
    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
    }


    float movementSpeed;
    float maxSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (still)
            return;

        LowerbodyScript.width = lowerBodySize.x;LowerbodyScript.height = lowerBodySize.y;
        showDashTime = dTime;

        if (dTime < dashTiming && _dashing)
            dTime += Time.deltaTime;
        else if (!Input.GetKey(KeyCode.LeftShift) && dTime > 0)
            dTime -= Time.deltaTime;

        Vector2 force = Vector2.zero;

        if (LowerbodyScript.onGround && Input.GetKey(KeyCode.UpArrow))
        {
            force.x = -LowerbodyScript.dir * wallJumpHorizontalForce;
            force.y += LowerbodyScript.dir == 0? jumpStrength : wallJumpVerticalForce;

            if (LowerbodyScript.dir != 0)
                thisRigidbody.velocity = Vector2.zero;

        }


        if (Input.GetKey(KeyCode.LeftArrow))
            force.x += -1;
        if (Input.GetKey(KeyCode.RightArrow))
            force.x += 1;



        _dashing = dTime < dashTiming ? Input.GetKey(KeyCode.LeftShift) : false;


        movementSpeed = _dashing ? dashSpeed: walkSpeed;
        maxSpeed = _dashing || (maxSpeed == maxDashSpeed && !LowerbodyScript.onGround)? maxDashSpeed: maxWalkSpeed;


        force.x *= movementSpeed;


        thisRigidbody.velocity += (force);

        if (force.x == 0)
            thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, slowdown), thisRigidbody.velocity.y);

        thisRigidbody.velocity = new Vector2(Mathf.Clamp(thisRigidbody.velocity.x, -maxSpeed, maxSpeed), thisRigidbody.velocity.y);
        //thisRigidbody.velocity = Vector2.MoveTowards(
        //    thisRigidbody.velocity,
        //                new Vector2(Mathf.Clamp(thisRigidbody.velocity.x, -maxSpeed, maxSpeed), thisRigidbody.velocity.y),
        //                slowdown * Time.deltaTime);



        if (thisRigidbody.velocity.y > 0)
            thisRigidbody.gravityScale = upwardsGravity;
        else
            thisRigidbody.gravityScale = downwardsGravity;
    }

}
