using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : TriggerScript
{
    //[SerializeField] FollowScript cameraScript;
    //private void Start()
    //{
    //    cameraScript = Camera.main.GetComponentInParent<FollowScript>();
    //}
    public override void onEnter(Component script)
    {
        FollowScript.mainCamera.followObj = transform;
    }

    public override void onExit(Component script)
    {
        FollowScript.mainCamera.followObj = script.transform;
    }
}
