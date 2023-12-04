using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPlayerBackToSpawn : MonoBehaviour
{
    BoxCollider2D[] thisCollider2D;

    PlayerMovement player;
    Vector3 spawn;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        thisCollider2D = GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D c in thisCollider2D)
            c.isTrigger = true;

        SetSpawn();
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
                return;

            if (c.gameObject.GetComponent<PlayerMovement>())
                c.gameObject.transform.position = spawn;
        }

    }

    public void SetSpawn() => spawn = player.transform.position;
}
