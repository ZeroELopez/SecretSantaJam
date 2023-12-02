using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kari.Animations
{
    public enum States
    {
        Standing, Hitstun, Dead
    }

    public class StateAnimation : MonoBehaviour
    {
        [SerializeField] States currentState = States.Standing;
        Animator thisAnimator;

        private void Start()
        {
            thisAnimator = GetComponent<Animator>();
        }
        // Update is called once per frame
        void Update()
        {
            States newState = FindState();

            //FOR TIME SENSITIVE ATTACKS!!!

            //if (newState >= CharacterStates.AttackDown && newState <= CharacterStates.AttackUp3)
            //{
            //    float finalTime = (Time.time - AttackstartTime) / attackFrames/*player.attackFrames*/;
            //    thisAnimator.Play((state = newState).ToString(), 0, finalTime);

            //    return;
            //}
            // if (thisAnimator.HasState(0, newState.ToString()))

            thisAnimator.Play((currentState = newState).ToString(), 0);

        }

        States FindState()
        {
            return States.Standing;
        }

        void StartAttack()
        {
            currentState = States.Standing;
        }


        public void Hitstun() => currentState = States.Hitstun;

        public void Death() => currentState = States.Dead;

        public void Neutral()
        {
            currentState = States.Standing;
        }
    }
}