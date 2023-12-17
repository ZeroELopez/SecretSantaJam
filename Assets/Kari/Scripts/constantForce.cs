using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constantForce : TriggerScript
{
    [SerializeField] Vector2 force;
    [SerializeField] Vector2 acceleration;

    Vector2 trueForce;
    int f = 0;
    public override void onStill(Component script)
    {
        base.onStill(script);
        trueForce = force + (acceleration * f);
        ((PlayerMovement)script).addForce += force;
    }

    public override void onExit(Component script)
    {
        base.onExit(script);
        f = 0;
    }
}
