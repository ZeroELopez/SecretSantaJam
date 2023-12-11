using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMomentum : TriggerScript
{
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    Vector3 lastPos;
    Vector3 force;

    [SerializeField] float multiplier = 10;

    public override void onEnter(Component script)
    {
        script.transform.parent = transform;
    }
    // Update is called once per frame
    public override void onStill(Component script) 
    {
        //Momentum From Platforms
        force = (transform.position - lastPos) * multiplier * Time.deltaTime;

        ((PlayerMovement)script).addForce += new Vector2(force.x,force.y);
        lastPos = transform.position;
    }

    public override void onExit(Component script)
    {
        script.transform.parent = null;
    }

}
