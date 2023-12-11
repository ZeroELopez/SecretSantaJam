using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    BoxCollider2D[] thisCollider2D = new BoxCollider2D[0];
    //[SerializeField] string obj;

    // Start is called before the first frame update
    [SerializeField]PlayerMovement obj;
    void Awake()
    {
        thisCollider2D = GetComponents<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        //filter.useTriggers = false;
        thisCollider2D[0].OverlapCollider(filter, allCollisions);

        foreach (BoxCollider2D c in allCollisions)
        {
            if (c == null)
                continue;

            if (!c.gameObject.TryGetComponent(typeof(PlayerMovement), out Component com))
                continue;
            Debug.Log("OnStill");

            onStill(com);

            if (obj != null)
                return;

            Debug.Log("OnEnter");

            obj = (PlayerMovement)com;
            onEnter(obj);

            return;
        }

        if (obj == null)
            return;

        Debug.Log("OnExit");
        onExit(obj);
        obj = null;
            
    }

    public virtual void onEnter(Component script) { }
    public virtual void onStill(Component script) { }
    public virtual void onExit(Component script) { }

}
