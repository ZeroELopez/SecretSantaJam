using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [Tooltip("paths to follow")]
    [SerializeField] public PathScript[] paths;
    [Tooltip("The speed the object will follow the path")]
    [SerializeField] public float speed;
    [SerializeField] public float zOffset;

    private int pathToGo;
    private float t;
    private Vector3 position;
    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        pathToGo = 0;
        t = 0;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed) 
        {
            StartCoroutine(GoByThePath(pathToGo));
        }
    }

    private IEnumerator GoByThePath(int pathNumber)
    {
        coroutineAllowed = false;

        Vector2 p0 = paths[pathNumber].controlPoints[0].position;
        Vector2 p1 = paths[pathNumber].controlPoints[1].position;
        Vector2 p2 = paths[pathNumber].controlPoints[2].position;
        Vector2 p3 = paths[pathNumber].controlPoints[3].position;

        while (t < 1) 
        {
            t += Time.deltaTime * speed;
            position = Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
            position.z = zOffset;
            transform.position = position;
            yield return new WaitForEndOfFrame();
        }

        t = 0;
        pathToGo += 1;
        if (pathToGo > paths.Length - 1) 
        {
            pathToGo = 0;
        }

        coroutineAllowed = true;
    }
}
