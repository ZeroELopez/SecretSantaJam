using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFall : MonoBehaviour, ISubscribable<onEscapeMode>
{
    public float pfallDelay = 1f;
    public float destroyDelay = 5f;

    [SerializeField] private Rigidbody2D rb;

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(pfallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }

    public void HandleEvent(onEscapeMode evt)
    {
        StartCoroutine(Fall());
    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onEscapeMode>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onEscapeMode>(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
