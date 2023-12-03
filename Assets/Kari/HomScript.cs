using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HomScript : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    public static bool EndAvailable = false;

    public void setAvailable(bool set) => EndAvailable = set;

    public UnityEvent onGameWon;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EndAvailable)
            return;

        Collider2D[] allCollisions = new Collider2D[10];

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        if (boxCollider2D.OverlapCollider(filter, allCollisions) == 0)
            return;


        foreach (BoxCollider2D t in allCollisions)
            if (t != null && t.gameObject.GetComponent<PlayerMovement>())
                onGameWon?.Invoke();

    }
}
