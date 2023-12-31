using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

namespace Kari.Animations
{
    public enum States
    {
        ONGROUND,
        Standing,
        Running,
        Dashing,

        MIDAIR,
        VerticalJump_Falling, VerticalJump_Neutral, VerticalJump_Rising,
        DiagnolJump_Falling, DiagnolJump_Neutral, DiagnolJump_Rising,
        LongJump_Falling, LongJump_Neutral, LongJump_Rising,


        ONWALL,
        WallCling,
        WallClimb,

        CAMERA,
        onCamera_Standing,
        onCamera_Moving,
        onCamera_InAir,
        onCamera_onWall,

        STATE,
        Hitstun
    }

    public class StateAnimation : MonoBehaviour, ISubscribable<onCameraToggle>
    {
        [SerializeField] PlayerMovement player;

        [SerializeField] States currentState = States.Standing;
        Animator thisAnimator;

        Rigidbody2D thisRigidbody => player.GetComponent<Rigidbody2D>();
        Vector3 velocity => thisRigidbody.velocity;

        bool onGround => LowerbodyScript.state == PhysicsState.onGround;

        bool onWall => LowerbodyScript.state == PhysicsState.onWall;

        public bool onCamera;
        Vector3 localScale;
        private void Start()
        {
            thisAnimator = GetComponent<Animator>();
            localScale = transform.localScale;
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
        // Update is called once per frame
        void Update()
        {
            States newState = FindState();

            //FOR TIME SENSITIVE ATTACKS!!!

            if (newState != States.WallCling && newState != States.WallClimb)
            {
                if (player.moveDirection > 0)
                    transform.localScale = new Vector3(localScale.x, localScale.y);
                else if (player.moveDirection < 0)
                    transform.localScale = new Vector3(-localScale.x, localScale.y);
            }


            
            //if (newState >= CharacterStates.AttackDown && newState <= CharacterStates.AttackUp3)
            //{
            //    float finalTime = (Time.time - AttackstartTime) / attackFrames/*player.attackFrames*/;
            //    thisAnimator.Play((state = newState).ToString(), 0, finalTime);

            //    return;
            //}
            // if (thisAnimator.HasState(0, newState.ToString()))

            if (currentState != newState)
            {
                thisAnimator.Play((currentState = newState).ToString(), 0,0);
            }

        }

        [SerializeField] float runningThreshold;
        [SerializeField] float dashingThreshold;

        States FindState()
        {
            if (player.IsStunned)
                return States.Hitstun;

            if (onCamera)
                return CameraState();

            if (onWall)
                return WallStates();

            if (!onGround)
                return MidAirState();

            States state = States.Standing;

            state += Mathf.Abs(velocity.x) >= runningThreshold ? 1 : 0;
            state += Mathf.Abs(velocity.x) >= dashingThreshold ? 1 : 0;


            return state;
        }

        void StartAttack()
        {
            currentState = States.Standing;
        }

        States WallStates()
        {
            if (velocity.y > 0)
                return States.WallClimb;

            return States.WallCling;
        }

        [SerializeField] float diagnolThreshold;
        [SerializeField] float longJumpThreshold;

        [SerializeField] float risingThreshold;
        [SerializeField] float fallingThreshold;

        
        States MidAirState()
        {
            int state = (int)States.MIDAIR + 2;

            state += Mathf.Abs(velocity.x) >= diagnolThreshold ? 3 : 0;
            state += Mathf.Abs(velocity.x) >= longJumpThreshold ? 3 : 0;

            if (state == (int)States.LongJump_Neutral)
                return States.LongJump_Rising;

            state += velocity.y >= risingThreshold ? 1 : 0;
            state += velocity.y <= fallingThreshold ? -1 : 0;

            return (States)state;
        }

        States CameraState()
        {
            if (onWall)
                return States.onCamera_onWall;

            if (!onGround)
                return States.onCamera_InAir;

            if (Mathf.Abs(velocity.x) > 0)
                return States.onCamera_Moving;

            return States.onCamera_Standing;
        }


        //public void Hitstun() => currentState = States.Hitstun;

        //public void Death() => currentState = States.Dead;

        public void Neutral()
        {
            currentState = States.Standing;
        }

        public void Subscribe()
        {
            EventHub.Instance.Subscribe<onCameraToggle>(this);
        }

        public void Unsubscribe()
        {
            EventHub.Instance.Subscribe<onCameraToggle>(this);
        }

        public void HandleEvent(onCameraToggle evt)
        {
            onCamera = evt.On;
        }
    }
}