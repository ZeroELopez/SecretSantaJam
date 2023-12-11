using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathParent : MonoBehaviour
{
    public List<PathScript> paths = new List<PathScript>();

    public void CreateNewPath()
    {
        GameObject obj = Instantiate(paths[paths.Count - 1].gameObject);
        PathScript script = obj.GetComponent<PathScript>();

        obj.transform.position = paths[paths.Count - 1].transform.position;

        paths.Add(script);
        obj.transform.parent = transform;

        Vector3 diff = script.controlPoints[3].position - script.controlPoints[0].position;

        script.controlPoints[0].position = script.controlPoints[3].position;
        script.controlPoints[3].position += diff;

    }
}
