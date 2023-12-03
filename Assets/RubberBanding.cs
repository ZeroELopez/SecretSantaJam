using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBanding : MonoBehaviour
{
    Animator thisAnimator;

    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
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

    // Update is called once per frame
    void Update()
    {
        playerPos.x = player.position.x;playerPos.y = player.position.y;
        creaturePos.x = transform.position.x;creaturePos.y = transform.position.y;

        distance = Vector2.Distance(playerPos, creaturePos);

        speed = Mathf.InverseLerp(minDistance, maxDistance, distance);
        speed = Mathf.Clamp(speed, 0, 1);
        speed = 1 - speed;

        thisAnimator.speed = speed;
    }
}
