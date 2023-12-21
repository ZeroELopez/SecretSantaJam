using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EmuSniperReticle : MonoBehaviour
{
    [SerializeField]
    private EmuSniper sniperParent;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Handle when the Player enters the Circle Collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if Player has entered the collider
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            ///Ignore the player if they are already stunned.
            if (player.IsStunned == false)
            {
                //inform Sniper Parent that the Player has entered the collider.
                //If so, kick start the coroutine.
                sniperParent.TakeAim();
            }
        }
    }

    public IEnumerator Blink(float seconds)
    {
        float t = seconds;
        int reps = 0;
        while(t > 0)
        {
            t-= Time.deltaTime;
            reps++;

            ///Alternate colors
            if(reps%2 == 0)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = Color.black;
            }

            yield return new WaitForEndOfFrame();
        }

        //Set it back to its original state at the end.
        spriteRenderer.color = Color.white;
    }
}
