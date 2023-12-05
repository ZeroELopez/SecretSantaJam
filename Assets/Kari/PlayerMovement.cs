using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D thisRigidbody;

    [Header("Horizontal Movement")]
    [Tooltip("normal movement speed")]
    [SerializeField] float walkSpeed;
    [Tooltip("The horizontal maximum speed the player can travel when not dashing")]
    [SerializeField] float maxWalkSpeed;
    [Tooltip("the variable that slows to a stop")]
    [SerializeField] float slowdown;

    [Header("Jumping")]
    [Tooltip("The strength of the jump")]
    [SerializeField] float jumpStrength;
    [Tooltip("The gravity scale when player is jumping")]
    [SerializeField] float upwardsGravity;
    [Tooltip("The gravity scale when player is falling")]
    [SerializeField] float downwardsGravity;
    [Tooltip("THe length of time a jump keeps the horizontal movement")]
    [SerializeField] float jumpKeepHorizontalMomentum;
    [Tooltip("the variable that slows to a horizontal stop in midair")]
    [SerializeField] float midairSlowdown;

    [Header("Dashing")]
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

    [Header("Wall Interactions")]
    [Tooltip("The strength of the wall jump")]
    [SerializeField] float wallJumpHorizontalForce;
    [Tooltip("The strength of the wall jump")]
    [SerializeField] float wallJumpVerticalForce;
    [SerializeField] float wallJumpMomentumTime = .1f;
    [SerializeField] float wallClimbForce;

    //ALL THE TIMES! Each of these times are to control the length of one of the forces above
    float jHFoce = 0;
    float jHTime = 0;

    float wJTime = 0;

    float dTime = 0;

    bool _dashing;

    private bool dashButtonDown;
    private bool jumpButtonDown;
    private float moveDir;
    private bool climbing;

    public static bool still;//Used when the player needs to stand still for a cutscene or going into camera mode.

    private Controls playerControls;

    Vector2 force;
    float movementSpeed;
    float maxSpeed;
    float wallJumpMomentum;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        playerControls = new Controls();
        playerControls.Enable();

        //Set up Input System callbacks;
        playerControls.Actions.Move.started += OnMove;
        playerControls.Actions.Move.performed += OnMove;
        playerControls.Actions.Move.canceled += OnMove;
        playerControls.Actions.Jump.started += OnJump;
        playerControls.Actions.Jump.performed += OnJump;
        playerControls.Actions.Jump.canceled += OnJump;
        playerControls.Actions.Dash.started += OnDash;
        playerControls.Actions.Dash.canceled += OnDash;

        force = Vector2.zero;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>().x;
        climbing = (context.started || context.performed) && context.ReadValue<Vector2>().y > 0;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpButtonDown = context.started || context.performed;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        dashButtonDown = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Fix the issue when in Camera mode, the player keeps moving
        if (still)
        {
            if (LowerbodyScript.onGround)
                thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, slowdown), thisRigidbody.velocity.y);

            return;
        }

        //Reset the Force every frame.
        force = Vector2.zero;

        LowerbodyScript.width = lowerBodySize.x;LowerbodyScript.height = lowerBodySize.y;
        showDashTime = dTime;

        if (dTime < dashTiming && _dashing)
            dTime += Time.deltaTime;
        else if (!dashButtonDown && dTime > 0)
            dTime -= Time.deltaTime;

        //Wall Jump Time
        wJTime = wJTime < wallJumpMomentumTime ? wJTime + Time.deltaTime : wJTime;

        //Jump Horizontal(?) time
        jHTime = jHTime < jumpKeepHorizontalMomentum ? jHTime + Time.deltaTime : jHTime;

        //Handle Jumping
        if (LowerbodyScript.onGround && jumpButtonDown)
        {
            jHTime = 0;
            jHFoce = thisRigidbody.velocity.x;
            wallJumpMomentum = -LowerbodyScript.dir * wallJumpHorizontalForce;
            force.y += LowerbodyScript.dir == 0? jumpStrength : wallJumpVerticalForce;

            if (LowerbodyScript.dir != 0)
            {
                wJTime = 0;
                thisRigidbody.velocity = Vector2.zero;
            }
        }

        //Climbing
        if (LowerbodyScript.onGround && LowerbodyScript.dir != 0)
        {
            force.y += climbing ? wallClimbForce : 0;

            thisRigidbody.velocity = Vector2.zero;
        }

        //Set the Horizontal Force Vector to the direction vector set on move
        force.x = moveDir;

        //Dashing and walk Speed
        _dashing = dTime < dashTiming ? dashButtonDown : false;

        movementSpeed = _dashing ? dashSpeed: walkSpeed;
        maxSpeed = _dashing || (maxSpeed == maxDashSpeed && !LowerbodyScript.onGround)? maxDashSpeed: maxWalkSpeed;

        //Calculate Horizontal force (used in velocity calculations)
        force.x *= movementSpeed;
        force.x += jHTime < jumpKeepHorizontalMomentum ? jHFoce * Mathf.InverseLerp(0, jumpKeepHorizontalMomentum, jHTime) : 0;
        force.x += wJTime < wallJumpMomentumTime ? wallJumpMomentum : 0;

        //Set the Velocity
        thisRigidbody.velocity += (force);

        if (force.x == 0)
            thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, LowerbodyScript.onGround? slowdown : midairSlowdown), thisRigidbody.velocity.y);

        thisRigidbody.velocity = new Vector2(Mathf.Clamp(thisRigidbody.velocity.x, -maxSpeed, maxSpeed), thisRigidbody.velocity.y);
        
        if (thisRigidbody.velocity.y > 0)
            thisRigidbody.gravityScale = upwardsGravity;
        else
            thisRigidbody.gravityScale = downwardsGravity;
    }

}
