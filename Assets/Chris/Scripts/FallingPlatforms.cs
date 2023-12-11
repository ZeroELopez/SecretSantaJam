using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    public float fallDelay = 1f;
    //public float destroyDelay = 2f;
    public float resetDelay = 4f;
    private Vector2 initialPosition;
    //private bool isFalling = false;

    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        this.initialPosition = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        //Destroy(gameObject, destroyDelay);
        yield return new WaitForSeconds(resetDelay);
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        //yield return new WaitForSeconds(resetDelay);
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = initialPosition;
    }
}
