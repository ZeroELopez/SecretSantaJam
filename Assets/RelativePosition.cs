using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePosition : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;

    [SerializeField] bool x, y;

    [SerializeField] Vector3 min;
    [SerializeField] Vector3 max;

    [SerializeField] Vector3 playerMin;
    [SerializeField] Vector3 playerMax;

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.InverseLerp(playerMin.x,playerMax.x,PlayerMovement.position.x);

        transform.position = Vector3.Lerp(min, max, curve.Evaluate(t));

        //transform.position = new Vector3(
        //    Mathf.Clamp(transform.position.x, min.x, max.x),
        //    Mathf.Clamp(transform.position.y, min.y, max.y),
        //                Mathf.Clamp(transform.position.z, min.z, max.z)

            //);
    }
}
