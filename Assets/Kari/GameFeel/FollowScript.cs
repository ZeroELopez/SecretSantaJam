using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FollowScript : MonoBehaviour
{
    public Transform followObj;
    Vector3 offset;

    [SerializeField] float speed;
    [SerializeField] AnimationCurve curve;

    public static FollowScript mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<Camera>())
            mainCamera = this;

        offset = transform.position - followObj.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.MoveTowards(
            transform.position, 
            followObj.position + offset,
            Time.deltaTime * speed * curve.Evaluate(Vector3.Distance(transform.position, followObj.position + offset)));

        transform.position = newPos;
    }
}
