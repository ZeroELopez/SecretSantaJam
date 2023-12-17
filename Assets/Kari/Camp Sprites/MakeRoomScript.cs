using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoomScript : MonoBehaviour
{
    [SerializeField]AnimationCurve curve;
    [SerializeField] float length;
    [SerializeField] float fps = 0.01666f;

    [SerializeField] Vector3 adjust;

    public void StartAdjust() => StartCoroutine("MoveTowards");

    // Update is called once per frame
    IEnumerator MoveTowards()
    {
        Debug.Log("Started MoveTowards");
        Vector3 target = transform.position + adjust;
        Vector3 origin = transform.position;

        for(float time = 0; time < length;time+=fps)
        {
            transform.position = Vector3.LerpUnclamped(origin, target, curve.Evaluate(time/length));

            yield return new WaitForSeconds(fps);
        }
    }
}
