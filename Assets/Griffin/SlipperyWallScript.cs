using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyWallScript : MonoBehaviour
{
    [Tooltip("how fast the player will slip down this wall")]
    [SerializeField] float wallMaxVelocity;

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
            player.wallMaxVelocity = wallMaxVelocity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            player.wallMaxVelocity = 0;
        }
    }
}
