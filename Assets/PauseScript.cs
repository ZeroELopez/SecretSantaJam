using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class PauseScript : SubscribeBehavior<TogglePause>
{
    [SerializeField] GameObject pauseMenu;

    public override void HandleEventVirtual(TogglePause evt)
    {
        Debug.Log("Pause Created");
        pauseMenu.SetActive(true);
    }
}

//Testing out something for future projects
public class SubscribeBehavior<TEvent> : MonoBehaviour, ISubscribable<TEvent> where TEvent : DispatchableEvent
{
    private void Start() => Subscribe();
    private void OnDestroy() => Unsubscribe();

    public void HandleEvent(TEvent evt)=>        HandleEventVirtual(evt);

    public virtual void HandleEventVirtual(TEvent evt) { }
    public void Subscribe()
    {
        EventHub.Instance.Subscribe<TEvent>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<TEvent>(this);
    }
}