using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFloorScript : MonoBehaviour
{
    [Tooltip("max speed will be multiplied by this when player walks on floor")]
    [SerializeField] float maxSpeedModifier;
    [Tooltip("jump force will be multiplied by this when player walks on floor")]
    [SerializeField] float jumpModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player) 
        {
            player.forceModifier = new Vector2(1, jumpModifier);
            player.maxSpeedModifier = maxSpeedModifier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            player.forceModifier = Vector2.one;
            player.maxSpeedModifier = 1;
        }
    }
}
