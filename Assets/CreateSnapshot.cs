using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Base.Events;

public class CreateSnapshot : MonoBehaviour, ISubscribable<onTakePhoto>
{
    [SerializeField]Camera mainCamera;
    public void HandleEvent(onTakePhoto evt)
    {

    }

    public void Subscribe()
    {
        EventHub.Instance.Subscribe<onTakePhoto>(this);
    }

    public void Unsubscribe()
    {
        EventHub.Instance.Unsubscribe<onTakePhoto>(this);

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
