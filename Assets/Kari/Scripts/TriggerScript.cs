using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    Transform target;
    [SerializeField] float maxDistance = 5;
    float largestSide;

    [SerializeField] BoxCollider2D[] thisCollider2D = new BoxCollider2D[0];
    //[SerializeField] string obj;

    public UnityEvent onStillEvents;
    public UnityEvent onEnterEvents;
    public UnityEvent onExitEvents;

    // Start is called before the first frame update
    public PlayerMovement obj { get; private set; }
    void Awake()
    {
        target = GameObject.FindObjectOfType<PlayerMovement>().transform;
        thisCollider2D = GetComponents<BoxCollider2D>();
        largestSide =  transform.lossyScale.x * thisCollider2D[0].size.x >  transform.lossyScale.y * thisCollider2D[0].size.y ?
             transform.lossyScale.x * thisCollider2D[0].size.x :
             transform.lossyScale.y * thisCollider2D[0].size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (thisCollider2D.Length == 0)
        //    return;

        Vector3 pos = transform.position + new Vector3(thisCollider2D[0].offset.x, thisCollider2D[0].offset.y);
        if (Vector2.Distance(target.position, pos)  > maxDistance + (largestSide / 2) && !obj)
            return;

        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        //filter.useTriggers = false;
        thisCollider2D[0].OverlapCollider(filter, allCollisions);

        

        foreach (Collider2D c in allCollisions)
        {
            if (c == null)
                continue;

            if (!c.gameObject.TryGetComponent(typeof(PlayerMovement), out Component com))
                continue;
            //Debug.Log("OnStill");

            onStill(com);
            onStillEvents?.Invoke();
            if (obj != null)
                return;

            //Debug.Log("OnEnter");

            obj = (PlayerMovement)com;
            onEnter(obj);
            onEnterEvents?.Invoke();
            return;
        }

        if (obj == null)
            return;

        //Debug.Log("OnExit");
        onExit(obj);
        onExitEvents?.Invoke();

        obj = null;
            
    }

    public virtual void onEnter(Component script) { }
    public virtual void onStill(Component script) { }
    public virtual void onExit(Component script) { }

}
