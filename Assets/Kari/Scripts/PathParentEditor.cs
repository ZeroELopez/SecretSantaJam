using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PathParent))]
public class PathParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathParent parent = (PathParent)target;

        if (GUILayout.Button("Create New Path"))
            parent.CreateNewPath();
    }
}
#endif