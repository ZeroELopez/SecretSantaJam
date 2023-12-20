using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class EscapeMode : MonoBehaviour, ISubscribable<onEscapeMode>
{
    [SerializeField] GameObject bucket;
    [SerializeField] GameObject mainCreature;
    [SerializeField] float timer = 30;
    public void HandleEvent(onEscapeMode evt)
    {
        bucket.SetActive(true);
        if (mainCreature) 
        {
            mainCreature.SetActive(true);
        }
        
        GameManager.Instance.SetTimer(timer);

        Vector2 pos = Creature.focusCreature.transform.position;
        if (mainCreature)
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
