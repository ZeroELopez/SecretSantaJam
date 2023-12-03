using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LowerbodyScript : MonoBehaviour
{
    BoxCollider2D[] thisCollider2D;

    public static bool onGround;
    public static int dir;

    public static float width;
    public static float height;
    // Start is called before the first frame update
    void Awake()
    {
        thisCollider2D = GetComponents<BoxCollider2D>();
        
        foreach(BoxCollider2D c in thisCollider2D)
        c.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        //filter.useLayerMask = true;
        //filter.layerMask = (LayerMask.NameToLayer("Default"));
        onGround = false;

        foreach (BoxCollider2D c in thisCollider2D)
        {
            c.size = new Vector2(c.size.x > .1f ? width : .1f,
                c.size.y > .1f ? height : .1f);

            if (c.OverlapCollider(filter, allCollisions) == 0)
                continue;

            bool ignore = true;

            foreach (BoxCollider2D t in allCollisions)
                if (t != null && !t.gameObject.GetComponent<PlayerMovement>())
                    ignore = false;

            if (ignore)
                continue;
            

            dir = c.size.x >= 1 ? allCollisions[0].ClosestPoint(transform.position).x > transform.position.x ? 1 : -1
                : 0;
            onGround = true;

            return;
        }

    }
}
