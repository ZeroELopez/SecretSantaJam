using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform followObj;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            followObj.position ,
            Time.deltaTime * speed);

        transform.position = newPos;
    }
}
