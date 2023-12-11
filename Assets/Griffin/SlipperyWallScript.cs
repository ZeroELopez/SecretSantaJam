using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyWallScript : MonoBehaviour
{
    [Tooltip("how fast the player will slip down this wall")]
    [SerializeField] float wallMaxVelocity;
    [Tooltip("jump force will be multiplied by this when player climbs on this wall")]
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
            player.wallMaxVelocity = wallMaxVelocity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            player.forceModifier = Vector2.one;
            player.wallMaxVelocity = 0;
        }
    }
}
