using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Base.Events;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour, ISubscribable<onCutsceneToggle>, ISubscribable<TogglePause>
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

    //[Tooltip("The size of the collider that judges if the character is grounded or against a wall. 1 is the normal size of the player")]
    //[SerializeField] Vector2 lowerBodySize = new Vector2(1.2f,0);

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

    float wJTime = float.MaxValue;

    float dTime = 0;

    bool _dashing;

    private bool dashButtonDown;
    private bool jumpButtonDown;
    public float moveDirection { get; private set; }
  
    private bool climbing;
    public Vector2 forceModifier;
    public float setMaxSpeedMod;
    float maxSpeedModifier;
    public float wallMaxVelocity;

    bool still;//Used when the player needs to stand still for a cutscene or going into camera mode.
    bool cutscene;//Used when the player needs to stand still for a cutscene or going into camera mode.

    private Controls playerControls;

    Vector2 force;
    float movementSpeed;
    float maxSpeed;
    float wallJumpMomentum;

    //Force other scripts can manipulate to move player;
    public Vector2 addForce;

    public static Vector3 position { get; private set; }
    public bool IsStunned
    {  
        get;
        private set; 
    }
    [SerializeField]
    private float stunTimer;

    // Start is called before the first frame update
    void Start()
    {
        EventHub.Instance.PostEvent(new onGameStart());
        //lastPos = transform.position;

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
        playerControls.Actions.CameraToggle.started += OnCameraToggle;
        playerControls.Actions.CameraToggle.canceled += OnCameraToggle;
        playerControls.Actions.Pause.started += OnPauseButton;

        Subscribe();

        force = Vector2.zero;
        forceModifier = Vector2.one;
        maxSpeedModifier = 1;
        setMaxSpeedMod = 1;
        wallMaxVelocity = 0;

        //StartCoroutine("PhysicsUpdate");
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
    private void OnPauseButton(InputAction.CallbackContext context)
    {
        //if (still)
        //    return;

        EventHub.Instance.PostEvent(new TogglePause());
    }
    private void OnMove(InputAction.CallbackContext context)
    {        
        moveDirection = context.ReadValue<Vector2>().x;
        climbing = (context.started || context.performed) && context.ReadValue<Vector2>().y > 0;            
    }

    [SerializeField] float jumpButtonHoldDownTiming;
    float jbhTime = 0;
    private void OnJump(InputAction.CallbackContext context)
    {
        jumpButtonDown = (context.started || context.performed);
        jbhTime = jumpButtonDown? 0 : float.MaxValue;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        dashButtonDown = context.ReadValueAsButton();
    }

    private void OnCameraToggle(InputAction.CallbackContext context)
    {
        EventHub.Instance.PostEvent(new onCameraToggle() { On = context.started });
        still = context.started;
    }

    // Update is called once per frame
    //void FixedUpdate()
    public static Action onPhysics;

    float fps => 0.01666f;
    float interval = 0;
    void Update()
    {
        interval += Time.deltaTime;
        if (interval < fps)
            return;

        //If a large fps drop happens. Just skip the frame just to be safe.
        if (interval > 1)
        {
            interval = 0;
            return;
        }
        while (interval >= fps)
        {
            onPhysics?.Invoke();
            interval -= fps;

            //For scripts that need Player position. Yes I know there are better ways but oh well.
            position = transform.position;

            if (IsStunned)
            {
                //yield return new WaitForSeconds(fps);
                Physics2D.Simulate(fps);
                return;
            }

            maxSpeedModifier = Mathf.MoveTowards(maxSpeedModifier, setMaxSpeedMod, .05f);

            //Fix the issue when in Camera mode, the player keeps moving
            if (still || cutscene)
            {
                if (LowerbodyScript.state == PhysicsState.onGround)
                {
                    thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, slowdown), thisRigidbody.velocity.y) + addForce;
                    addForce = Vector2.zero;
                }

                //yield return new WaitForSeconds(fps);
                Physics2D.Simulate(fps);
                return;// return;
            }

            //Reset the Force every frame.
            force = Vector2.zero;

            //LowerbodyScript.width = lowerBodySize.x;LowerbodyScript.height = lowerBodySize.y;
            showDashTime = dTime;

            if (dTime < dashTiming && _dashing)
                dTime += fps;
            else if (!dashButtonDown && dTime > 0)
                dTime -= fps;

            jbhTime += jumpButtonDown ? fps : 0;
            jumpButtonDown = jbhTime < jumpButtonHoldDownTiming;

            //Wall Jump Time
            wJTime = wJTime < wallJumpMomentumTime ? wJTime + fps : wJTime;

            //Keep Horizontal Momentum for Jump
            jHTime = jHTime < jumpKeepHorizontalMomentum ? jHTime + fps : jHTime;

            //Set the Horizontal Force Vector to the direction vector set on move
            force.x = moveDirection;

            //Handle Jumping
            if (LowerbodyScript.state != PhysicsState.isFalling && jumpButtonDown)
            {
                jHTime = 0;
                jHFoce = thisRigidbody.velocity.x;
                wallJumpMomentum = -LowerbodyScript.wallDirection * wallJumpHorizontalForce;
                force.y += LowerbodyScript.wallDirection == 0 ? jumpStrength : wallJumpVerticalForce;

                if (LowerbodyScript.wallDirection != 0)
                {
                    wJTime = 0;
                    //thisRigidbody.velocity = new Vector2(0, Mathf.Clamp(thisRigidbody.velocity.y, -wallMaxVelocity, 0));
                }
            }

            //Climbing
            if (LowerbodyScript.state == PhysicsState.onWall && LowerbodyScript.wallDirection != 0)
            {
                if (wJTime >= wallJumpMomentumTime)
                    force.x = 0;

                force.y += climbing ? wallClimbForce : 0;

                thisRigidbody.velocity = new Vector2(thisRigidbody.velocity.x, Mathf.Clamp(thisRigidbody.velocity.y, -wallMaxVelocity, 0));
            }

            //Dashing and walk Speed
            _dashing = dTime < dashTiming ? dashButtonDown : false;

            movementSpeed = _dashing ? dashSpeed : walkSpeed;

            maxSpeed = (_dashing || (maxSpeed >= maxDashSpeed && LowerbodyScript.state == PhysicsState.isFalling) ? maxDashSpeed : maxWalkSpeed) * maxSpeedModifier;

            //Calculate Horizontal force (used in velocity calculations)
            force.x *= movementSpeed;
            //force.x += jHTime < jumpKeepHorizontalMomentum ? jHFoce *(1 - Mathf.InverseLerp(0, jumpKeepHorizontalMomentum, jHTime)) : 0;
            force.x += wJTime < wallJumpMomentumTime ? wallJumpMomentum * (1 - Mathf.InverseLerp(0, wallJumpMomentumTime, wJTime)) : 0;

            //Adjust force based on hazards
            force = force * forceModifier;

            //Set the Velocity
            thisRigidbody.velocity += (force);

            if (force.x == 0)
                thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, LowerbodyScript.state == PhysicsState.onGround ? slowdown : midairSlowdown), thisRigidbody.velocity.y);

            thisRigidbody.velocity = new Vector2(Mathf.Clamp(thisRigidbody.velocity.x, -maxSpeed, maxSpeed), thisRigidbody.velocity.y);

            thisRigidbody.velocity += addForce;
            addForce = Vector2.zero;

            if (thisRigidbody.velocity.y > 0)
                thisRigidbody.gravityScale = upwardsGravity;
            else
                thisRigidbody.gravityScale = downwardsGravity;

            //yield return new WaitForSeconds(fps);

            Physics2D.Simulate(fps);
        }
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onCutsceneToggle>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onCutsceneToggle>(this);
    }

    public void HandleEvent(onCutsceneToggle evt)
    {
        EventHub.Instance.PostEvent(new onCameraToggle() { On = false });
        cutscene = evt.On;
    }

    public bool StunPlayer()
    {
        //If we're not already stunned, kick off the coroutine
        if(!IsStunned)
        {
            StartCoroutine(StunCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator StunCoroutine()
    {
        IsStunned = true;
        thisRigidbody.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(stunTimer);
        IsStunned = false;
    }

    public void PushPlayer(Vector2 direction)
    {
        thisRigidbody.AddForce(direction, ForceMode2D.Impulse);
    }

    public void HandleEvent(TogglePause evt)
    {
        still = true;
    }

}



// Update is called once per frame
//void FixedUpdate()
//float fps = 0.01666f;
//IEnumerator PhysicsUpdate()
//{
//    while (true)
//    {
//        position = transform.position;

//        if (IsStunned)
//        {
//            yield return new WaitForSeconds(fps);
//            continue;
//        }

//        maxSpeedModifier = Mathf.MoveTowards(maxSpeedModifier, setMaxSpeedMod, .05f);

//        //Fix the issue when in Camera mode, the player keeps moving
//        if (still || cutscene)
//        {
//            if (LowerbodyScript.state == PhysicsState.onGround)
//            {
//                thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, slowdown), thisRigidbody.velocity.y) + addForce;
//                addForce = Vector2.zero;
//            }

//            yield return new WaitForSeconds(fps);
//            continue;// return;
//        }

//        //Reset the Force every frame.
//        force = Vector2.zero;

//        //LowerbodyScript.width = lowerBodySize.x;LowerbodyScript.height = lowerBodySize.y;
//        showDashTime = dTime;

//        if (dTime < dashTiming && _dashing)
//            dTime += fps;
//        else if (!dashButtonDown && dTime > 0)
//            dTime -= fps;

//        jbhTime += jumpButtonDown ? fps : 0;
//        jumpButtonDown = jbhTime < jumpButtonHoldDownTiming;

//        //Wall Jump Time
//        wJTime = wJTime < wallJumpMomentumTime ? wJTime + fps : wJTime;

//        //Keep Horizontal Momentum for Jump
//        jHTime = jHTime < jumpKeepHorizontalMomentum ? jHTime + fps : jHTime;

//        //Set the Horizontal Force Vector to the direction vector set on move
//        force.x = moveDirection;

//        //Handle Jumping
//        if (LowerbodyScript.state != PhysicsState.isFalling && jumpButtonDown)
//        {
//            jHTime = 0;
//            jHFoce = thisRigidbody.velocity.x;
//            wallJumpMomentum = -LowerbodyScript.wallDirection * wallJumpHorizontalForce;
//            force.y += LowerbodyScript.wallDirection == 0 ? jumpStrength : wallJumpVerticalForce;

//            if (LowerbodyScript.wallDirection != 0)
//            {
//                wJTime = 0;
//                //thisRigidbody.velocity = new Vector2(0, Mathf.Clamp(thisRigidbody.velocity.y, -wallMaxVelocity, 0));
//            }
//        }

//        //Climbing
//        if (LowerbodyScript.state == PhysicsState.onWall && LowerbodyScript.wallDirection != 0)
//        {
//            if (wJTime >= wallJumpMomentumTime)
//                force.x = 0;

//            force.y += climbing ? wallClimbForce : 0;

//            thisRigidbody.velocity = new Vector2(thisRigidbody.velocity.x, Mathf.Clamp(thisRigidbody.velocity.y, -wallMaxVelocity, 0));
//        }

//        //Dashing and walk Speed
//        _dashing = dTime < dashTiming ? dashButtonDown : false;

//        movementSpeed = _dashing ? dashSpeed : walkSpeed;

//        maxSpeed = (_dashing || (maxSpeed >= maxDashSpeed && LowerbodyScript.state == PhysicsState.isFalling) ? maxDashSpeed : maxWalkSpeed) * maxSpeedModifier;

//        //Calculate Horizontal force (used in velocity calculations)
//        force.x *= movementSpeed;
//        //force.x += jHTime < jumpKeepHorizontalMomentum ? jHFoce *(1 - Mathf.InverseLerp(0, jumpKeepHorizontalMomentum, jHTime)) : 0;
//        force.x += wJTime < wallJumpMomentumTime ? wallJumpMomentum * (1 - Mathf.InverseLerp(0, wallJumpMomentumTime, wJTime)) : 0;

//        //Adjust force based on hazards
//        force = force * forceModifier;

//        //Set the Velocity
//        thisRigidbody.velocity += (force);

//        if (force.x == 0)
//            thisRigidbody.velocity = new Vector2(Mathf.MoveTowards(thisRigidbody.velocity.x, 0, LowerbodyScript.state == PhysicsState.onGround ? slowdown : midairSlowdown), thisRigidbody.velocity.y);

//        thisRigidbody.velocity = new Vector2(Mathf.Clamp(thisRigidbody.velocity.x, -maxSpeed, maxSpeed), thisRigidbody.velocity.y);

//        thisRigidbody.velocity += addForce;
//        addForce = Vector2.zero;

//        if (thisRigidbody.velocity.y > 0)
//            thisRigidbody.gravityScale = upwardsGravity;
//        else
//            thisRigidbody.gravityScale = downwardsGravity;

//        yield return new WaitForSeconds(fps);
//    }

//}