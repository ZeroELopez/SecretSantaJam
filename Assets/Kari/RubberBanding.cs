using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

public class RubberBanding : MonoBehaviour
{
    [SerializeField] MonoBehaviour thisAnimator;

    [SerializeField] Transform player;
    FieldInfo fieldInfo;
    // Start is called before the first frame update
    void Start()
    {
        if (!thisAnimator)
        thisAnimator = GetComponent<FollowPath>();

        fieldInfo = thisAnimator.GetType().GetField("speed");
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

    float speed;
    float direction = 1;
    public void SetDirection(int newDir) => direction = newDir;


    void Update()
    {
        playerPos.x = player.position.x;playerPos.y = player.position.y;
        creaturePos.x = transform.position.x;creaturePos.y = transform.position.y;

        distance = Vector2.Distance(playerPos, creaturePos);

        speed = Mathf.InverseLerp(minDistance, maxDistance, distance);
        speed = Mathf.Clamp(speed, 0, 1);
        speed = 1 - speed;

        fieldInfo.SetValue(thisAnimator, Mathf.Lerp(minSpeed, maxSpeed, speed) * direction);
    }
}
