using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RubberBanding : MonoBehaviour
{
    FollowPath thisAnimator;

    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<FollowPath>();
    }

    Vector2 playerPos;
    Vector2 creaturePos;

    [SerializeField] float distance;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [Min(0)]
    [SerializeField] float maxSpeed;
    [Min(0)]
    [SerializeField] float minSpeed;

    [SerializeField] float speed;
[SerializeField] float direction = 1;

    public enum ChaseState { Explore,Chase,Escape};

    public static ChaseState chaseState = ChaseState.Explore;
    public void changeChaseState(int s) => chaseState = (ChaseState)s;

    [Header("State of the Game Loop")]
    public UnityEvent<int> onChaseStateChange;
    //Minimum speed before the state turns into a Chase
    [SerializeField] float InvestigationThreshold = .1f;
    
    // Update is called once per frame
    void Update()
    {
        playerPos.x = player.position.x;playerPos.y = player.position.y;
        creaturePos.x = transform.position.x;creaturePos.y = transform.position.y;

        distance = Vector2.Distance(playerPos, creaturePos);

        speed = Mathf.InverseLerp(minDistance, maxDistance, distance);
        speed = Mathf.Clamp(speed, 0, 1);
        speed = 1 - speed;

        thisAnimator.speed = speed * direction;

        ChaseState newState = getState();

        if (chaseState != newState)
            onChaseStateChange?.Invoke((int)(chaseState = newState));
    }

    //get the current state based on the speed of the creature. Is it chilling? then we chilling. If not, we not
    ChaseState getState()
    {
        //If already in escape mode then there will not be a change in state
        if (chaseState == ChaseState.Escape)
            return chaseState;

        if (speed <= InvestigationThreshold)
            return ChaseState.Explore;

        return ChaseState.Chase;

    }
}
