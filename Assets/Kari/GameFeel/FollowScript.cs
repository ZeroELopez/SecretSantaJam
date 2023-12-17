using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FollowScript : MonoBehaviour
{
    [SerializeField] bool enableBounds;
    [SerializeField]Vector4 bounds;
    [SerializeField] Transform followObj;

    Transform followingObj;

    public void SetSecondary(Transform newObj)=>        followingObj = newObj;
    public void SetPrimary() => followingObj = followObj;

    Transform prevObj;
    public void TempSwitch(Transform newObj)
    {
        prevObj = followingObj;
        SetSecondary(newObj);
    }
    public void DeletePrevObj() => prevObj = null;
    public void EndSwitch()
    {
        if (!prevObj)
        {
            SetPrimary();
            return;
        }

        followingObj = prevObj;
    }

    Vector3 offset;

    [SerializeField] float speed;
    [SerializeField] AnimationCurve curve;

    public static FollowScript mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<Camera>())
            mainCamera = this;

        followingObj = followObj;
        offset = transform.position - followingObj.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            followingObj.position + offset,
            Time.deltaTime * speed * curve.Evaluate(Vector3.Distance(transform.position, followingObj.position + offset)));

        if (enableBounds)
        {
            newPos.x = Mathf.Clamp(newPos.x, bounds.x, bounds.z);
            newPos.y = Mathf.Clamp(newPos.y, bounds.y, bounds.w);
        }

        transform.position = newPos;
    }

    //float fps = 0.01666f;
    //IEnumerator followObj()
    //{

    //    yield return new WaitForSeconds(fps);
    //}
}
