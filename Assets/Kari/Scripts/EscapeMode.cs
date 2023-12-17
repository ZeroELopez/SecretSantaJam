using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class EscapeMode : MonoBehaviour, ISubscribable<onEscapeMode>
{
    [SerializeField] GameObject bucket;
    [SerializeField] GameObject mainCreature;
    public void HandleEvent(onEscapeMode evt)
    {
        bucket.SetActive(true);
        mainCreature.SetActive(true);

        Vector2 pos = Creature.focusCreature.transform.position;
        mainCreature.transform.position = pos;
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
