using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KangarooBoss : MonoBehaviour, ISubscribable<TransportKangarooBoss>
{
    /// <summary>
    /// The amount of time in seconds for the boss disappear
    /// </summary>
    [SerializeField]
    private float teleportOutWindup;

    /// <summary>
    /// Amoutn of time needed for the animation of teleporting in.
    /// </summary>
    [SerializeField]
    private float teleportInTime;

    /// <summary>
    /// Time in seconds before the attack hitbox is triggered.
    /// </summary>
    [SerializeField]
    private float attackWindupTime;

    /// <summary>
    /// Time in seconds for how long the attack lasts.
    /// </summary>
    [SerializeField]
    private float attackDuration;

    /// <summary>
    /// The Attack Hitbox for the Boss.
    /// Recommended that this be a child object of the boss.
    /// </summary>
    [SerializeField]
    private KangarooAttackBox AttackHitboxObject;

    /// <summary>
    /// Vector of Force to push the player.
    /// </summary>
    [SerializeField]
    private Vector2 AttackForce;

    private bool isAttacking;
    string animationName;

    public void HandleEvent(TransportKangarooBoss evt)
    {
        //Ensure we are not in the middle of an existing attack.
        if(!isAttacking)
        {
            animationName = evt.animation;
            AttackHitboxObject.attackForce = evt.pushback;
            StartCoroutine(AttackRoutine(evt.newLocation));
        }
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
        isAttacking = false;

        //Set Attack force of the Kangaroo AttackBox;
        AttackHitboxObject.attackForce = AttackForce;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Unsubscribe(); 
    }

    private IEnumerator AttackRoutine(Vector3 newLocation)
    {
        isAttacking = true;

        //Wait to account for animation of the kangaroo teleporting in.
        yield return new WaitForSecondsRealtime(teleportInTime);

        //Set Position
        transform.position = newLocation;

        //TODO: Make visually appear.
        GetComponentInChildren<Animator>().Play(animationName,0);
        //Wait to account for attack winding up
        yield return new WaitForSecondsRealtime(attackWindupTime);

        //Enable Attack Hitbox
        //AttackHitboxObject.gameObject.SetActive(true);

        //Wrap up Attack animation
        yield return new WaitForSecondsRealtime(attackDuration);

        //Disable Attack Hitbox
        //AttackHitboxObject.gameObject.SetActive(false);

        //Wait to account for teleporting away
        yield return new WaitForSecondsRealtime(teleportOutWindup);

        //TODO: Set Invisible.

        isAttacking = false;
    }
}
